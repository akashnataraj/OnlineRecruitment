using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OREC.BL.DTO
{
    public class AccountsDTO : StatusResponse
    {
        //public int UserId { get; set; }
        //public int RoleId { get; set; }
        public string Username { get; set; }
    }

    public class LoginDTO : StatusResponse
    {
        [Required(ErrorMessage = "Please enter email!")]
        public string EmailID { get; set; }
        [Required(ErrorMessage = "Please enter password!")]
        public string Password { get; set; }
    }

    public class RegisterDTO : StatusResponse
    {
        [EmailAddress(ErrorMessage = "The Email field is not a valid e-mail address.")]
        [Required(ErrorMessage = "Please select your email!")]
        public string EmailID { get; set; }

        [StringLength(18, ErrorMessage = "The password must be at least 8 characters long.", MinimumLength = 8)]
        [Required(ErrorMessage = "Please select a password!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter your first name!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your last name!")]
        public string LastName { get; set; }
    }
}
