using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlinkCash.Core.Models.Dtos
{
    public class CreateS3BucketResponseDto
    {
        public string RequestId { get; set; }
        public string BucketName { get; set; }
    }
}
