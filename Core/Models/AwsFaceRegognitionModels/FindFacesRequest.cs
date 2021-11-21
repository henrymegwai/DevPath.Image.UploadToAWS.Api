using System.ComponentModel.DataAnnotations;

namespace BlinkCash.Core.Models.AwsFaceRegognitionModels
{
    public class FindFacesRequest: Model
    {
        [Required]
        public string SourceImage { get; set; }
    }
}
