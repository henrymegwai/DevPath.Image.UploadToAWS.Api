using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BlinkCash.Core.Dtos;
using BlinkCash.Core.Models.Dtos;
using Microsoft.AspNetCore.Http;

namespace BlinkCash.Core.Interfaces.Services
{
    public interface IAWSS3Service
    {
        Task<bool> DoesS3BucketExistAsync(string bucketName = null);
        Task<ExecutionResponse<CreateS3BucketResponseDto>> CreateBucketAsync(string bucketName = null);
        Task<ExecutionResponse<string>> UploadFileAsync(IFormFile file);
        Task<ExecutionResponse<string>> UploadFileAsync(Stream fileStream, string fileName);
    }
}