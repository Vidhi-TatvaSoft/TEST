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
    public List<JobApplicationViewModel> GetJobApplicationsByJobId(int JobId)
    {
        try
        {
            if (JobId == 0)
            {
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
                    Resume = j.Resume == null ? null : j.Resume,
                    UserId = j.UserId,
                    UserName = _context.Users.FirstOrDefault(u => u.UserId == j.UserId).Name
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
    public JobApplicationViewModel GetJobDetailByJobId(int JobId, long userId)
    {
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
    public async Task<bool> ApplyJob(JobApplicationViewModel jobApplicationViewModel)
    {
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

    #region GetAllJobApplications
    public List<JobApplicationViewModel> GetAllJobApplications()
    {
        try
        {
            List<JobApplicationViewModel> JobApplicationList = _context.JobApplications.Include(x => x.Jobs).Include(x => x.Users)
                                .Where(ja => !ja.IsDelete)
                                .Select(j => new JobApplicationViewModel()
                                {
                                    ApplicationId = j.JobAppliactionId,
                                    JobId = j.JobId,
                                    JobName = j.Jobs.JobName,
                                    status = j.status,
                                    Resume = j.Resume,
                                    UserId = j.UserId,
                                    CompanyName = j.Jobs.CompanyName,
                                    Location = j.Jobs.Location,
                                    UserName = _context.Users.FirstOrDefault(u => u.UserId == j.UserId).Name
                                }).ToList();
            if (JobApplicationList != null)
            {
                return JobApplicationList;
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

    #region GetJobDetail
    public JobApplicationViewModel GetJobDetail(int id)
    {
        try
        {
            return _context.JobApplications.Include(x => x.Jobs).Include(x => x.Users)
                        .Where(j => j.JobAppliactionId == id && !j.IsDelete)
                        .Select(j => new JobApplicationViewModel
                        {
                            ApplicationId = j.JobAppliactionId,
                            JobId = j.JobId,
                            JobName = j.Jobs.JobName,
                            status = j.status,
                            Resume = j.Resume,
                            UserId = j.UserId,
                            UserName = _context.Users.FirstOrDefault(u => u.UserId == j.UserId)!.Name,
                            CompanyName = j.Jobs.CompanyName,
                            Location = j.Jobs.Location
                        }).FirstOrDefault()!;
        }
        catch (Exception e)
        {
            return null!;
        }
    }
    #endregion


    #region UpdateJobApplication
    public async Task<bool> UpdateJobApplication(JobApplicationViewModel jobApplicationViewModel)
    {
        try
        {
            if (jobApplicationViewModel.ApplicationId == 0)
            {
                return false;
            }
            JobApplication? jobApplication = await _context.JobApplications.FirstOrDefaultAsync(ja => ja.JobAppliactionId == jobApplicationViewModel.ApplicationId && !ja.IsDelete);
            Jobs? job = await _context.Jobs.FirstOrDefaultAsync(j => !j.IsDelete && j.JobId == jobApplicationViewModel.JobId);
            if(jobApplication != null && job != null){
                jobApplication.JobId = jobApplicationViewModel.JobId;
                jobApplication.status = jobApplicationViewModel.status;
                _context.Update(jobApplication);

                job.CompanyName = jobApplicationViewModel.CompanyName;
                job.Location = jobApplicationViewModel.Location;
                job.JobName = jobApplicationViewModel.JobName;
                _context.Update(job);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }catch(Exception e){
            return false;
        }
    }
    #endregion

    #region DeleteJobApplication
    public async Task<bool> DeleteJobApplication(int AppId){
        try{
            JobApplication? jobApplication = await _context.JobApplications.FirstOrDefaultAsync(ja => ja.JobAppliactionId == AppId && !ja.IsDelete);
            if(jobApplication != null){
                jobApplication.IsDelete=true;
                _context.Update(jobApplication);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }catch(Exception e){
            return false;
        }
    }
    #endregion

    #region getJobIdByAppId
    public int getJobIdByAppId(int AppId){
        return _context.JobApplications.FirstOrDefault(ja => ja.JobAppliactionId == AppId && !ja.IsDelete).JobId;
    }
    #endregion

}
