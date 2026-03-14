using auradocs_api.Contexts;
using Microsoft.AspNetCore.Mvc;
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

    public async Task<List<FolderResponse>> ListActiveChildFoldersAsync(List<int> folderIdList, int userId)
    {
        List<FolderResponse> folderResponses =  await _auradocsContext.Folders
                                        .Where(e => folderIdList.Contains(e.uId) && e.uOwnerUserId == userId && !e.boolIsDeleted)
                                        .Select(e => new FolderResponse
                                        {
                                            folderId = e.strGuid,
                                            folderTitle = e.strTitle
                                        })
                                        .ToListAsync();
        return folderResponses;
    }

    public async Task<List<Folder>> GetActivatedFoldersAsync(int userId)
    {
        return await _auradocsContext.Folders.Where(f => f.uOwnerUserId == userId && !f.boolIsDeleted).ToListAsync();
    }

    public async Task<List<FolderResponse>> ListOrphanFoldersAsync(int userId)
    {
        List<FolderResponse> folderResponses =  await _auradocsContext.Folders
                                        .Where(e => e.uParentFolderId == 0 && e.uOwnerUserId == userId && !e.boolIsDeleted)
                                        .Select(e => new FolderResponse
                                        {
                                            folderId = e.strGuid,
                                            folderTitle = e.strTitle
                                        })
                                        .ToListAsync();
        return folderResponses;
    }

    public async Task<List<int>> GetChildrenFoldersIdAsync(int parentFolderId)
    {
        return await _auradocsContext.Folders
                                            .Where(e => e.uParentFolderId == parentFolderId && !e.boolIsDeleted)
                                            .Select(e => e.uId)
                                            .ToListAsync();
    }

    public async Task<bool> AddFolderAsync(Folder folder)
    {
        _auradocsContext.Folders.Add(folder);
        await _auradocsContext.SaveChangesAsync();
        return true;
    }
}