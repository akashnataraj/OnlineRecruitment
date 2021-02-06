using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OREC.DAL.Models
{
    public class Users
    {
        public int UserID { get; set; }
        public int RoleID { get; set; }
        public string EmailID { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
