using System;
using System.Collections.Generic;

namespace OREC.API.DAL
{
    public partial class Skills
    {
        public int SkillId { get; set; }
        public string SkillTitle { get; set; }
        public bool IsDeleted { get; set; }
    }
}
