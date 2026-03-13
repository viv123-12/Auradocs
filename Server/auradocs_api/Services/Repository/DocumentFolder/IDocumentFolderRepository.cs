public interface IDocumentFolderRepository
{
    public Task<List<int>> GetDocumentsOfFoldersIdlistAsync(int folderId);
    public Task AddDocumentInFolderAsync(DocumentFolder documentFolder);
}