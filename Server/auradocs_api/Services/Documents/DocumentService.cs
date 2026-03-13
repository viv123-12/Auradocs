
using auradocs_api.Contexts;
using auradocs_api.Models;
using Microsoft.EntityFrameworkCore;

public class DocumentService : IDocumentService
{
    IDocumentRepository _documentRepository;
    IFolderRepository _folderRepository;
    IDocumentFolderRepository _documentFolderRepository;
    IDocumentVersionRepository _documentVersionRepository;
    IDocumentSharedWithUsersRepository _documentSharedWithUserRepository;
    AuradocsContext _auradocsContext;
    private readonly IConvertFileService _convertFileToPdf;
    public DocumentService(IDocumentRepository documentRepository,IConvertFileService convertFileToPdf, IFolderRepository folderRepository, IDocumentFolderRepository documentFolderRepository, IDocumentVersionRepository documentVersionRepository, AuradocsContext auradocsContext, IDocumentSharedWithUsersRepository documentSharedWithUserRepository)
    {
        _auradocsContext = auradocsContext;
        _documentRepository = documentRepository;
        _folderRepository = folderRepository;
        _documentFolderRepository = documentFolderRepository;
        _documentVersionRepository = documentVersionRepository;
        _documentSharedWithUserRepository = documentSharedWithUserRepository;
        _convertFileToPdf = convertFileToPdf;
    }
    public async Task<bool> CreateDocumentAsync(int userId, CreateDocumentDto createDocument)
    {
        using var transaction = await _auradocsContext.Database.BeginTransactionAsync();
        try{
            Document? document = await _documentRepository.GetDocumentUsingTitleAsync(createDocument.Title);
            while(document != null)
            {
                string tempTitle = document.strTitle + $" {AppConstants.existingTitlePostfix}";
                document = await _documentRepository.GetDocumentUsingTitleAsync(tempTitle);
            };
            Document newDocument = new Document
            {
                strGuid = Guid.NewGuid().ToString(),
                strTitle = createDocument.Title,
                strContent = createDocument.Content,
                uStatusId = (int)createDocument.Status,
                uCurrentVersionId = 1,
                uCreatedBy = userId,
                uOwnerUserId = userId,
                boolIsDeleted = false,
                dtUpdatedOn = DateTime.UtcNow,
                dtCreatedOn = DateTime.UtcNow
            };
            await _documentRepository.AddDocumentAsync(newDocument);

            Document currentDocument = await _documentRepository.GetDocumentUsingIdAsync(newDocument.strGuid);

            DocumentVersion newdocumentVersion = new DocumentVersion
            {
                strGuid = Guid.NewGuid().ToString(),
                uVersion = 1,
                uDocumentId = currentDocument.uId,
                strContent = currentDocument.strContent,
                uUpdatedBy = userId,
                dtUpdatedOn = DateTime.UtcNow
            };
            await _documentVersionRepository.AddDocumentVersionAsync(newdocumentVersion);

            await AssignDocumentToFolderAsync(createDocument.FolderId, newDocument.strGuid);
            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            return false;
        }
    }

    public Task CreateFolderAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteDocumentAsync(string documenId)
    {
        Document? document = await _documentRepository.GetDocumentUsingIdAsync(documenId);
        if (document == null)
        {
            return false;            
        }
        document.boolIsDeleted = true;
        await _auradocsContext.SaveChangesAsync();
        return true;
    }

    public async Task<(byte[],string documentTitle)> DownloadDocumentAsync(string documenId)
    {
        Document? document = await _documentRepository.GetDocumentUsingIdAsync(documenId);
        if (document == null)
        {
            return (null,null);
        }
        byte[] pdf = _convertFileToPdf.ConvertFile(document.strContent);
        return (pdf, document.strTitle);
    }

