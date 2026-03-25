public interface IStorageService
{
    public Task<string> UploadFileAsync(Stream fileStream, string fileName);
    public Task<Stream> GetFileAsync(string key);
    public Task DeleteFileAsync(string fileName);
}