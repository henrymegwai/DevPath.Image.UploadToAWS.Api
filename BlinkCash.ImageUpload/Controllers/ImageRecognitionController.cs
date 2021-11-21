using BlinkCash.Core.Dtos;
using BlinkCash.Core.Interfaces.Services;
using BlinkCash.Core.Models.AwsFaceRegognitionModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlinkCash.ImageUpload.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageRecognitionController : ControllerBase
    {
        
        private readonly IServiceDetectFaces _serviceDetectFaces;
        private readonly IServiceCompareFaces _serviceCompareFaces;

        public ImageRecognitionController(
            IServiceDetectFaces serviceDetectFaces,
            IServiceCompareFaces serviceCompareFaces )
        {
            _serviceDetectFaces = serviceDetectFaces;
            _serviceCompareFaces = serviceCompareFaces;
             
        }

        [HttpPost("facematch")]
        public async Task<IActionResult> GetFaceMatches([FromBody] FaceMatchRequest faceMatchRequest)
        {

            try
            {
                faceMatchRequest.Validate();
                
                var response = await _serviceCompareFaces.CompareFacesAsync(faceMatchRequest.SourceImage, faceMatchRequest.TargetImage);
                if (!response.Status)
                    return BadRequest(response);

                return Ok(response);
            }
            catch (Exception ex)
            {
                ExecutionResponse<string> response = new ExecutionResponse<string>();
                response.Message = ex.Message;
                return BadRequest(response);
            }

        }

        [HttpPost("findfaces")]
        public async Task<IActionResult> GetFaceMatches([FromBody] FindFacesRequest request)
        {
            try
            {
                request.Validate(); 
                var response = await _serviceDetectFaces.DetectFacesAsync(request.SourceImage);
                if (!response.Status)
                    return BadRequest(response);

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

    }
}
