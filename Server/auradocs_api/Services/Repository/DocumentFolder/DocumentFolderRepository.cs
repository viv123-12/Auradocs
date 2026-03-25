
using auradocs_api.Contexts;
using Microsoft.EntityFrameworkCore;

public class DocumentFolderRepository : IDocumentFolderRepository
{
    AuradocsContext _auradocsContext;
    IFolderRepository _folderRepository;
    public DocumentFolderRepository(AuradocsContext auradocsContext, IFolderRepository folderRepository)
    {
        _auradocsContext = auradocsContext;
        _folderRepository = folderRepository;
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

    public async Task DeleteDocumentFromFolderAsync(int documentId, int folderId)
    {
        DocumentFolder documentFolder = await _auradocsContext.DocumentFolders.Where(df => df.uDocumentId == documentId && df.uFolderId == folderId).FirstOrDefaultAsync();
        _auradocsContext.Remove(documentFolder);
        await _auradocsContext.SaveChangesAsync();
    }

    public async Task DeleteDocumentFromFolderAsync(int documentId)
    {
        List<DocumentFolder> documentsOfFolder = await _auradocsContext.DocumentFolders.Where(df => df.uDocumentId == documentId).ToListAsync();
        _auradocsContext.RemoveRange(documentsOfFolder);
        await _auradocsContext.SaveChangesAsync();
    }
}