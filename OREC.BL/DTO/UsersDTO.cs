using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OREC.BL.DTO
{
    public class UsersDTO : StatusResponse
    {
        public int RoleId { get; set; }
        public int UserId { get; set; }
        public string EmailID { get; set; }
        public string Password { get; set; }

        [Display(Name="First Name")]
        [Required(ErrorMessage = "Please enter your first name!")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please enter your last name!")]
        public string LastName { get; set; }

        [Display(Name = "Mobile number")]
        [Required(ErrorMessage = "Please enter your mobile number!")]
        public string MobileNumber { get; set; }

        [Display(Name = "Highest Qualification")]
        [Required(ErrorMessage = "Please enter your highest qualification!")]
        public string HighestQualification { get; set; }

        [Display(Name = "Experience (in years)")]
        [Required(ErrorMessage = "Please enter your work experience in years!")]
        public double Experience { get; set; }

        [Display(Name = "Internal Employee ID")]
        public long? InternalEmployeeId { get; set; }

        [Display(Name = "Current Organization")]
        [Required(ErrorMessage = "Please enter your current organization!")]
        public string CurrentOrganization { get; set; }

        [Display(Name = "Current Position")]
        [Required(ErrorMessage = "Please enter your current position!")]
        public string CurrentPosition { get; set; }
        public List<SkillsDTO> Skills {get;set;} 
        public double Score { get; set; }
    }
}
