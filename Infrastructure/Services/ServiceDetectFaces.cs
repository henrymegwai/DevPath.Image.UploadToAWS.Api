using Amazon.Rekognition;
using System.Threading.Tasks;
using Amazon.Rekognition.Model;
using System.Collections.Generic;
using BlinkCash.Core.Interfaces.Services;
using BlinkCash.Core.Models.AwsFaceRegognitionModels;
using System;
using BlinkCash.Core.Dtos;
using BlinkCash.Core.Configs;
using Microsoft.Extensions.Options;

namespace BlinkCash.Infrastructure.Services
{
    public class ServiceDetectFaces : IServiceDetectFaces
    {
        private readonly IImageServiceUtils _imageServiceUtils;
        private readonly AmazonRekognitionClient _rekognitionClient;
        private readonly AWSS3Settings _s3Settings;
        public ServiceDetectFaces(IImageServiceUtils serviceUtils, IOptions<AWSS3Settings> s3Options)
        {
            _imageServiceUtils = serviceUtils;
            _s3Settings = s3Options.Value;
            _rekognitionClient = new AmazonRekognitionClient(_s3Settings.AccessKeyId, _s3Settings.SecretAccessKey, Amazon.RegionEndpoint.USEast2);
        }

        public async Task<ExecutionResponse<FindFacesResponse>> DetectFacesAsync(string sourceImage)
        {
            ExecutionResponse<FindFacesResponse> findFaceResponse = new ExecutionResponse<FindFacesResponse>();
            try
            {
                //Converts the source image to a MemoryStream object
                var imageSource = new Image();
                imageSource.Bytes = _imageServiceUtils.ConvertImageToMemoryStream(sourceImage);

                // Configures the object that will make the request to AWS Rekognition
                var request = new DetectFacesRequest
                {
                    Attributes = new List<string> { "DEFAULT" },
                    Image = imageSource
                };

                // Call the DetectFaces service            
                var response = await _rekognitionClient.DetectFacesAsync(request);
                // Call the draw squares function and get the generated URL
                var fileName = _imageServiceUtils.Drawing(imageSource.Bytes, response.FaceDetails);

                // Returns the object with the generated URL
                findFaceResponse.Data = new FindFacesResponse(fileName);
                findFaceResponse.Status = true;
                findFaceResponse.Message = "Find Face request was succesfully process";
                return findFaceResponse;  
            }
            catch(Exception ex) 
            {
                findFaceResponse.Message = ex.Message;
                findFaceResponse.Data = null;
                findFaceResponse.Status = false;
                return findFaceResponse;
            }
            
        }
    }
}