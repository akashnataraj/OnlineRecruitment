using System;
using System.Collections.Generic;

namespace OREC.API.DAL
{
    public partial class UserSkillMap
    {
        public int UserSkillMapId { get; set; }
        public int UserId { get; set; }
        public int SkillId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
