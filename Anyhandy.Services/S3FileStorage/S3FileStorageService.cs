using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Anyhandy.Interface;

namespace Anyhandy.Services.Users
{
    public class S3FileStorageService : IFileStorage
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName = "anyhandybucket";

        public S3FileStorageService(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
        }

        public async Task UploadFileAsync(string key, byte[] content, string contentType)
        {
            var putObjectRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = key,
                ContentType = contentType,
                InputStream = new MemoryStream(content),
            };

            try
            {
                await _s3Client.PutObjectAsync(putObjectRequest);
            }
            catch (AmazonServiceException e)
            {
                // Handle exception (e.g., log error, throw specific exception)
                Console.WriteLine($"Error uploading file: {e.Message}");
            }


        }

    }
}
