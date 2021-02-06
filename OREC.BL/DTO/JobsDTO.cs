using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OREC.BL.DTO
{
    public class JobsDTO : StatusResponse
    {
        public int JobId { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "Please enter job title!")]
        public string Title { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "Please enter job description!")]
        public string JobDescription { get; set; }

        [Display(Name = "Location")]
        [Required(ErrorMessage = "Please enter job location!")]
        public string City { get; set; }

        [Display(Name = "State Code")]
        [StringLength(2, ErrorMessage = "The State code must be 2 characters long.", MinimumLength = 2)]
        [Required(ErrorMessage = "Please enter state code!")]
        public string State { get; set; }

        [Display(Name = "Job Type")]
        [Required(ErrorMessage = "Please select job type!")]
        public string JobType { get; set; }

        [Display(Name = "Experience")]
        [Required(ErrorMessage = "Please enter required experience!")]
        public int RequiredExperience { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string PostedBefore { get; set; }
        public bool HasApplied { get; set; }
        public int AppliedUserCount { get; set; }
        public int MatchingSkillCount { get; set; }
        public int TotalJobSkills { get; set; }
        public double UserScore { get; set; }
        public List<SkillsDTO> Skills { get; set; }
        public List<UsersDTO> AppliedUsers { get; set; }
    }

    public class JobsDashboardDTO : StatusResponse
    {
        public List<JobsDTO> Jobs { get; set; }
    }
}
