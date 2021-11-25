using Core.Dtos;
using Core.Models.AwsFaceRegognitionModels;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IServiceCompareFaces
    {
        Task<ExecutionResponse<FaceMatchResponse>> CompareFacesAsync(string sourceImage, string targetImage);
    }
}