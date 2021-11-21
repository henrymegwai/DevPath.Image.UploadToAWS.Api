using BlinkCash.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO; 
using aws = Amazon.Rekognition.Model;

namespace BlinkCash.Infrastructure.Services
{
   
    public class ImageServiceUtils : IImageServiceUtils
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ImageServiceUtils(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // Converts a base64 string image to MemoryStream
        public MemoryStream ConvertImageToMemoryStream(string imageBase64)
        {
            var bytes = Convert.FromBase64String(imageBase64);
            return new MemoryStream(bytes);
        }

        // Overload function that receives the CompareFaces parameter
        public string Drawing(MemoryStream imageSource, aws.ComparedSourceImageFace faceMatch)
        {
            // Creates a list of squares that will be drawn on the image
            var squares = new List<aws.BoundingBox>();

            // Adiciona as posições dos quadrados que serão desenhados
            squares.Add(
                new aws.BoundingBox
                {
                    Left = faceMatch.BoundingBox.Left,
                    Top = faceMatch.BoundingBox.Top,
                    Width = faceMatch.BoundingBox.Width,
                    Height = faceMatch.BoundingBox.Height
                }
            );

            return Drawing(imageSource, squares);
        }

        // Overload function that receives the DetectFaces parameter
        public string Drawing(MemoryStream imageSource, List<aws.FaceDetail> faceDetails)
        {
            // Creates a list of squares that will be drawn on the image
            var squares = new List<aws.BoundingBox>();

            // Add the positions of the squares that will be drawn
            faceDetails.ForEach(f => {
                squares.Add(
                    new aws.BoundingBox
                    {
                        Left = f.BoundingBox.Left,
                        Top = f.BoundingBox.Top,
                        Width = f.BoundingBox.Width,
                        Height = f.BoundingBox.Height
                    }
                );
            });

            return Drawing(imageSource, squares);
        }

        // Function that draws the squares in the source image
        private string Drawing(MemoryStream imageSource, List<aws.BoundingBox> squares)
        {
            //Converts MemoryStream from source image to Image (System.Drawing)
            var image = Image.FromStream(imageSource);
            // Graphics lets you draw new features in the source image
            var graphic = Graphics.FromImage(image);
            //Pen object that will be used to draw the squares
            var pen = new Pen(Brushes.Red, 5f);

            // Draw the squares in the source image
            squares.ForEach(b => {
                graphic.DrawRectangle(
                    pen,
                    b.Left * image.Width,
                    b.Top * image.Height,
                    b.Width * image.Width,
                    b.Height * image.Height
                );
            });

            // Create the filename with the Guid
            var fileName = $"{Guid.NewGuid()}.jpg";

            // Save the new drawn image
            image.Save($"Images/{fileName}", ImageFormat.Jpeg);

            // Calls the function and returns the URL with the generated image
            return GetUrlImage(fileName);
        }

        // Generate a URL with the created image
        private string GetUrlImage(string fileName)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var urlImage = $"{request.Scheme}://{request.Host.ToUriComponent()}/images/{fileName}";

            return urlImage;
        }
    }
}
