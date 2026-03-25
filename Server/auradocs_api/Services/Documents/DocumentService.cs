
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
    private readonly IStorageService _minIoStorageService;
    IUSerRepository _userRepository;
    public DocumentService(IDocumentRepository documentRepository,IConvertFileService convertFileToPdf, IFolderRepository folderRepository, IDocumentFolderRepository documentFolderRepository, IDocumentVersionRepository documentVersionRepository, AuradocsContext auradocsContext, IDocumentSharedWithUsersRepository documentSharedWithUserRepository,
    IStorageService minIoStorageService, IUSerRepository userRepository)
    {
        _auradocsContext = auradocsContext;
        _documentRepository = documentRepository;
        _folderRepository = folderRepository;
        _documentFolderRepository = documentFolderRepository;
        _documentVersionRepository = documentVersionRepository;
        _documentSharedWithUserRepository = documentSharedWithUserRepository;
        _convertFileToPdf = convertFileToPdf;
        _minIoStorageService = minIoStorageService;
        _userRepository = userRepository;
    }
    public async Task<string> CreateDocumentAsync(int userId, CreateDocumentDto createDocument)
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
                uDocumentType = (int)DocumentTypes.EDITOR,
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
            if(!String.IsNullOrEmpty(createDocument.FolderId))
            {
                 await AssignDocumentToFolderAsync(createDocument.FolderId, newDocument.strGuid);  
            }
            await transaction.CommitAsync();
            return newDocument.strGuid;
        }
        catch
        {
            await transaction.RollbackAsync();
            return null;
        }
    }

    public async Task<bool> CreateFolderAsync(int userId, CreateFolderDto createFolder)
    {
        int parentFolderId = 0;
        if(!string.IsNullOrEmpty(createFolder.ParentFolderId))
        {
            parentFolderId = (await _folderRepository.GetActiveFolderUsingId(createFolder.ParentFolderId)).uId;
        }
        Folder newFolder = new Folder
        {
            strGuid = Guid.NewGuid().ToString(),
            strTitle = createFolder.Title,
            uParentFolderId = parentFolderId,
            uOwnerUserId = userId,
            uCreatedBy = userId,
            boolIsDeleted = false,
            dtCreatedOn = DateTime.UtcNow
        };
        if(!await _folderRepository.AddFolderAsync(newFolder))
        {
            return false;
        }
        return true;
    }

    public async Task<bool> DeleteDocumentAsync(string documenId, string? folderId = null)
    {
        Document deletedDocument = await _documentRepository.GetDocumentUsingIdAsync(documenId);
        if(!string.IsNullOrEmpty(folderId))
        {
            Folder parentFolder = await _folderRepository.GetActiveFolderUsingId(folderId);
            await _documentFolderRepository.DeleteDocumentFromFolderAsync(deletedDocument.uId, parentFolder.uId);
            return true;
        }
        using var transaction = await _auradocsContext.Database.BeginTransactionAsync();
        try
        {
            await _documentFolderRepository.DeleteDocumentFromFolderAsync(deletedDocument.uId);
            Document? document = await _documentRepository.GetDocumentUsingIdAsync(documenId);
            if (document == null)
            {
                return false;            
            }
            document.boolIsDeleted = true;
            await _auradocsContext.SaveChangesAsync();
            await transaction.CommitAsync();
            return true;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex);
            await transaction.RollbackAsync();
            return false;
        }
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

    public async Task<bool> DuplicateDocumentAsync(int userId, string documentId, string folderGuid)
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
            uStatusId = (int)DocumentStatus.Draft,
            uCurrentVersionId = 1,
            uDocumentType = document.uDocumentType,
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
        if(!string.IsNullOrWhiteSpace(folderGuid))
        {
            Folder parentFolder = await _folderRepository.GetActiveFolderUsingId(folderGuid);
            DocumentFolder folderHasDocument = new DocumentFolder
            {
                strGuid = Guid.NewGuid().ToString(),
                uDocumentId = currentDocument.uId,
                uFolderId = parentFolder.uId,
                dtCreatedAt = DateTime.UtcNow
            };
            await _documentFolderRepository.AddDocumentInFolderAsync(folderHasDocument);
        }
        return true;
    }

    public async Task<List<DocumentResponse>> GetDocumentListAsync(int userId, string? folderGuid, GetDocumentsListDto getDocumentsListDto)
    {
        if(!string.IsNullOrEmpty(folderGuid))
        {
            Folder folder = await _folderRepository.GetActiveFolderUsingId(folderGuid);
            if (folder == null)
            {
                return null;
            }
            List<int> currentFolderDocumentList = await _documentFolderRepository.GetDocumentsOfFoldersIdlistAsync(folder.uId);
            return await _documentRepository.ListAllActiveDocumentsOfFolderAsync(currentFolderDocumentList, userId, getDocumentsListDto);
        }
        List<DocumentResponse> documents = await _documentRepository.ListOrphenDocumentsAsync(userId, getDocumentsListDto);
        return documents;
    }

    public async Task<List<FolderResponse>> GetFolderListAsync(int userId, string? folderId)
    {
        if(!string.IsNullOrEmpty(folderId))
        {
            Folder folder = await _folderRepository.GetActiveFolderUsingId(folderId);
            if (folder == null)
            {
                return null;
            }
            List<int> currentFolderChildrendFolderList = await _folderRepository.GetChildrenFoldersIdAsync(folder.uId);
            return await _folderRepository.ListActiveChildFoldersAsync(currentFolderChildrendFolderList, userId);
        }
        List<FolderResponse> folders = await _folderRepository.ListOrphanFoldersAsync(userId);
        return folders;
    }

    public async Task<DocumentResponse> GetActiveDocuementAsync(string folderId)
    {
        Document document = await _documentRepository.GetDocumentUsingIdAsync(folderId);
        DocumentResponse documentResponse = new DocumentResponse
        {
          documentId  = document.strGuid,
          documentContent = document.strContent,
          documentTitle = document.strTitle,
          documentState = CommonHelper.GetDocumentState(document.uStatusId),  
          currentDocumentVersion = document.uCurrentVersionId
        };
        return documentResponse;
    }

    public async Task<List<DocumentVersion>> GetDocumentVersionsAsync(string documentId)
    {
        return await _documentVersionRepository.GetDocumentVersionAsync(documentId);
    }

    public async Task<bool> ShareDocumentAsync(int userId, ShareDocumentDto shareDocumentDto)
    {
        User sharedWithUserId = await _userRepository.GetUserWihIdAsync(shareDocumentDto.sharedWith);
        DocumentSharedWithUser? documentShared = await _documentSharedWithUserRepository.GetDocumentSharedWithUserAsync(shareDocumentDto.documentId, userId,sharedWithUserId.uUid);
        if (documentShared is null)
        {
            documentShared = new DocumentSharedWithUser
            {
                uSharedDocumentId = shareDocumentDto.documentId,
                uSharedWith = sharedWithUserId.uUid,
                uSharedBy = userId,
                uAccessgiven = shareDocumentDto.AccessType,
                dtAccessGivenOn = DateTime.UtcNow
            };
            await _documentSharedWithUserRepository.AddDocumentSharedWithUserAsync(documentShared);
        }
        else
        {
            documentShared.uAccessgiven = shareDocumentDto.AccessType;
            documentShared.dtAccessGivenOn = DateTime.UtcNow;
            await _auradocsContext.SaveChangesAsync();
        }
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
        Folder? folder = await _folderRepository.GetActiveFolderUsingId(folderId);
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

    public async Task<FolderResponse> GetActiveFolderAsync(string folderId)
    {
        Folder folder = await _folderRepository.GetActiveFolderUsingId(folderId);
        if(folder == null)
        {
            return null;
        }
        FolderResponse folderResponse = new FolderResponse
        {
          folderId = folder.strGuid,
          folderTitle = folder.strTitle,  
        };
        return folderResponse;
    }

    public async Task<bool> UploadDocumentToFolderAsync(int userId, UploadDocumentDto uploadDocumentDto)
    {
        string fileUrl;
        DateTime now = DateTime.UtcNow;
        using (var memoryStream = new MemoryStream())
        {
            await uploadDocumentDto.File.CopyToAsync(memoryStream);

            memoryStream.Position = 0; // 🔥 CRITICAL

            fileUrl = await _minIoStorageService.UploadFileAsync(memoryStream, uploadDocumentDto.Title);
        }
        using var transaction = await _auradocsContext.Database.BeginTransactionAsync();
        try{
            Document newDocument = new Document
            {
                strGuid = Guid.NewGuid().ToString(),
                strTitle = uploadDocumentDto.Title,
                uDocumentType = (int)Enum.Parse<DocumentTypes>(uploadDocumentDto.DocumentType),
                strContent = String.Empty,
                strFileUrl = fileUrl,
                strFileType = uploadDocumentDto.Filetype,
                uStatusId = (int)DocumentStatus.Published,
                uCurrentVersionId = 1,
                uCreatedBy = userId,
                uOwnerUserId = userId,
                boolIsDeleted = false,
                dtUpdatedOn = now,
                dtCreatedOn = now
            };
            await _documentRepository.AddDocumentAsync(newDocument);

            Document uploadedDocumentMetaData = await _documentRepository.GetDocumentUsingIdAsync(newDocument.strGuid);

            DocumentVersion newDocumentVersion = new DocumentVersion
            {
                strGuid = Guid.NewGuid().ToString(),
                uVersion = 1,
                uDocumentId = uploadedDocumentMetaData.uId,
                strContent = String.Empty,
                uUpdatedBy = userId,
                dtUpdatedOn = now
            };
            await _documentVersionRepository.AddDocumentVersionAsync(newDocumentVersion);

            Folder parentFolder = await _folderRepository.GetActiveFolderUsingId(uploadDocumentDto.FolderId);

            if(!String.IsNullOrEmpty(uploadDocumentDto.FolderId))
            {
                 await AssignDocumentToFolderAsync(uploadDocumentDto.FolderId, newDocument.strGuid);  
            }
            await transaction.CommitAsync();
            return true;
        }
        catch(Exception ex)
        {
            await transaction.RollbackAsync();
            return false;
        }
    }
}