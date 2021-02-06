using System;
using System.Collections.Generic;

namespace OREC.API.DAL
{
    public partial class Users
    {

        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string MobileNumber { get; set; }
        public string HighestQualification { get; set; }
        public int? Experience { get; set; }
        public long? InternalEmployeeId { get; set; }
        public string CurrentOrganization { get; set; }
        public string CurrentPosition { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
