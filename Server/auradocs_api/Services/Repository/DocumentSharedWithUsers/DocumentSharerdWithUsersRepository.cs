using auradocs_api.Contexts;
using Microsoft.EntityFrameworkCore;

public class DocumentSharerdWithUsersRepository: IDocumentSharedWithUsersRepository
{
    AuradocsContext _auradocsContext;
    public DocumentSharerdWithUsersRepository(AuradocsContext auradocsContext)
    {
        _auradocsContext = auradocsContext;
    }
    public async Task<DocumentSharedWithUser> GetDocumentSharedWithUserAsync(int documentId, int sharedBy, int sharedWith)
    {
        DocumentSharedWithUser? documentShared = await _auradocsContext.DocumentsSharedWithUsers
                                                    .Where(e => e.uSharedDocumentId == documentId && e.uSharedBy == sharedBy && e.uSharedWith == sharedWith)
                                                    .FirstOrDefaultAsync();
        return documentShared;
    }
}