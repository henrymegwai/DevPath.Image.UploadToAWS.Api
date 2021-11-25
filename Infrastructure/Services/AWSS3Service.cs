using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Core.Configs;
using Core.Dtos;
using Core.Interfaces.Services;
using Core.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class AWSS3Service : IAWSS3Service
    {
        private readonly IAmazonS3 _awsS3Client;
        private readonly AWSS3Settings _s3Settings;
        public AWSS3Service(IAmazonS3 awsS3Client, IOptions<AWSS3Settings> s3Options)
        {
            _awsS3Client = awsS3Client;
            _s3Settings = s3Options.Value;
        }

        public async Task<bool> DoesS3BucketExistAsync(string bucketName = null)
        {
            return await _awsS3Client.DoesS3BucketExistAsync(bucketName ?? _s3Settings.BucketName);
        }

        public async Task<ExecutionResponse<CreateS3BucketResponseDto>> CreateBucketAsync(string bucketName = null)
        {
            ExecutionResponse<CreateS3BucketResponseDto> createbucketresponse = new ExecutionResponse<CreateS3BucketResponseDto>();
            try
            {
                var putBucketRequest = new PutBucketRequest
                {
                    BucketName = bucketName ?? _s3Settings.BucketName,
                    UseClientRegion = true
                };

                var response = await _awsS3Client.PutBucketAsync(putBucketRequest);
                createbucketresponse.Data = new CreateS3BucketResponseDto
                {
                    BucketName = bucketName ?? _s3Settings.BucketName,
                    RequestId = response.ResponseMetadata.RequestId
                };
                createbucketresponse.Message = $"Bucket with name {bucketName} was successfully created.";
                createbucketresponse.Status = true;
                return createbucketresponse;
            }
            catch (AmazonS3Exception ex)
            {
                ExecutionResponse<CreateS3BucketResponseDto> response = new ExecutionResponse<CreateS3BucketResponseDto>();
                response.Message = ex.Message;
                response.Data = null;
                response.Status = false;
                return response;
            }
            catch (Exception ex)
            {
                ExecutionResponse<CreateS3BucketResponseDto> response = new ExecutionResponse<CreateS3BucketResponseDto>();
                response.Message = ex.Message;
                response.Data = null;
                response.Status = false;
                return response;
            }
        }

        public async Task<ExecutionResponse<string>> UploadFileAsync(Stream fileStream,
            string fileName)
        {
            try
            {
                var fileNamePart = fileName.Substring(0, fileName.LastIndexOf("."));
                var fileExtensionPart = fileName.Substring(fileName.LastIndexOf("."));
                fileName = $"{fileNamePart}_{Guid.NewGuid().ToString().Substring(0, 3)}{fileExtensionPart}";
                var fileTransferRequest = new TransferUtilityUploadRequest
                {
                    InputStream = fileStream,
                    BucketName =  _s3Settings.BucketName,
                    Key = $"{_s3Settings.ApplicationFolder}/{fileName}",
                    CannedACL = S3CannedACL.PublicRead,
                    StorageClass = S3StorageClass.ReducedRedundancy
                };

                using (var fileTransferUtility = new TransferUtility(_awsS3Client))
                {
                    await fileTransferUtility.UploadAsync(fileTransferRequest);
                }

                return new ExecutionResponse<string>
                {
                    Status = true,
                    Message = "File uploaded successfully.",
                    Data = $"{_s3Settings.ServiceUrl}/{_s3Settings.ApplicationFolder}/{fileTransferRequest.Key}"
                };
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                 amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    return new ExecutionResponse<string>
                    {
                        Message = "Please check the provided AWS Credentials."
                    };
                }
                else
                {
                    return new ExecutionResponse<string>
                    {
                        Message = $"Error - {amazonS3Exception.Message}' while uploading {fileName}."
                    };
                }
            }
        }

        public async Task<ExecutionResponse<string>> UploadFileAsync(IFormFile file)
        {
            string fileName = WebUtility.HtmlEncode(file.FileName);
            try
            {                
                var fileNamePart = fileName.Substring(0, fileName.LastIndexOf("."));
                var fileExtensionPart = fileName.Substring(fileName.LastIndexOf("."));
                fileName = $"{fileNamePart}_{Guid.NewGuid().ToString().Substring(0, 3)}{fileExtensionPart}";
                var memoryStream = file.OpenReadStream();
                var fileTransferRequest = new TransferUtilityUploadRequest
                {
                    InputStream = memoryStream,
                    BucketName = _s3Settings.BucketName,
                    Key = $"{_s3Settings.ApplicationFolder}/{fileName.Replace(" ", "_")}",
                    CannedACL = S3CannedACL.PublicRead,
                    StorageClass = S3StorageClass.ReducedRedundancy
                };

                using (var fileTransferUtility = new TransferUtility(_awsS3Client))
                {
                    await fileTransferUtility.UploadAsync(fileTransferRequest);
                }

                return new ExecutionResponse<string>
                {
                    Status = true,
                    Message = "File uploaded successfully.",
                    Data = $"{_s3Settings.ServiceUrl}/{_s3Settings.ApplicationFolder}/{fileTransferRequest.Key}"
                };
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                 amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    return new ExecutionResponse<string>
                    {
                        Message = "Please check the provided AWS Credentials."
                    };
                }
                else
                {
                    return new ExecutionResponse<string>
                    {
                        Message = $"Error - {amazonS3Exception.Message}' while uploading {fileName}."
                    };
                }
            }
        }
    }
}
