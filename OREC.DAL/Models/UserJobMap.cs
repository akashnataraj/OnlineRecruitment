using System;
using System.Collections.Generic;

namespace OREC.DAL.Models
{
    public class UserJobMap
    {
        public int UserJobMapId { get; set; }
        public int UserId { get; set; }
        public int JobId { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
