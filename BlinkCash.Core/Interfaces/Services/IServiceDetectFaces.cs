using BlinkCash.Core.Dtos;
using BlinkCash.Core.Models.AwsFaceRegognitionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlinkCash.Core.Interfaces.Services
{
    public interface IServiceDetectFaces
    {
        Task<ExecutionResponse<FindFacesResponse>> DetectFacesAsync(string sourceImage);
    }
}
