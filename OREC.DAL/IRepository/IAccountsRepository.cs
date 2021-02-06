using OREC.BL.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace OREC.DAL.IRepository
{
    public interface IAccountsRepository
    {
        AccountsDTO GetUserOnLogin(LoginDTO login);
        RegisterDTO RegisterUser(RegisterDTO register);
    }
}
