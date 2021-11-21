
using BlinkCash.Core.Models;
using BlinkCash.Core.Utilities;
using System;
namespace BlinkCash.Core.Dtos
{
    
    public abstract class BaseDto : Model
    {
        public long Id { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }
        public RecordStatus RecordStatus { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
}
