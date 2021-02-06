using System;
using System.Collections.Generic;

namespace OREC.API.DAL
{
    public partial class Jobs
    {
        public int JobId { get; set; }
        public string JobTitle { get; set; }
        public string JobDesc { get; set; }
        public string JobType { get; set; }
        public int RequiredExperience { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
