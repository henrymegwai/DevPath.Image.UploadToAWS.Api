using System.ComponentModel.DataAnnotations;

namespace Core.Models.AwsFaceRegognitionModels
{
    public class FaceMatchRequest : Model
    {
        [Required]
        public string SourceImage { get; set; }
        [Required]
        public string TargetImage { get; set; }
    }
}
