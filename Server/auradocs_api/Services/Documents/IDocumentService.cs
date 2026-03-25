public interface IDocumentService
{
    public Task<List<DocumentResponse>> GetDocumentListAsync(int userId, string? folderGuid, GetDocumentsListDto getDocumentsListDto);
    public Task<List<FolderResponse>> GetFolderListAsync(int userId, string? folderId);
    public Task<List<DocumentVersion>> GetDocumentVersionsAsync(string documentId);
    public Task<DocumentResponse> GetActiveDocuementAsync(string documentId);
    public Task<FolderResponse> GetActiveFolderAsync(string folderId);
    public Task<string> CreateDocumentAsync(int userId, CreateDocumentDto createDocument);
    public Task<bool> UpdateDocumentAsync(UpdateDocumentDto updateDocumentDto);
    public Task<bool> DeleteDocumentAsync(string documentId, string folderId);
    public Task<bool> CreateFolderAsync(int userId, CreateFolderDto createFolder);
    public Task<bool> DuplicateDocumentAsync(int userId, string documentId, string folderId);
    public Task<bool> ShareDocumentAsync(int userId, ShareDocumentDto shareDocumentDto);
    public Task<(byte[], string documentTitle)> DownloadDocumentAsync(string documenId);
    public Task AssignDocumentToFolderAsync(string folderId, string documentId);
    public Task<bool> UploadDocumentToFolderAsync(int userId, UploadDocumentDto uploadDocumentDto);
}