    public async Task<bool> DuplicateDocumentAsync(int userId, string documentId)
    {
        Document? document  =  await _auradocsContext.Documents
                                    .Where(e => e.strGuid == documentId)
                                    .FirstOrDefaultAsync();
        if (document == null)
        {
            return false;
        }

        //create copy of the document
        Document newDocument = new Document()
        {
            strGuid = Guid.NewGuid().ToString(),
            strTitle = $"{document.strTitle} {AppConstants.duplicateDocumentTitlePostFix}",
            strContent = document.strContent,
            uStatusId = (int)DocumentStatus.DRAFT,
            uCurrentVersionId = 1,
            uCreatedBy = userId,
            uOwnerUserId = userId,
            boolIsDeleted = false,
            dtUpdatedOn = DateTime.UtcNow,
            dtCreatedOn = DateTime.UtcNow
        };
        await _documentRepository.AddDocumentAsync(newDocument);

        // create the version of document
        Document? currentDocument = await _documentRepository.GetDocumentUsingIdAsync(newDocument.strGuid);
        if (currentDocument == null)
        {
            return false;
        }

        DocumentVersion newdocumentVersion = new DocumentVersion
        {
            strGuid = Guid.NewGuid().ToString(),
            uVersion = 1,
            uDocumentId = currentDocument.uId,
            strContent = currentDocument.strContent,
            uUpdatedBy = userId,
            dtUpdatedOn = DateTime.UtcNow
        };
        await _documentVersionRepository.AddDocumentVersionAsync(newdocumentVersion);
        return true;
    }

    public async Task<List<Document>> GetDocumentListAsync(int userId, string? folderGuid)
    {
        if(!string.IsNullOrEmpty(folderGuid))
        {
            Folder folder = await _folderRepository.GetFolderUsingId(folderGuid);
            if (folder == null)
            {
                return null;
            }
            List<int> currentFolderDocumentList = await _documentFolderRepository.GetDocumentsOfFoldersIdlistAsync(folder.uId);
            return await _documentRepository.GetAllActiveDocumentsOfFolderAsync(currentFolderDocumentList, userId);
        }
        List<Document> documents = await _documentRepository.GetActiveDocumentsAsync(userId);
        return documents;
    }

    public async Task<List<Folder>> GetFolderListAsync(int userId, string? folderId)
    {
        if(!string.IsNullOrEmpty(folderId))
        {
            Folder folder = await _folderRepository.GetFolderUsingId(folderId);
            if (folder == null)
            {
                return null;
            }
            List<int> currentFolderChildrendFolderList = await _folderRepository.GetChildrenFoldersIdAsync(folder.uId);
            return await _folderRepository.GetActiveChildFoldersAsync(currentFolderChildrendFolderList, userId);
        }
        List<Folder> folders = await _folderRepository.GetActivatedFoldersAsync(userId);
        return folders;
    }

    public async Task<Document> GetActiveDocuementAsync(string folderId)
    {
        return await _documentRepository.GetDocumentUsingIdAsync(folderId);
    }

    public async Task<List<DocumentVersion>> GetDocumentVersionsAsync(string documentId)
    {
        return await _documentVersionRepository.GetDocumentVersionAsync(documentId);
    }

    public async Task<bool> ShareDocumentAsync(int userId, ShareDocumentDto shareDocumentDto)
    {
        DocumentSharedWithUser? documentShared = await _documentSharedWithUserRepository.GetDocumentSharedWithUserAsync(shareDocumentDto.documentId, userId,shareDocumentDto.sharedWith);
        if (documentShared is null)
        {
            documentShared = new DocumentSharedWithUser
            {
                uSharedDocumentId = shareDocumentDto.documentId,
                uSharedWith = shareDocumentDto.sharedWith,
                uSharedBy = userId,
                uAccessgiven = shareDocumentDto.AccessType,
                dtAccessGivenOn = DateTime.UtcNow
            };
        }
        else
        {
            documentShared.uAccessgiven = shareDocumentDto.AccessType;
            documentShared.dtAccessGivenOn = DateTime.UtcNow;
        }
        await _auradocsContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateDocumentAsync(UpdateDocumentDto updateDocumentDto)
    {
        Document? document = await _documentRepository.GetDocumentUsingIdAsync(updateDocumentDto.DocumentId);
        if (document == null)
        {
            return false;
        }

        document.strTitle = updateDocumentDto.Title;
        document.strContent = updateDocumentDto.Content;
        await _auradocsContext.SaveChangesAsync();
        return true;
    }

    public async Task AssignDocumentToFolderAsync(string folderId, string documentId)
    {
        Folder? folder = await _folderRepository.GetFolderUsingId(folderId);
        Document? currentDocument = await _documentRepository.GetDocumentUsingIdAsync(documentId);
        if (folder != null)
        {
            DocumentFolder folderHasDocument = new DocumentFolder
            {
                strGuid = Guid.NewGuid().ToString(),
                uDocumentId = currentDocument.uId,
                uFolderId = folder.uId,
                dtCreatedAt = DateTime.UtcNow
            };
            await _documentFolderRepository.AddDocumentInFolderAsync(folderHasDocument);
        }
    }
}