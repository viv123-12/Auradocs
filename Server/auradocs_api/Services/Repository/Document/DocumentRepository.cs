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

    public async Task<List<DocumentResponse>> ListAllActiveDocumentsOfFolderAsync(List<int> documentIdList, int userId)
    {
        List<Document> documents = await _auradocsContext.Documents
                                                .Where(e => documentIdList.Contains(e.uId) 
                                                    && e.uOwnerUserId == userId 
                                                    && !e.boolIsDeleted)
                                                .ToListAsync();

        DocumentResponse[] documentResponses = await Task.WhenAll(
            documents.Select(async e => new DocumentResponse
            {
                documentId = e.strGuid,
                documentTitle = e.strTitle,
                documentContent = e.strContent,
                documentState = CommonHelper.GetDocumentState(e.uStatusId),
                currentDocumentVersion = e.uCurrentVersionId
            })
        );
        return documentResponses.ToList();

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

    public async Task<List<DocumentResponse>> ListOrphenDocumentsAsync(int userId)
    {
        List<DocumentResponse> listOrphanFolder = await (from d in _auradocsContext.Documents
                                                        join df in _auradocsContext.DocumentFolders
                                                        on d.uId equals df.uDocumentId into folderGroup
                                                        from fg in folderGroup.DefaultIfEmpty()
                                                        where fg == null && !d.boolIsDeleted
                                                        select  new DocumentResponse{
                                                            documentId = d.strGuid,
                                                            documentTitle = d.strTitle,
                                                            documentContent = d.strContent,
                                                            documentState = CommonHelper.GetDocumentState(d.uStatusId),
                                                            currentDocumentVersion = d.uCurrentVersionId
                                                        }).ToListAsync();
        return listOrphanFolder;                                             
    }
}