using System;
using System.Collections.Generic;
using System.Text;

namespace OREC.BL.DTO
{
    public class StatusResponse
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public int Code { get; set; }
        public string Description { get; set; }
        public bool Success { get; set; }
    }
}
