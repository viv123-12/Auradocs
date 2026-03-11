using System.Transactions;
using auradocs_api.Contexts;
using auradocs_api.Models;
using auradocs_api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("[controller]")]
public class DocumentManagerController : ControllerBase
{
    private readonly AuradocsContext _auradocsContext;
    private readonly UserInformationService _userInformationService;
    private readonly IConvertFileService _convertFileToPdf;
    private readonly IAIService _aiService;
    public DocumentManagerController(AuradocsContext auradocsContext, UserInformationService userInformationService, IConvertFileService convertFileToPdf, IAIService aIService)
    {
        _auradocsContext = auradocsContext;
        _userInformationService = userInformationService;
        _convertFileToPdf = convertFileToPdf;
        _aiService = aIService;
    }

    [HttpGet("documents")]
    public async Task<IActionResult> GetDocumentListAsync()
    {
        User user = await _userInformationService.GetUserInformation();
        if(user == null)
        {
            return Unauthorized();
        }
        List<string> documents = await _auradocsContext.Documents
                                        .Where(e => e.uOwnerUserId == user.uUid && !e.boolIsDeleted)
                                        .Select(e => e.strTitle)
                                        .ToListAsync();

        return Ok(documents);
    }

    [HttpGet("folders")]
    public async Task<IActionResult> GetFolderListAsync()
    {
        List<Folder> folders = await _auradocsContext.Folders.Where(f => f.uOwnerUserId == 1 && !f.boolIsDeleted).ToListAsync();
        return Ok(folders);
    }

    [HttpGet("versions/{documentId}")]
    public async Task<IActionResult> GetDocumentVersionAsync(string documentId)
    {
        List<DocumentVersion> documentVersions = await _auradocsContext.DocumentVersion.Where(d => d.strGuid == documentId).ToListAsync();
        return Ok(documentVersions);
    }

