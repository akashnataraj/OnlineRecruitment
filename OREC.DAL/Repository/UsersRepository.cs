using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using OREC.DAL.Models;
using OREC.BL.DTO;
using OREC.DAL.IRepository;
using System.Linq;

namespace OREC.DAL.Repository
{
    public class UsersRepository : IUsersRepository
    {
        private readonly IConfiguration Configuration;

        public UsersRepository(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public UsersDTO GetUserDetails(int id)
        {
            UsersDTO user = new UsersDTO();
            try
            {
                using (var ef = new ORECContext())
                {
                    var userDetails = ef.UserDetails.Where(x => x.UserID == id && x.IsDeleted == false).FirstOrDefault();
                    if (userDetails != null)
                    {
                        user.UserId = userDetails.UserID;
                        user.FirstName = userDetails.FirstName;
                        user.LastName = userDetails.LastName;
                        user.MobileNumber = userDetails.MobileNumber;
                        user.HighestQualification = userDetails.HighestQualification;
                        user.InternalEmployeeId = userDetails.InternalEmployeeId;
                        user.CurrentOrganization = userDetails.CurrentOrganization;
                        user.CurrentPosition = userDetails.CurrentPosition;
                        user.Experience = userDetails.Experience;
                        user.Skills = new List<SkillsDTO>();
                        user.Skills = (from skill in ef.Skills
                                       where skill.IsDeleted == false
                                       select new SkillsDTO
                                       {
                                           SkillID = skill.SkillId,
                                           SkillName = skill.SkillTitle,
                                           IsMapped = ef.UserSkillMap.Where(x => x.SkillId == skill.SkillId
                                                                            && x.UserId == id && x.IsDeleted == false
                                                                            ).Count() > 0 ? true : false

                                       }).ToList();
                        user.Code = Configuration.GetValue<int>("ResponseStatus:Success:Code");
                        user.Success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                user.Code = Configuration.GetValue<int>("ResponseStatus:Error:Code");
                user.Description = ex.Message;
                user.Success = true;
            }
            return user;
        }
        public StatusResponse UpdateUser(UsersDTO userDTO)
        {
            StatusResponse res = new StatusResponse();
            try
            {
                using (var ef = new ORECContext())
                {
                    //update user details
                    UserDetails user = ef.UserDetails.Where(x => x.UserID == userDTO.UserId && x.IsDeleted == false).FirstOrDefault();
                    user.FirstName = userDTO.FirstName;
                    user.LastName = userDTO.LastName;
                    user.HighestQualification = userDTO.HighestQualification;
                    user.CurrentOrganization = userDTO.CurrentOrganization;
                    user.CurrentPosition = userDTO.CurrentPosition;
                    user.Experience = userDTO.Experience;
                    user.MobileNumber = userDTO.MobileNumber;
                    user.InternalEmployeeId = userDTO.InternalEmployeeId;
                    user.ModifiedDate = DateTime.Now;
                    ef.UserDetails.Update(user);
                    ef.SaveChanges();

                    if (userDTO.Skills != null)
                    {
                        //remove current user skills
                        var skills = ef.UserSkillMap.Where(x => x.UserId == userDTO.UserId && x.IsDeleted == false).ToList();
                        skills.ForEach(x => x.IsDeleted = true);
                        ef.SaveChanges();

                        //add new skills
                        var newSkills = userDTO.Skills.Where(x => x.IsMapped == true).ToList();
                        foreach (var item in newSkills)
                        {
                            UserSkillMap userSkill = new UserSkillMap
                            {
                                SkillId = item.SkillID,
                                UserId = userDTO.UserId
                            };
                            ef.UserSkillMap.Add(userSkill);
                        }
                        ef.SaveChanges();
                    }

                    res.Code = Configuration.GetValue<int>("ResponseStatus:Success:Code");
                    res.Success = true;
                }
            }
            catch (Exception ex)
            {
                res.Code = Configuration.GetValue<int>("ResponseStatus:Error:Code");
                res.Description = ex.Message;
                res.Success = false;
            }
            return res;
        }
    }
}
