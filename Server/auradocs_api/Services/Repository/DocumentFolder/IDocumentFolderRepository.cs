public interface IDocumentFolderRepository
{
    public Task<List<int>> GetDocumentsOfFoldersIdlistAsync(int folderId);
    public Task AddDocumentInFolderAsync(DocumentFolder documentFolder);
    public Task DeleteDocumentFromFolderAsync(int documentId, int FolderId);
    public Task DeleteDocumentFromFolderAsync(int documentId);
}