    [HttpPost("document")]
    public async Task<IActionResult> CreateDocumentAsync(CreateDocumentDto createDocument)
    {
        User user = await _userInformationService.GetUserInformation();
        if (user == null)
        {
            return Unauthorized();
        }
        using var transaction = await _auradocsContext.Database.BeginTransactionAsync();
        try{
            Document? document = await GetDocumentUsingTitle(createDocument.Title);
            while(document != null)
            {
                string tempTitle = document.strTitle + $" {AppConstants.existingTitlePostfix}";
                document = await GetDocumentUsingTitle(tempTitle);
            };
            Document newDocument = new Document
            {
                strGuid = Guid.NewGuid().ToString(),
                strTitle = createDocument.Title,
                strContent = createDocument.Content,
                uStatusId = (int)createDocument.Status,
                uCurrentVersionId = 1,
                uCreatedBy = user.uUid,
                uOwnerUserId = user.uUid,
                boolIsDeleted = false,
                dtUpdatedOn = DateTime.UtcNow,
                dtCreatedOn = DateTime.UtcNow
            };
            _auradocsContext.Documents.Add(newDocument);
            await _auradocsContext.SaveChangesAsync();

            Document currentDocument = await GetDocumentUsingId(newDocument.strGuid);

            DocumentVersion newdocumentVersion = new DocumentVersion
            {
                strGuid = Guid.NewGuid().ToString(),
                uVersion = 1,
                uDocumentId = currentDocument.uId,
                strContent = currentDocument.strContent,
                uUpdatedBy = user.uUid,
                dtUpdatedOn = DateTime.UtcNow
            };
            _auradocsContext.DocumentVersion.Add(newdocumentVersion);
            await _auradocsContext.SaveChangesAsync();

            await AssignDocumentToFolder(createDocument.FolderId, newDocument.strGuid);
            await transaction.CommitAsync();
            return Ok(newDocument.strGuid);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    [HttpPut("document")]
    public async Task<IActionResult> UpdateDocumentAsync(UpdateDocumentDto updateDocumentDto)
    {
        Document? document = await GetDocumentUsingId(updateDocumentDto.Id);
        if (document == null)
        {
            return BadRequest("Document doesn't exist");
        }

        document.strTitle = updateDocumentDto.Title;
        document.strContent = updateDocumentDto.Content;
        await _auradocsContext.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("document/{documentId}")]
    public async Task<IActionResult> DeleteDocumentAsync(string documentId)
    {
        Document? document = await GetDocumentUsingId(documentId);
        if (document == null)
        {
            return BadRequest();            
        }
        document.boolIsDeleted = true;
        await _auradocsContext.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("folder")]
    public async Task<IActionResult> CreateFolderAsync(CreateFolderDto createFolderDto)
    {
        Folder? folder = await GetFolderUsingTitle(createFolderDto.Title);
        if (folder != null)
        {
            createFolderDto.Title = $"{folder.strTitle} {AppConstants.existingTitlePostfix}";
        }
        return Ok();
    }

    [HttpPost("duplicate-document/{documentId}")]
    public async Task<IActionResult> DuplicateDocumentAsync(string documentId)
    {
        User user = await _userInformationService.GetUserInformation();
        if (user == null)
        {
            return Unauthorized();
        }
        //get esiting document
        Document? document  =  await _auradocsContext.Documents
                                    .Where(e => e.strGuid == documentId)
                                    .FirstOrDefaultAsync();
        if (document == null)
        {
            return BadRequest();
        }

        //create copy of the document
        Document newDocument = new Document()
        {
            strGuid = Guid.NewGuid().ToString(),
            strTitle = $"{document.strTitle} {AppConstants.duplicateDocumentTitlePostFix}",
            strContent = document.strContent,
            uStatusId = (int)DocumentStatus.DRAFT,
            uCurrentVersionId = 1,
            uCreatedBy = user.uUid,
            uOwnerUserId = user.uUid,
            boolIsDeleted = false,
            dtUpdatedOn = DateTime.UtcNow,
            dtCreatedOn = DateTime.UtcNow
        };
        _auradocsContext.Documents.Add(newDocument);
        await _auradocsContext.SaveChangesAsync();

        // create the version of document
        Document? currentDocument = await GetDocumentUsingId(newDocument.strGuid);
        if (currentDocument == null)
        {
            return BadRequest();
        }

        DocumentVersion newdocumentVersion = new DocumentVersion
        {
            strGuid = Guid.NewGuid().ToString(),
            uVersion = 1,
            uDocumentId = currentDocument.uId,
            strContent = currentDocument.strContent,
            uUpdatedBy = user.uUid,
            dtUpdatedOn = DateTime.UtcNow
        };
        _auradocsContext.DocumentVersion.Add(newdocumentVersion);
        await _auradocsContext.SaveChangesAsync();
    
        return Ok();
    }

    [HttpPost("share-document")]
    public async Task<IActionResult> ShareDocumentAsync(ShareDocumentDto shareDocument)
    {
        User user = await _userInformationService.GetUserInformation();
        if (user == null)
        {
            return Unauthorized();
        }

        DocumentSharedWithUser? documentShared = await _auradocsContext.DocumentsSharedWithUsers
                                                    .Where(e => e.uSharedDocumentId == shareDocument.documentId && e.uSharedBy == user.uUid && e.uSharedWith == shareDocument.sharedWith)
                                                    .FirstOrDefaultAsync();
        if (documentShared is null)
        {
            documentShared = new DocumentSharedWithUser
            {
                uSharedDocumentId = shareDocument.documentId,
                uSharedWith = shareDocument.sharedWith,
                uSharedBy = user.uUid,
                uAccessgiven = shareDocument.AccessType,
                dtAccessGivenOn = DateTime.UtcNow
            };
            _auradocsContext.DocumentsSharedWithUsers.Add(documentShared);
        }
        else
        {
            documentShared.uAccessgiven = shareDocument.AccessType;
            documentShared.dtAccessGivenOn = DateTime.UtcNow;
        }
        await _auradocsContext.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("download-document/{documentId}")]
    public async Task<IActionResult> DownloadDocumentAsync(string documentId)
    {
        Document? document = await _auradocsContext.Documents
                                    .Where(e => e.strGuid == documentId)
                                    .FirstOrDefaultAsync();
        if (document == null)
        {
            return BadRequest("Document not exist!!");
        }
        byte[] pdf = _convertFileToPdf.ConvertFile(document.strContent);
        return Ok(File(pdf,"application/octet-stream", document.strTitle));
    }

    [HttpPost("rewrite-text")]
    public async Task<IActionResult> RewriteTextAsync(DocumentEditorTextRequest documentEditorTextRequest)
    {
        User user = await _userInformationService.GetUserInformation();
        if (user == null)
        {
            return Unauthorized();
        }

        string llmResponse = await _aiService.RewriteAsync(documentEditorTextRequest.SelectedText, documentEditorTextRequest.DocumentContent);
        return Ok(llmResponse);
    }

    [HttpPost("summerize-text")]
    public async Task<IActionResult> SummerizeTextAsync(DocumentEditorTextRequest documentEditorTextRequest)
    {
        User user = await _userInformationService.GetUserInformation();
        if (user == null)
        {
            return Unauthorized();
        }

        string llmResponse = await _aiService.SummerizeAsync(documentEditorTextRequest.SelectedText, documentEditorTextRequest.DocumentContent);
        return Ok(llmResponse);
    }

    [HttpPost("translate-text")]
    public async Task<IActionResult> TranslateTextAsync(DocumentEditorTextRequest documentEditorTextRequest)
    {
        User user = await _userInformationService.GetUserInformation();
        if (user == null)
        {
            return Unauthorized();
        }

        string llmResponse = await _aiService.TranslateAsync(documentEditorTextRequest.SelectedText, documentEditorTextRequest.Language);
        return Ok(llmResponse);
    }

    private async Task<Folder?> GetFolderUsingId(string id)
    {
        return await _auradocsContext.Folders.Where(f => f.strGuid == id).FirstOrDefaultAsync();
    }

    private async Task<Folder?> GetFolderUsingTitle(string title)
    {
        return await _auradocsContext.Folders.Where(f => f.strTitle == title).FirstOrDefaultAsync();
    }

    private async Task<Document?> GetDocumentUsingId(string id)
    {
        return await _auradocsContext.Documents.Where(e => e.strGuid == id).FirstOrDefaultAsync();
    }

    private async Task<Document?> GetDocumentUsingTitle(string title)
    {
        return await _auradocsContext.Documents.Where(e => e.strTitle == title).FirstOrDefaultAsync();
    }

    private async Task AssignDocumentToFolder(string folderId, string documentId)
    {
        Folder? folder = await GetFolderUsingId(folderId);
        Document? currentDocument = await GetDocumentUsingId(documentId);
        if (folder != null)
        {
            DocumentFolder folderHasDocument = new DocumentFolder
            {
                strGuid = Guid.NewGuid().ToString(),
                uDocumentId = currentDocument.uId,
                uFolderId = folder.uId,
                dtCreatedAt = DateTime.UtcNow
            };
            _auradocsContext.DocumentFolders.Add(folderHasDocument);
            await _auradocsContext.SaveChangesAsync();
        }
    }
}