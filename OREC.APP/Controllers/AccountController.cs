using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OREC.APP.Common;
using OREC.BL.DTO;
using OREC.DAL.IRepository;

namespace OnlineRecruitment.Controllers
{
    public class AccountController : Controller
    {
        private readonly Util _util;
        private IAccountsRepository _accRepo { get; }
        private IConfiguration Configuration { get; }
        public AccountController(IConfiguration configuration, Util util, IAccountsRepository accountsRepository)
        {
            Configuration = configuration;
            _util = util;
            _accRepo = accountsRepository;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginDTO login)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var Res = _accRepo.GetUserOnLogin(login);
                    if (Res.Code != Configuration.GetValue<int>("ResponseStatus:Error:Code"))
                    {
                        if (Res.Code == Configuration.GetValue<int>("ResponseStatus:InvalidLogin:Code"))
                        {
                            ModelState.Clear();
                            ModelState.AddModelError("customError", Configuration.GetValue<string>("ResponseStatus:InvalidLogin:Message"));
                        }
                        else
                        {
                            _util.SaveUserInfo(Res);
                            return RedirectToAction("Dashboard", "Jobs");
                        }
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

        [HttpGet]
        public IActionResult Register()
        {
            RegisterDTO register = new RegisterDTO();
            return View(register);
        }

        [HttpPost]
        public ActionResult Register(RegisterDTO register)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var Res = _accRepo.RegisterUser(register);
                    if (Res.Code != Configuration.GetValue<int>("ResponseStatus:Error:Code"))
                    {
                        if (Res.Code == Configuration.GetValue<int>("ResponseStatus:DuplicateUsername:Code"))
                        {
                            ModelState.Clear();
                            ModelState.AddModelError("customError", Configuration.GetValue<string>("ResponseStatus:DuplicateUsername:Message"));
                        }
                        else
                        {
                            return View(Res);
                        }
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
        public ActionResult Logout()
        {
            _util.ClearSession();
            return RedirectToAction("Login");
        }
    }
}