using System.Threading.Tasks;
using BLL.Interfaces;
using DAL.Models;
using DAL.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services;

public class JobApplicationService : IJobApplicationService
{

    private readonly TestDatabaseContext _context;

    public JobApplicationService(TestDatabaseContext context)
    {
        _context = context;

    }

    #region GetJobApplications
    public List<JobApplicationViewModel> GetJobApplicationsByJobId(int JobId){
        try
        {
            if(JobId==0){
                return null!;
            }
            var jobApplications = _context.JobApplications.Include(x => x.Jobs).Where(j => j.JobId == JobId && !j.IsDelete).ToList();
            if (jobApplications != null)
            {
                return jobApplications.Select(j => new JobApplicationViewModel()
                {
                    ApplicationId = j.JobAppliactionId,
                    JobId = j.JobId,
                    JobName = j.Jobs.JobName,
                    status = j.status,
                    Resume = j.Resume == null? null : j.Resume,
                    UserId = j.UserId,
                    UserName = _context.Users.FirstOrDefault(u => u.UserId == j.UserId ).Name
                }).ToList();
            }
            else
            {
                return null!;
            }
        }
        catch (Exception e)
        {
            return null!;
        }

    }
    #endregion

    #region  GetJobDetailByJobId
    public JobApplicationViewModel GetJobDetailByJobId(int JobId, long userId){
        return _context.Jobs.Where(x => x.JobId == JobId && !x.IsDelete).Select(x => new JobApplicationViewModel()
        {
            JobId = x.JobId,
            JobName = x.JobName,
            UserId = userId,
            UserName = _context.Users.FirstOrDefault(u => u.UserId == userId).Name,
        }).FirstOrDefault()!;
    }
    #endregion

    #region ApplyJob
    public async Task<bool> ApplyJob(JobApplicationViewModel jobApplicationViewModel){
        try
        {
            var jobApplication = new JobApplication()
            {
                JobId = jobApplicationViewModel.JobId,
                status = "Applied",
                Resume = jobApplicationViewModel.Resume,
                IsDelete = false,
                UserId = jobApplicationViewModel.UserId
            };
           await _context.JobApplications.AddAsync(jobApplication);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
    #endregion

}
