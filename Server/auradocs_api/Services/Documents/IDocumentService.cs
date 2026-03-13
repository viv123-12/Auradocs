public interface IDocumentService
{
    public Task<List<Document>> GetDocumentListAsync(int userId, string? folderGuid);
    public Task<List<Folder>> GetFolderListAsync(int userId, string? folderId);
    public Task<List<DocumentVersion>> GetDocumentVersionsAsync(string documentId);
    public Task<Document> GetActiveDocuementAsync(string documentId);
    public Task<bool> CreateDocumentAsync(int userId, CreateDocumentDto createDocument);
    public Task<bool> UpdateDocumentAsync(UpdateDocumentDto updateDocumentDto);
    public Task<bool> DeleteDocumentAsync(string documentId);
    public Task CreateFolderAsync();
    public Task<bool> DuplicateDocumentAsync(int userId, string documentId);
    public Task<bool> ShareDocumentAsync(int userId, ShareDocumentDto shareDocumentDto);
    public Task<(byte[], string documentTitle)> DownloadDocumentAsync(string documenId);
    public Task AssignDocumentToFolderAsync(string folderId, string documentId);
}