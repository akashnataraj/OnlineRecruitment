using Microsoft.Extensions.Configuration;
using OREC.BL.DTO;
using OREC.DAL.IRepository;
using OREC.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OREC.DAL.Repository
{
    public class AccountsRepository : IAccountsRepository
    {
        private readonly IConfiguration Configuration;

        public AccountsRepository(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public RegisterDTO RegisterUser(RegisterDTO register)
        {
            try
            {
                using (var ef = new ORECContext())
                {
                    if (ef.Users.Where(x => x.EmailID.Trim() == register.EmailID.Trim() && x.IsDeleted == false).Count() > 0)
                    {
                        register.Code = Configuration.GetValue<int>("ResponseStatus:DuplicateUsername:Code");
                    }
                    else
                    {
                        Users user = new Users()
                        {
                            RoleID = Configuration.GetValue<int>("Role:User"),
                            EmailID = register.EmailID,
                            Password = register.Password
                        };
                        ef.Users.Add(user);
                        ef.SaveChanges();

                        UserDetails userDetails = new UserDetails()
                        {
                            UserID = user.UserID,
                            FirstName = register.FirstName,
                            LastName = register.LastName
                        };
                        ef.UserDetails.Add(userDetails);
                        ef.SaveChanges();

                        register.Code = Configuration.GetValue<int>("ResponseStatus:Success:Code");
                        register.Success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                register.Code = Configuration.GetValue<int>("ResponseStatus:Error:Code");
                register.Description = ex.Message;
                register.Success = false;
            }
            return register;
        }
        public AccountsDTO GetUserOnLogin(LoginDTO login)
        {
            AccountsDTO loginResponse = new AccountsDTO();
            try
            {
                using (var ef = new ORECContext())
                {
                    var user = ef.Users.Where(x => x.EmailID == login.EmailID.Trim() && x.Password == login.Password && x.IsDeleted == false).FirstOrDefault();
                    if (user != null)
                    {
                        loginResponse.UserId = user.UserID;
                        loginResponse.RoleId = user.RoleID;
                        loginResponse.Username = ef.UserDetails.Where(x => x.UserID == user.UserID && x.IsDeleted == false).Select(x => x.FirstName + " " + x.LastName).FirstOrDefault();
                        loginResponse.Code = Configuration.GetValue<int>("ResponseStatus:Success:Code");
                        loginResponse.Success = true;
                    }
                    else
                    {
                        loginResponse.Code = Configuration.GetValue<int>("ResponseStatus:InvalidLogin:Code");
                        loginResponse.Success = false;
                    }
                }
            }
            catch (Exception ex)
            {
                loginResponse.Code = Configuration.GetValue<int>("ResponseStatus:Error:Code");
                loginResponse.Description = ex.Message;
                loginResponse.Success = false;
            }
            return loginResponse;
        }
    }
}
