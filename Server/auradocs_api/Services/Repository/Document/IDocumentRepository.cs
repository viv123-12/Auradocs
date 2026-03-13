public interface IDocumentRepository
{
    public Task<List<Document>> GetActiveDocumentsAsync(int userId);
    public Task<List<Document>> GetAllActiveDocumentsOfFolderAsync(List<int> documentIdList, int userId);
    public Task<Document> GetDocumentUsingIdAsync(string documentId);
    public Task<Document> GetDocumentUsingTitleAsync(string title);
    public Task AddDocumentAsync(Document document);
}