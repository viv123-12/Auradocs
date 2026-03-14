public interface IDocumentRepository
{
    public Task<List<DocumentResponse>> ListActiveDocumentsAsync(int userId);
    public Task<List<DocumentResponse>> ListOrphenDocumentsAsync(int userId);
    public Task<List<DocumentResponse>> ListAllActiveDocumentsOfFolderAsync(List<int> documentIdList, int userId);
    public Task<Document> GetDocumentUsingIdAsync(string documentId);
    public Task<Document> GetDocumentUsingTitleAsync(string title);
    public Task AddDocumentAsync(Document document);
}