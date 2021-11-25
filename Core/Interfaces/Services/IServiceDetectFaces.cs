using Core.Dtos;
using Core.Models.AwsFaceRegognitionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IServiceDetectFaces
    {
        Task<ExecutionResponse<FindFacesResponse>> DetectFacesAsync(string sourceImage);
    }
}
