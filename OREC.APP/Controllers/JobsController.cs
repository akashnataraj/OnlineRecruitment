using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OREC.APP.Common;
using OREC.BL.DTO;
using OREC.DAL.IRepository;

namespace OnlineRecruitment.Controllers
{
    public class JobsController : Controller
    {
        public JobsController(IConfiguration configuration, Util util, IJobsRepository jobsRepository)
        {
            Configuration = configuration;
            _util = util;
            _jobRepo = jobsRepository;
        }
        private Util _util { get; }
        private IConfiguration Configuration { get; }
        private IJobsRepository _jobRepo { get; }

        [HttpGet]
        public IActionResult Dashboard()
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
                    int userId = Convert.ToInt32(userdata["userid"]);
                    int roleId = Convert.ToInt32(userdata["roleid"]);

                    var Res = _jobRepo.GetAllJobs(userId, roleId);
                    if (Res.Code == Configuration.GetValue<int>("ResponseStatus:Success:Code"))
                    {
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

        [HttpGet]
        public IActionResult JobDetail(int jobId = 0)
        {
            JobsDTO job = new JobsDTO();
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
                    int userId = Convert.ToInt32(userdata["userid"]);
                    int roleId = Convert.ToInt32(userdata["roleid"]);

                    job = _jobRepo.GetJobbyId(jobId, userId, roleId);
                    if (job.Code == Configuration.GetValue<int>("ResponseStatus:Success:Code"))
                    {
                        return View(job);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View(job);
        }

        [HttpPost]
        public IActionResult JobDetail(JobsDTO job)
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
                    job.UserId = Convert.ToInt32(userdata["userid"]);
                    job.RoleId = Convert.ToInt32(userdata["roleid"]);
                }
                if (ModelState.IsValid)
                {
                    var Res = job.JobId == 0 ? _jobRepo.AddJobs(job) : _jobRepo.UpdateJobs(job);
                    return Json(Res);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View(job);
        }

        [HttpPost]
        public IActionResult ApplyJob(int jobId, bool apply)
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
                    int userId = Convert.ToInt32(userdata["userid"]);
                    int roleId = Convert.ToInt32(userdata["roleid"]);

                    var Res = _jobRepo.ApplyJob(jobId, userId, apply);
                    return Json(Res);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();
        }

        [HttpPost]
        public ActionResult DeleteJob(int jobId)
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
                    int userId = Convert.ToInt32(userdata["userid"]);
                    var Res = _jobRepo.DeleteJobs(jobId, userId);
                    return Json(Res);
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