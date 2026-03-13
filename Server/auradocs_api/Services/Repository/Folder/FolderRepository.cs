using auradocs_api.Contexts;
using Microsoft.EntityFrameworkCore;

public class FolderRepository: IFolderRepository
{
    AuradocsContext _auradocsContext;
    public FolderRepository(AuradocsContext auradocsContext)
    {
        _auradocsContext = auradocsContext;
    }
    public async Task<Folder> GetFolderUsingId(string id)
    {
        return await _auradocsContext.Folders.Where(f => f.strGuid == id).FirstOrDefaultAsync();
    }

    public async Task<Folder> GetFolderUsingTitle(string title)
    {
        return await _auradocsContext.Folders.Where(f => f.strTitle == title).FirstOrDefaultAsync();
    }

    public async Task<List<Folder>> GetActiveChildFoldersAsync(List<int> folderIdList, int userId)
    {
        return await _auradocsContext.Folders
                                        .Where(e => folderIdList.Contains(e.uId) && e.uOwnerUserId == userId && !e.boolIsDeleted)
                                        .ToListAsync();
    }

    public async Task<List<Folder>> GetActivatedFoldersAsync(int userId)
    {
        return await _auradocsContext.Folders.Where(f => f.uOwnerUserId == userId && !f.boolIsDeleted).ToListAsync();
    }

    public async Task<List<int>> GetChildrenFoldersIdAsync(int parentFolderId)
    {
        return await _auradocsContext.Folders
                                            .Where(e => e.uParentFolderId == parentFolderId && !e.boolIsDeleted)
                                            .Select(e => e.uId)
                                            .ToListAsync();
    }
}