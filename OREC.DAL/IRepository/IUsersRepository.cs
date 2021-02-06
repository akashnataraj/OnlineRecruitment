using System;
using System.Collections.Generic;
using System.Text;
using OREC.BL.DTO;

namespace OREC.DAL.IRepository
{
    public interface IUsersRepository
    {
        UsersDTO GetUserDetails(int id);
        StatusResponse UpdateUser(UsersDTO user);
    }
}
