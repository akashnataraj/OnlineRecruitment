using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OREC.BL.DTO;
using OREC.DAL.IRepository;

namespace OREC.API.Controllers
{
    public class UsersController : Controller
    {
        private IUsersRepository _users;
        public IConfiguration Configuration { get; }

        public UsersController(IUsersRepository u, IConfiguration config)
        {
            _users = u;
            Configuration = config;
        }

        [Route("api/User/GetUserDetails")]
        [HttpGet]
        public ActionResult<UsersDTO> GetUserDetails(int id)
        {
            UsersDTO user = new UsersDTO();
            try
            {
                user = _users.GetUserDetails(id);
                return (user.Code != Configuration.GetValue<int>("ResponseStatus:Error:Code"))
                       ? Ok(user) : StatusCode(StatusCodes.Status500InternalServerError, user.Description);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Route("api/User/SaveUserDetails")]
        [HttpPost]
        public ActionResult<StatusResponse> SaveUserDetails([FromBody] UsersDTO usersDTO)
        {
            StatusResponse res = new StatusResponse();
            try
            {
                res = _users.UpdateUser(usersDTO);
                return (res.Code != Configuration.GetValue<int>("ResponseStatus:Error:Code"))
                       ? Ok(res) : StatusCode(StatusCodes.Status500InternalServerError, res.Description);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}