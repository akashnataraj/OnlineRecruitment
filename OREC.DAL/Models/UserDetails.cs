using System;
using System.Collections.Generic;
using System.Text;

namespace OREC.DAL.Models
{
    public class UserDetails
    {
        public int UserDetailsID { get; set; }
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string HighestQualification { get; set; }
        public double Experience { get; set; }
        public long? InternalEmployeeId { get; set; }
        public string CurrentOrganization { get; set; }
        public string CurrentPosition { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
