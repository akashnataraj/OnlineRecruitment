using System;
using System.Collections.Generic;

namespace OREC.DAL.Models
{
    public class JobSkillMap
    {
        public int JobSkillMapId { get; set; }
        public int JobId { get; set; }
        public int SkillId { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
