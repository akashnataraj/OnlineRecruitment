using Microsoft.AspNetCore.Http;
using OREC.BL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OREC.APP.Common
{
    public class Util
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public Util(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Dictionary<string, string> GetUserrInfo()
        {
            Dictionary<string, string> userInfo = new Dictionary<string, string>();
            try
            {
                if (_httpContextAccessor.HttpContext.Session.GetInt32("userid") != null)
                {
                    userInfo.Add("userid", _httpContextAccessor.HttpContext.Session.GetInt32("userid").Value.ToString());
                    userInfo.Add("roleid", _httpContextAccessor.HttpContext.Session.GetInt32("roleid").Value.ToString());
                    userInfo.Add("username", _httpContextAccessor.HttpContext.Session.GetString("username"));
                }
            }
            catch (Exception ex)
            {

            }
            return userInfo;
        }
        public void SaveUserInfo(AccountsDTO login)
        {
            try
            {
                _httpContextAccessor.HttpContext.Session.SetInt32("userid", login.UserId);
                _httpContextAccessor.HttpContext.Session.SetInt32("roleid", login.RoleId);
                _httpContextAccessor.HttpContext.Session.SetString("username", login.Username);
            }
            catch (Exception ex)
            {

            }
        }
        public void ClearSession()
        {
            _httpContextAccessor.HttpContext.Session.Clear();
        }


    }
}
