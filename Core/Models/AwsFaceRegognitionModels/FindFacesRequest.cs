using System.ComponentModel.DataAnnotations;

namespace Core.Models.AwsFaceRegognitionModels
{
    public class FindFacesRequest: Model
    {
        [Required]
        public string SourceImage { get; set; }
    }
}
