using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using Microsoft.Extensions.Options;

public class MinIoStorageService: IStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;

    public MinIoStorageService(IAmazonS3 s3Client, IOptions<MinIoStorageServiceSettings> minIoSettings)
    {
        _s3Client = s3Client;
        _bucketName = minIoSettings.Value.BucketName;
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
    {
        if(!await IsBucketExist())
        {
            CreateBucketAsync();
        }
        PutObjectRequest request = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = fileName,
            InputStream = fileStream
        };

        await _s3Client.PutObjectAsync(request);

        return $"http://localhost:9001/{_bucketName}/{fileName}";
    }

    public async Task DeleteFileAsync(string fileName)
    {
        if(!await IsBucketExist())
        {
            CreateBucketAsync();
        }
        await _s3Client.DeleteObjectAsync(_bucketName, fileName);
    }

    public async Task<Stream> GetFileAsync(string key)
    {
        if(!await IsBucketExist())
        {
            CreateBucketAsync();
        }
        GetObjectResponse response = await _s3Client.GetObjectAsync(_bucketName, key);
        return response.ResponseStream;
    }

    private async Task<bool> IsBucketExist()
    {
        bool exists = await AmazonS3Util.DoesS3BucketExistV2Async(_s3Client,_bucketName);
        return exists;
    }

    private async Task CreateBucketAsync()
    {
        PutBucketRequest request = new PutBucketRequest
        {
            BucketName = _bucketName,
            UseClientRegion = false  
        };
        await _s3Client.PutBucketAsync(request);
    }
}