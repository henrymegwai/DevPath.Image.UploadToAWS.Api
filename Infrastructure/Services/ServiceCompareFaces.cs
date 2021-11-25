using System.Linq;
using Amazon.Rekognition;
using System.Threading.Tasks;
using Amazon.Rekognition.Model;
using System;
using Amazon.Runtime;
using Microsoft.Extensions.Options;
using Core.Interfaces.Services;
using Core.Configs;
using Core.Models.AwsFaceRegognitionModels;
using Core.Dtos;

namespace Infrastructure.Services
{
    public class ServiceCompareFaces : IServiceCompareFaces
    {
        private readonly IImageServiceUtils _imageServiceUtils;
        private readonly AmazonRekognitionClient _rekognitionClient;
        private readonly AWSS3Settings _s3Settings;
        public ServiceCompareFaces(IImageServiceUtils serviceUtils, IOptions<AWSS3Settings> s3Options)
        {
            _imageServiceUtils = serviceUtils;
            _s3Settings = s3Options.Value;
            _rekognitionClient = new AmazonRekognitionClient(_s3Settings.AccessKeyId, _s3Settings.SecretAccessKey, Amazon.RegionEndpoint.USEast2);
        }

        public async Task<ExecutionResponse<FaceMatchResponse>> CompareFacesAsync(string sourceImage, string targetImage)
        {
            ExecutionResponse<FaceMatchResponse> faceResponse = new ExecutionResponse<FaceMatchResponse>();
            try 
            {
                // Converts the source image to a MemoryStream object
                var imageSource = new Image();
                imageSource.Bytes = _imageServiceUtils.ConvertImageToMemoryStream(sourceImage);

                // Converts the target image to a MemoryStream object
                var imageTarget = new Image();
                imageTarget.Bytes = _imageServiceUtils.ConvertImageToMemoryStream(targetImage);

                // Configures the object that will make the request for the AWS Rekognition
                // The SimilarityThreshold property adjusts the similarity level when comparing images
                var request = new CompareFacesRequest
                {
                    SourceImage = imageSource,
                    TargetImage = imageTarget,
                    SimilarityThreshold = 80f
                };

                // Call the CompareFaces service
                var response = await _rekognitionClient.CompareFacesAsync(request);
                // Check if there were any matches in the images
                var hasMatch = response.FaceMatches.Any();

                // If there was no match it returns an object not found
                if (!hasMatch)
                {
                    faceResponse.Data = new FaceMatchResponse(hasMatch, null, string.Empty);
                    faceResponse.Status = true;
                    faceResponse.Message = "Face match request was succesfully process";
                    return faceResponse;
                }
                // With the source image and the match return parameters we outline the face found in the image
                var fileName = _imageServiceUtils.Drawing(imageSource.Bytes, response.SourceImageFace);
                // Gets the percentage of similarity of the image found
                var similarity = response.FaceMatches.FirstOrDefault().Similarity;

                // Returns the object with the information found and the URL to check the image
                faceResponse.Data = new FaceMatchResponse(hasMatch, similarity, fileName);
                faceResponse.Status = true;
                faceResponse.Message = "Face match request was succesfully process";
                return faceResponse;
            }
            catch(Exception ex) 
            {
                faceResponse.Message = ex.Message;
                faceResponse.Data = null;
                faceResponse.Status = false;
                return faceResponse;
            }
           
        }
    }
}