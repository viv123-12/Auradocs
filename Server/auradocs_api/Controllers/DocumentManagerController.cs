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
    private readonly IAIService _aiService;
    private readonly IDocumentService _documentService;
    public DocumentManagerController(AuradocsContext auradocsContext, UserInformationService userInformationService, IAIService aIService, IDocumentService documentService)
    {
        _auradocsContext = auradocsContext;
        _userInformationService = userInformationService;
        _aiService = aIService;
        _documentService = documentService;
    }

    [HttpGet("documents")]
    public async Task<IActionResult> GetDocumentListAsync(string? folderGuid)
    {
        User user = await _userInformationService.GetUserInformation();
        if(user == null)
        {
            return Unauthorized();
        }
        List<Document> documents  = await _documentService.GetDocumentListAsync(user.uUid, folderGuid);
        return Ok(documents);
    }

    [HttpGet("folders")]
    public async Task<IActionResult> GetFolderListAsync(string? folderGuid)
    {
        User user = await _userInformationService.GetUserInformation();
        if(user == null)
        {
            return Unauthorized();
        }
        List<Folder> folders = await _documentService.GetFolderListAsync(user.uUid, folderGuid);
        return Ok(folders);
    }

    [HttpGet("document/{documentId}")]
    public async Task<IActionResult> GetDocumentAsync(string documentId)
    {
        Document? document = await _documentService.GetActiveDocuementAsync(documentId);
        if (document == null)
        {
            return BadRequest("Document doesn't exist");            
        }
        return Ok(document);
    }

    [HttpGet("versions/{documentId}")]
    public async Task<IActionResult> GetDocumentVersionAsync(string documentId)
    {
        List<DocumentVersion> documentVersions = await _documentService.GetDocumentVersionsAsync(documentId);
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

        if(!await _documentService.CreateDocumentAsync(user.uUid, createDocument))
        {
            return BadRequest();
        }
        return Ok();
    }

    [HttpPut("document")]
    public async Task<IActionResult> UpdateDocumentAsync(UpdateDocumentDto updateDocumentDto)
    {
        User user = await _userInformationService.GetUserInformation();
        if (user == null)
        {
            return Unauthorized();
        }

        if(!await _documentService.UpdateDocumentAsync(updateDocumentDto))
        {
            return BadRequest();
        }
        return Ok();
    }

    [HttpDelete("document/{documentId}")]
    public async Task<IActionResult> DeleteDocumentAsync(string documentId)
    {
        User user = await _userInformationService.GetUserInformation();
        if (user == null)
        {
            return Unauthorized();
        }
        if (!await _documentService.DeleteDocumentAsync(documentId))
        {
            return BadRequest();
        }
        return Ok();
    }

    [HttpPost("folder")]
    public async Task<IActionResult> CreateFolderAsync(CreateFolderDto createFolderDto)
    {
        // Folder? folder = await GetFolderUsingTitle(createFolderDto.Title);
        // if (folder != null)
        // {
        //     createFolderDto.Title = $"{folder.strTitle} {AppConstants.existingTitlePostfix}";
        // }
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

        if (await _documentService.DuplicateDocumentAsync(user.uUid, documentId))
        {
            return BadRequest();
        }
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

        if(!await _documentService.ShareDocumentAsync(user.uUid, shareDocument))
        {
            return BadRequest();
        }
        return Ok();
    }

    [HttpGet("download-document/{documentId}")]
    public async Task<IActionResult> DownloadDocumentAsync(string documentId)
    {
        (byte[] pdf, string documentTitle) = await _documentService.DownloadDocumentAsync(documentId);
        if (pdf == null || documentTitle == null)
        {
            return BadRequest();
        }
        return Ok(File(pdf,"application/octet-stream", documentTitle));
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
}