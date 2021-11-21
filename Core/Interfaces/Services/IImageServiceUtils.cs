using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aws = Amazon.Rekognition.Model;

namespace BlinkCash.Core.Interfaces.Services
{
    public interface IImageServiceUtils
    {
        MemoryStream ConvertImageToMemoryStream(string imageBase64);
        string Drawing(MemoryStream imageSource, aws.ComparedSourceImageFace faceMatch);
        string Drawing(MemoryStream imageSource, List<aws.FaceDetail> faceDetails);
    }
}
