using auradocs_api.Contexts;
using Microsoft.EntityFrameworkCore;

public class DocumentRepository: IDocumentRepository
{
    AuradocsContext _auradocsContext;
    public DocumentRepository(AuradocsContext auradocsContext)
    {
        _auradocsContext = auradocsContext;
    }
    public async Task<List<Document>> GetActiveDocumentsAsync(int userId)
    {
        return await _auradocsContext.Documents
                                        .Where(e => e.uOwnerUserId == userId && !e.boolIsDeleted)
                                        .ToListAsync();
    }

    public async Task<List<Document>> GetAllActiveDocumentsOfFolderAsync(List<int> documentIdList, int userId)
    {
        return await _auradocsContext.Documents
                                        .Where(e => documentIdList.Contains(e.uId) && e.uOwnerUserId == userId && !e.boolIsDeleted)
                                        .ToListAsync();
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

}