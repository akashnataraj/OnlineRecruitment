using OREC.BL.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace OREC.DAL.IRepository
{
    public interface IJobsRepository
    {
        JobsDashboardDTO GetAllJobs(int userId, int roleId);
        JobsDTO GetJobbyId(int jobId, int roleId, int userId);
        JobsDTO AddJobs(JobsDTO job);
        JobsDTO UpdateJobs(JobsDTO job);
        StatusResponse DeleteJobs(int jobId, int userId);
        StatusResponse ApplyJob(int jobId, int userId, bool apply);
    }
}
