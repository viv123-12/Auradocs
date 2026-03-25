using auradocs_api.Contexts;
using Microsoft.EntityFrameworkCore;

public class DocumentRepository: IDocumentRepository
{
    AuradocsContext _auradocsContext;
    public DocumentRepository(AuradocsContext auradocsContext)
    {
        _auradocsContext = auradocsContext;
    }
    public async Task<List<DocumentResponse>> ListActiveDocumentsAsync(int userId)
    {
        return await _auradocsContext.Documents
                                        .Where(e => e.uOwnerUserId == userId && !e.boolIsDeleted)
                                        .Select(e =>
                                        new DocumentResponse{
                                            documentId = e.strGuid,
                                            documentTitle = e.strTitle,
                                            documentContent = e.strContent,
                                            documentState = CommonHelper.GetDocumentState(e.uStatusId),
                                            currentDocumentVersion = e.uCurrentVersionId
                                        })
                                        .ToListAsync();
    }

    public async Task<List<DocumentResponse>> ListAllActiveDocumentsOfFolderAsync(List<int> documentIdList, int userId, GetDocumentsListDto getDocumentsListDto)
    {
        IQueryable<Document> documents =  _auradocsContext.Documents
                                                .Where(e => documentIdList.Contains(e.uId) 
                                                    && e.uOwnerUserId == userId 
                                                    && !e.boolIsDeleted);
        if(!string.IsNullOrWhiteSpace(getDocumentsListDto.searchValue))
        {
             documents = documents.Where(d => EF.Functions.Like(d.strTitle,$"%{getDocumentsListDto.searchValue}%"));
        }
        
        if (getDocumentsListDto?.DocumentType != null)
        {
            documents = documents.Where(d => d.uDocumentType == getDocumentsListDto.DocumentType);
        }

        if (getDocumentsListDto?.DocumentStatus != null)
        {
            documents = documents.Where(d => d.uDocumentType == getDocumentsListDto.DocumentType);
        }

        if (getDocumentsListDto?.DocumentStatus != null)
        {
            documents = documents.Where(d => d.uDocumentType == getDocumentsListDto.DocumentStatus);
        }

        if (getDocumentsListDto?.CreatedOn != null)
        {
            var createOn = getDocumentsListDto.CreatedOn.Value.Date;
            documents = documents.Where(d => d.dtCreatedOn.Date == createOn);
        }
        List<DocumentResponse> documentResponses = documents.Select(e => new DocumentResponse
        {
            documentId = e.strGuid,
            documentTitle = e.strTitle,
            documentContent = e.strContent,
            documentState = CommonHelper.GetDocumentState(e.uStatusId),
            currentDocumentVersion = e.uCurrentVersionId
        }).ToList();
        return documentResponses;
    }

    public async Task<Document> GetDocumentUsingIdAsync(string documentId)
    {
        return await _auradocsContext.Documents.Where(e => e.strGuid == documentId && !e.boolIsDeleted).FirstOrDefaultAsync();
    }

    public async Task<Document> GetDocumentUsingTitleAsync(string title)
    {
        return await _auradocsContext.Documents.Where(e => e.strGuid == title && !e.boolIsDeleted).FirstOrDefaultAsync();
    }

    public async Task AddDocumentAsync(Document document)
    {
        _auradocsContext.Documents.Add(document);
        await _auradocsContext.SaveChangesAsync();
    }

    public async Task<List<DocumentResponse>> ListOrphenDocumentsAsync(int userId, GetDocumentsListDto getDocumentsListDto)
    {
        IQueryable<Document> OrphanDocuments =  from d in _auradocsContext.Documents
                                                join df in _auradocsContext.DocumentFolders
                                                on d.uId equals df.uDocumentId into folderGroup
                                                from fg in folderGroup.DefaultIfEmpty()
                                                where fg == null && !d.boolIsDeleted
                                                select d;
        if(!string.IsNullOrWhiteSpace(getDocumentsListDto.searchValue))
        {
        OrphanDocuments = OrphanDocuments.Where(d => EF.Functions.Like(d.strTitle,$"%{getDocumentsListDto.searchValue}%"));    
        }

        if (getDocumentsListDto.DocumentType != null)
        {
            OrphanDocuments = OrphanDocuments.Where(d => d.uDocumentType == getDocumentsListDto.DocumentType);
        }

        if (getDocumentsListDto.DocumentStatus != null)
        {
            OrphanDocuments = OrphanDocuments.Where(d => d.uDocumentType == getDocumentsListDto.DocumentType);
        }

        if (getDocumentsListDto.DocumentStatus != null)
        {
            OrphanDocuments = OrphanDocuments.Where(d => d.uDocumentType == getDocumentsListDto.DocumentStatus);
        }

        if (getDocumentsListDto.CreatedOn != null)
        {
            OrphanDocuments = OrphanDocuments.Where(d => d.dtCreatedOn.Date == getDocumentsListDto.CreatedOn);
        }
        List<DocumentResponse> listOrphanDocument = OrphanDocuments.Select(e => new DocumentResponse
        {
            documentId = e.strGuid,
            documentTitle = e.strTitle,
            documentContent = e.strContent,
            documentState = CommonHelper.GetDocumentState(e.uStatusId),
            currentDocumentVersion = e.uCurrentVersionId
        }).ToList();
        return listOrphanDocument;                                          
    }
}