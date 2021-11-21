using BlinkCash.Core.Dtos;
using BlinkCash.Core.Interfaces.Services;
using BlinkCash.Core.Models.AwsFaceRegognitionModels;
using BlinkCash.Core.Models.AWSImageUploadModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlinkCash.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IAWSS3Service _aWSS3Service;
      

        public ImageController(
            IAWSS3Service aWSS3Service)
        {
            
            _aWSS3Service = aWSS3Service;
        }
 
         
        [HttpPost("upload")]       
        public async Task<IActionResult> Post([FromForm] UploadRequestModel requestDto)
        {
            try
            {
                requestDto.Validate();
                ExecutionResponse<string> response = new ExecutionResponse<string>();                
                var result = await _aWSS3Service.UploadFileAsync(requestDto.File);
                response.Data = result.Data;
                response.Message = result.Message;
                response.Status = result.Status;
                return Ok(response);
            }
            catch (Exception ex) 
            {
                ExecutionResponse<string> response = new ExecutionResponse<string>();
                response.Message = ex.Message;
                response.Data = null;
                response.Status = false;
                return BadRequest(response);
            }
          
        }

        [HttpPost("create/bucketname")]
        public async Task<IActionResult> CreateS3BucketAsync(string bucketName)
        {
            try
            {
                ExecutionResponse<string> response = new ExecutionResponse<string>();
                if (string.IsNullOrEmpty(bucketName))
                {
                    response.Data = null;
                    response.Status = false;
                    response.Message = $"Bucket name is required";
                    return BadRequest(response);
                }
                var result = await _aWSS3Service.CreateBucketAsync(bucketName);
                if (!result.Status)
                    return BadRequest(result);

                return Ok(result);
            }
            catch(Exception ex) 
            {
                ExecutionResponse<string> response = new ExecutionResponse<string>();
                response.Data = null;
                response.Status = false;
                response.Message = ex.Message;
                return BadRequest(response);
            }
           
        }
    }
}
