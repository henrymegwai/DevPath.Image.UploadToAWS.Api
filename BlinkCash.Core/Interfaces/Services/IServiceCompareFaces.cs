using BlinkCash.Core.Dtos;
using BlinkCash.Core.Models.AwsFaceRegognitionModels;
using System.Threading.Tasks;

namespace BlinkCash.Core.Interfaces.Services
{
    public interface IServiceCompareFaces
    {
        Task<ExecutionResponse<FaceMatchResponse>> CompareFacesAsync(string sourceImage, string targetImage);
    }
}