using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlinkCash.Core.Models.AWSImageUploadModels
{
   public class UploadRequestModel : Model
    {
        
        [Required]
        public IFormFile File { get; set; }
        public Dictionary<string, string> Metatags { get; set; }
    }
}
