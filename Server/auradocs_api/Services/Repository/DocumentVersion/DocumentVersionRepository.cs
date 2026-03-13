using auradocs_api.Contexts;
using Microsoft.EntityFrameworkCore;

public class DocumentVersionRepository:IDocumentVersionRepository
{
    AuradocsContext _auradocsContext;
    public DocumentVersionRepository(AuradocsContext auradocsContext)
    {
        _auradocsContext = auradocsContext;
    }
    public async Task<List<DocumentVersion>> GetDocumentVersionAsync(string documenId)
    {
        List<DocumentVersion> documentVersions = await _auradocsContext.DocumentVersion.Where(d => d.strGuid == documenId).ToListAsync();
        return documentVersions;
    }

    public async Task AddDocumentVersionAsync(DocumentVersion documentVersion)
    {
        _auradocsContext.DocumentVersion.Add(documentVersion);
        await _auradocsContext.SaveChangesAsync();
    }
}