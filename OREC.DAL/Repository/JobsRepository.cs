using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using OREC.DAL.Models;
using OREC.DAL.IRepository;
using System.Linq;
using OREC.BL.DTO;

namespace OREC.DAL.Repository
{
    public class JobsRepository : IJobsRepository
    {
        private readonly IConfiguration Configuration;

        public JobsRepository(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public JobsDashboardDTO GetAllJobs(int userId, int roleId)
        {
            JobsDashboardDTO res = new JobsDashboardDTO();
            try
            {
                using (var ef = new ORECContext())
                {
                    res.Jobs = new List<JobsDTO>();
                    var jobs = ef.Jobs.Where(x => x.IsDeleted == false).ToList();

                    if (roleId == Configuration.GetValue<int>("Role:Recruiter"))
                        jobs = jobs.Where(x => x.CreatedBy == userId).ToList();

                    foreach (var item in jobs)
                        res.Jobs.Add(GetJobDetails(item.JobId, userId, roleId));

                    //Sort jobs based on user profile
                    if (roleId == Configuration.GetValue<int>("Role:User"))
                        res.Jobs = res.Jobs.OrderByDescending(x => x.UserScore).ThenByDescending(x => x.MatchingSkillCount).ToList();
                }
                res.Code = Configuration.GetValue<int>("ResponseStatus:Success:Code");
                res.Success = true;
            }
            catch (Exception ex)
            {
                res.Code = Configuration.GetValue<int>("ResponseStatus:Error:Code");
                res.Description = ex.Message;
                res.Success = false;
            }
            return res;
        }
        public JobsDTO GetJobbyId(int jobId, int userId, int roleId)
        {
            JobsDTO res = new JobsDTO();
            try
            {
                res = GetJobDetails(jobId, userId, roleId);
                using (var ef = new ORECContext())
                {
                    res.AppliedUsers = new List<UsersDTO>();
                    res.AppliedUsers = (from uj in ef.UserJobMap
                                        join u in ef.UserDetails on uj.UserId equals u.UserID
                                        where uj.JobId == jobId && uj.IsDeleted == false && u.IsDeleted == false
                                        select new UsersDTO
                                        {
                                            UserId = u.UserID,
                                            FirstName = u.FirstName,
                                            LastName = u.LastName,
                                            Score = UserScore(u.UserID, jobId, Configuration)
                                        }).ToList();
                }
                res.Code = Configuration.GetValue<int>("ResponseStatus:Success:Code");
                res.Success = true;
            }
            catch (Exception ex)
            {
                res.Code = Configuration.GetValue<int>("ResponseStatus:Error:Code");
                res.Description = ex.Message;
                res.Success = false;
            }
            return res;
        }
        public JobsDTO AddJobs(JobsDTO jobsDTO)
        {
            JobsDTO res = new JobsDTO();
            try
            {
                using (var ef = new ORECContext())
                {
                    Jobs job = new Jobs()
                    {
                        JobTitle = jobsDTO.Title,
                        JobDesc = jobsDTO.JobDescription,
                        City = jobsDTO.City,
                        State = jobsDTO.State,
                        JobType = jobsDTO.JobType,
                        RequiredExperience = jobsDTO.RequiredExperience,
                        CreatedBy = jobsDTO.UserId
                    };
                    ef.Jobs.Add(job);
                    ef.SaveChanges();
                    res.JobId = job.JobId;

                    if (jobsDTO.Skills != null)
                    {
                        UpdateJobSkill(jobsDTO.Skills, job.JobId, jobsDTO.UserId, false);
                    }
                }
                res.Code = Configuration.GetValue<int>("ResponseStatus:Success:Code");
                res.Success = true;
            }
            catch (Exception ex)
            {
                res.Code = Configuration.GetValue<int>("ResponseStatus:Error:Code");
                res.Description = ex.Message;
                res.Success = false;
            }
            return res;
        }
        public JobsDTO UpdateJobs(JobsDTO jobsDTO)
        {
            JobsDTO res = new JobsDTO();
            try
            {
                using (var ef = new ORECContext())
                {
                    //update job
                    var job = ef.Jobs.Where(x => x.JobId == jobsDTO.JobId).FirstOrDefault();
                    job.JobTitle = jobsDTO.Title;
                    job.JobDesc = jobsDTO.JobDescription;
                    job.City = jobsDTO.City;
                    job.State = jobsDTO.State;
                    job.JobType = jobsDTO.JobType;
                    job.RequiredExperience = jobsDTO.RequiredExperience;
                    job.ModifiedBy = jobsDTO.UserId;
                    job.ModifiedDate = DateTime.Now;
                    ef.Jobs.Update(job);
                    ef.SaveChanges();

                    //update skills
                    if (jobsDTO.Skills != null)
                    {
                        UpdateJobSkill(jobsDTO.Skills, job.JobId, jobsDTO.UserId, false);
                    }
                }
                res.Code = Configuration.GetValue<int>("ResponseStatus:Success:Code");
                res.Success = true;
            }
            catch (Exception ex)
            {
                res.Code = Configuration.GetValue<int>("ResponseStatus:Error:Code");
                res.Description = ex.Message;
                res.Success = false;
            }
            return res;
        }
        public StatusResponse DeleteJobs(int jobId, int userId)
        {
            StatusResponse res = new StatusResponse();
            try
            {
                using (var ef = new ORECContext())
                {
                    //delete job
                    var job = ef.Jobs.Where(x => x.JobId == jobId).FirstOrDefault();
                    job.IsDeleted = true;
                    job.ModifiedBy = userId;
                    job.ModifiedDate = DateTime.Now;
                    ef.SaveChanges();

                    //delete mapped skills
                    UpdateJobSkill(null, jobId, userId, true);

                    //delete mapped users
                    var users = ef.UserJobMap.Where(x => x.JobId == jobId && x.IsDeleted == false).ToList();
                    users.ForEach(x => x.IsDeleted = true);
                    ef.SaveChanges();
                }
                res.Code = Configuration.GetValue<int>("ResponseStatus:Success:Code");
                res.Success = true;
            }
            catch (Exception ex)
            {
                res.Code = Configuration.GetValue<int>("ResponseStatus:Error:Code");
                res.Description = ex.Message;
                res.Success = false;
            }
            return res;
        }
        public StatusResponse ApplyJob(int jobId, int userId, bool apply)
        {
            StatusResponse res = new StatusResponse();
            try
            {
                using (var ef = new ORECContext())
                {
                    if (apply)
                    {
                        UserJobMap uj = new UserJobMap()
                        {
                            UserId = userId,
                            JobId = jobId,
                            CreatedBy = userId
                        };
                        ef.UserJobMap.Add(uj);
                    }
                    else //withdraw application
                    {
                        var uj = ef.UserJobMap.Where(x => x.UserId == userId && x.JobId == jobId && x.IsDeleted == false).ToList();
                        uj.ForEach(x => x.IsDeleted = true);
                    }
                    ef.SaveChanges();
                }
                res.Code = Configuration.GetValue<int>("ResponseStatus:Success:Code");
                res.Success = true;
            }
            catch (Exception ex)
            {
                res.Code = Configuration.GetValue<int>("ResponseStatus:Error:Code");
                res.Description = ex.Message;
                res.Success = false;
            }
            return res;
        }
        public JobsDTO GetJobDetails(int jobId, int userId, int roleId)
        {
            JobsDTO res = new JobsDTO();
            using (var ef = new ORECContext())
            {
                if (jobId > 0)
                {
                    var job = ef.Jobs.Where(x => x.JobId == jobId && x.IsDeleted == false).FirstOrDefault();
                    res.JobId = job.JobId;
                    res.Title = job.JobTitle;
                    res.JobDescription = job.JobDesc;
                    res.City = job.City;
                    res.State = job.State;
                    res.JobType = job.JobType;
                    res.RequiredExperience = job.RequiredExperience;
                    res.CreatedDate = job.CreatedDate;
                    res.AppliedUserCount = ef.UserJobMap.Where(x => x.JobId == jobId && x.IsDeleted == false).Count();
                    res.TotalJobSkills = ef.JobSkillMap.Where(x => x.JobId == jobId && x.IsDeleted == false).Count();
                    res.PostedBefore = CalculateDayDiff(job.CreatedDate);
                    if (roleId == Configuration.GetValue<int>("Role:User"))
                    {
                        res.HasApplied = ef.UserJobMap.Where(x => x.JobId == jobId && x.UserId == userId && x.IsDeleted == false).Count() > 0
                                     ? true : false;
                        res.MatchingSkillCount = (from js in ef.JobSkillMap
                                                  join us in ef.UserSkillMap on js.SkillId equals us.SkillId
                                                  where js.JobId == jobId && us.UserId == userId &&
                                                  js.IsDeleted == false && us.IsDeleted == false
                                                  select js.SkillId).Distinct().Count();
                        res.UserScore = UserScore(userId, jobId, Configuration);
                    }
                }
                res.Skills = new List<SkillsDTO>();
                res.Skills = (from skill in ef.Skills
                              where skill.IsDeleted == false
                              select new SkillsDTO
                              {
                                  SkillID = skill.SkillId,
                                  SkillName = skill.SkillTitle,
                                  IsMapped = ef.JobSkillMap.Where(x => x.SkillId == skill.SkillId
                                                                   && x.JobId == jobId && x.IsDeleted == false
                                                                   ).Count() > 0 ? true : false

                              }).ToList();
            }
            return res;
        }
        public void UpdateJobSkill(List<SkillsDTO> jobSkills, int jobId, int userId, bool isDel)
        {
            using (var ef = new ORECContext())
            {
                //remove existing skills
                var skills = ef.JobSkillMap.Where(x => x.JobId == jobId && x.IsDeleted == false).ToList();
                skills.ForEach(x => x.IsDeleted = true);
                ef.SaveChanges();

                if (!isDel)
                {
                    //add new skills
                    var newSkills = jobSkills.Where(x => x.IsMapped == true).ToList();
                    foreach (var item in newSkills)
                    {
                        JobSkillMap jobSkill = new JobSkillMap
                        {
                            SkillId = item.SkillID,
                            JobId = jobId,
                            CreatedBy = userId
                        };
                        ef.JobSkillMap.Add(jobSkill);
                    }
                    ef.SaveChanges();
                }
            }
        }
        public static double UserScore(int userId, int jobId, IConfiguration Configuration)
        {
            using (var ef = new ORECContext())
            {
                //Fetch user and job details
                var userDetails = ef.UserDetails.Where(x => x.UserID == userId).FirstOrDefault();
                var jobDetails = ef.Jobs.Where(x => x.JobId == jobId).FirstOrDefault();

                //Skill score = Matching skills * skill multiplier
                int skillScore = (from js in ef.JobSkillMap
                                  join us in ef.UserSkillMap on js.SkillId equals us.SkillId
                                  where js.JobId == jobId && us.UserId == userId &&
                                  js.IsDeleted == false && us.IsDeleted == false
                                  select js.SkillId).Distinct().Count() * Configuration.GetValue<int>("Score:Skill");

                //Experience score = (User experience - Required job experience) * exp multiplier
                double experienceScore = (userDetails.Experience - jobDetails.RequiredExperience) * Configuration.GetValue<int>("Score:Experience");

                //0 for non internal employees
                int internalEmpScore = (userDetails.InternalEmployeeId == null || userDetails.InternalEmployeeId == 0) ? 0
                                       : Configuration.GetValue<int>("Score:InternalEmployee");

                
                return skillScore + experienceScore + internalEmpScore;
            }
        }
        public static string CalculateDayDiff(DateTime createdDate)
        {
            int dayDiff = Math.Abs((createdDate - DateTime.Now).Days);
            string plural = dayDiff > 1 ? "s" : "";
            if (dayDiff < 7)
                return dayDiff + " day" + plural + " ago";
            else if (dayDiff >= 7 && dayDiff < 30)
                return dayDiff / 7 + " week" + plural + " ago";
            else if (dayDiff >= 30 && dayDiff < 365)
                return dayDiff / 30 + " month" + plural + " ago";
            else
                return dayDiff / 365 + " year" + plural + " ago";
        }
    }
}
