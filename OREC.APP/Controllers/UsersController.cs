using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OREC.BL.DTO;
using OREC.APP.Common;
using OREC.DAL.IRepository;

namespace OnlineRecruitment.Controllers
{
    public class UsersController : Controller
    {
        public UsersController(IConfiguration configuration, Util util, IUsersRepository usersRepository)
        {
            Configuration = configuration;
            _util = util;
            _userRepo = usersRepository;
        }
        private Util _util { get; }
        private IConfiguration Configuration { get; }
        private IUsersRepository _userRepo { get; }

        [HttpGet]
        public IActionResult Profile(int uid = 0)
        {
            try
            {
                if (_util.GetUserrInfo().Count == 0)
                {
                    _util.ClearSession();
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    var userdata = _util.GetUserrInfo();
                    uid = uid > 0 ? uid : Convert.ToInt32(userdata["userid"]);
                }

                if (ModelState.IsValid)
                {
                    var Res = _userRepo.GetUserDetails(uid);
                    if (Res.Code == Configuration.GetValue<int>("ResponseStatus:Success:Code"))
                    {
                        Res.Code = 0;
                        return View(Res);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();
        }

        [HttpPost]
        public IActionResult Profile(UsersDTO userDetail)
        {
            try
            {
                if (_util.GetUserrInfo().Count == 0)
                {
                    _util.ClearSession();
                    return RedirectToAction("Login", "Account");
                }
                if (ModelState.IsValid)
                {
                    var Res = _userRepo.UpdateUser(userDetail);
                    if (Res.Code != Configuration.GetValue<int>("ResponseStatus:Error:Code"))
                    {
                        userDetail.Code = Res.Code;
                        userDetail.Description = Res.Description;
                        return View(userDetail);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError("customError", Configuration.GetValue<string>("ResponseStatus:Error:Message"));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();
        }
    }
}