public interface IDocumentSharedWithUsersRepository
{
    public Task<DocumentSharedWithUser> GetDocumentSharedWithUserAsync(int documentId, int sharedBy, int sharedWith);
    
}