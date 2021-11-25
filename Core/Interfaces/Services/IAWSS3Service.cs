using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Core.Dtos;
using Core.Models.Dtos;
using Microsoft.AspNetCore.Http;

namespace Core.Interfaces.Services
{
    public interface IAWSS3Service
    {
        Task<bool> DoesS3BucketExistAsync(string bucketName = null);
        Task<ExecutionResponse<CreateS3BucketResponseDto>> CreateBucketAsync(string bucketName = null);
        Task<ExecutionResponse<string>> UploadFileAsync(IFormFile file);
        Task<ExecutionResponse<string>> UploadFileAsync(Stream fileStream, string fileName);
    }
}