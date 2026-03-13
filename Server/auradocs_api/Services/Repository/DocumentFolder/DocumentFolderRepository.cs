
using auradocs_api.Contexts;
using Microsoft.EntityFrameworkCore;

public class DocumentFolderRepository : IDocumentFolderRepository
{
    AuradocsContext _auradocsContext;
    public DocumentFolderRepository(AuradocsContext auradocsContext)
    {
        _auradocsContext = auradocsContext;
    }
    public async Task<List<int>> GetDocumentsOfFoldersIdlistAsync(int folderId)
    {
        return await _auradocsContext.DocumentFolders.Where(e => e.uFolderId == folderId)
                                                        .Select(e => e.uDocumentId)
                                                        .ToListAsync();
    }

    public async Task AddDocumentInFolderAsync(DocumentFolder folderHasDocument)
    {
        _auradocsContext.DocumentFolders.Add(folderHasDocument);
        await _auradocsContext.SaveChangesAsync();
    }
}