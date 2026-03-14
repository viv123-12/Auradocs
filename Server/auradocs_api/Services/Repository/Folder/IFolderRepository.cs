public interface IFolderRepository
{
    public Task<Folder> GetFolderUsingId(string id);
    public Task<Folder> GetFolderUsingTitle(string title);
    public Task<List<Folder>> GetActivatedFoldersAsync(int userId);
    public Task<List<FolderResponse>> ListOrphanFoldersAsync(int userId);
    public Task<List<FolderResponse>> ListActiveChildFoldersAsync(List<int> folderIdList, int userId);
    public Task<List<int>> GetChildrenFoldersIdAsync(int parentFolderId);
    public Task<bool> AddFolderAsync(Folder folder);
}