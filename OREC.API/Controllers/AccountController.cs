using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OREC.BL.DTO;
using OREC.DAL.IRepository;

namespace OREC.API.Controllers
{
    
    public class AccountController : Controller
    {
        private IUsersRepository _users;
        public IConfiguration Configuration { get; }

        public AccountController(IUsersRepository u, IConfiguration config)
        {
            _users = u;
            Configuration = config;
        }

        [Route("api/Account/Login")]
        [HttpPost]
        public ActionResult<AccountsDTO> Login([FromBody]LoginDTO login)
        {
            AccountsDTO loginResponse = new AccountsDTO();
            try
            {
                loginResponse = _users.GetUserOnLogin(login);
                return (loginResponse.Code != Configuration.GetValue<Int32>("ResponseStatus:Error:Code") ) 
                       ? Ok(loginResponse) : StatusCode(StatusCodes.Status500InternalServerError, loginResponse.Description);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Route("api/Account/Register")]
        [HttpPost]
        public ActionResult<RegisterDTO> Register([FromBody]RegisterDTO register)
        {
            RegisterDTO regResponse = new RegisterDTO();
            try
            {
                regResponse = _users.RegisterUser(register);
                return (regResponse.Code != Configuration.GetValue<Int32>("ResponseStatus:Error:Code"))
                       ? Ok(regResponse) : StatusCode(StatusCodes.Status500InternalServerError, regResponse.Description);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}