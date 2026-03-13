public interface IFolderRepository
{
    public Task<Folder> GetFolderUsingId(string id);
    public Task<Folder> GetFolderUsingTitle(string title);
    public Task<List<Folder>> GetActivatedFoldersAsync(int userId);
    public Task<List<Folder>> GetActiveChildFoldersAsync(List<int> folderIdList, int userId);
    public Task<List<int>> GetChildrenFoldersIdAsync(int parentFolderId);
}