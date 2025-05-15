using System.Threading.Tasks;
using BLL.Interfaces;
using DAL.Models;
using DAL.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services;

public class HomeService : IHomeService
{
    private readonly TestDatabaseContext _context;

    public HomeService(TestDatabaseContext context)
    {
        _context = context;

    }

    public List<JobViewModel> GetAllJobs(string role, long userId)
    {
        try
        {
            if(role == "User"){
                List<int> jobids = _context.JobApplications.Where(x => !x.IsDelete && x.UserId==userId).Select(x => x.JobId).ToList();
                var jobs = _context.Jobs.Include(x => x.JobApplications).Where(j => !j.IsDelete && j.Status == "Available" && (!jobids.Contains(j.JobId) || jobids.Contains(j.JobId)))
                .Select(j => new JobViewModel{
                    JobId = j.JobId,
                    JobName = j.JobName,
                    CompanyName = j.CompanyName,
                    Location = j.Location,
                    NoOfAplicants = j.JobApplications.Where(ja => ja.JobId == j.JobId && !ja.IsDelete).Count(),
                    status = jobids.Contains(j.JobId) ? "Applied" : "Available"
                }).OrderBy(x => x.JobId).ToList();
                return jobs;
            }else{
                return  _context.Jobs.Include(x => x.JobApplications).Where(j => !j.IsDelete)
            .Select(j => new JobViewModel{
                JobId = j.JobId,
                JobName = j.JobName,
                CompanyName = j.CompanyName,
                Location = j.Location,
                NoOfAplicants = j.JobApplications.Where(ja => ja.JobId == j.JobId && !ja.IsDelete).Count(),
                status = j.Status
            }).OrderBy(x => x.JobId).ToList();
            }
        }
        catch (Exception e)
        {
            return null!;
        }
    }
    

    
    #region AddJob
    public async Task<bool> AddJob(JobViewModel jobViewModel)
    {
        try
        {
            var job = new Jobs()
            {
                JobName = jobViewModel.JobName,
                CompanyName = jobViewModel.CompanyName,
                JobDescription = jobViewModel.JobDescription,
                Location = jobViewModel.Location,
                NoOfAplicants = 0,
                Status = "Available",
                IsDelete = false
            };
            await _context.Jobs.AddAsync(job);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
    #endregion

    #region GetJobDetais
    public JobViewModel GetJobDetais(int id){
        try
        {
            if(id==0){
                return null!;
            }
            var job = _context.Jobs.Include(x => x.JobApplications).FirstOrDefault(j => j.JobId == id && !j.IsDelete);
            if (job != null)
            {
                return new JobViewModel()
                {
                    JobId = job.JobId,
                    JobName = job.JobName,
                    CompanyName = job.CompanyName,
                    JobDescription = job.JobDescription == null || job.JobDescription == ""? "Not Available": job.JobDescription,
                    Location = job.Location,
                    NoOfAplicants = job.JobApplications.Where(ja => ja.JobId == job.JobId).Count(),
                    status = job.Status
                };
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

    #region UpdateJob
    public async Task<bool> UpdateJob(JobViewModel jobViewModel)
    {
        try
        {
            var job = _context.Jobs.FirstOrDefault(j => j.JobId == jobViewModel.JobId && !j.IsDelete);
            if (job != null)
            {
                job.JobName = jobViewModel.JobName;
                job.CompanyName = jobViewModel.CompanyName;
                job.JobDescription = jobViewModel.JobDescription;
                job.Location = jobViewModel.Location;
                job.Status = jobViewModel.status;
                _context.Update(job);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception e)
        {
            return false;
        }
    }
    #endregion

    #region DeleteJob
    public async Task<bool> DeleteJob(int id)
    {
        try
        {
            var job = _context.Jobs.FirstOrDefault(j => j.JobId == id && !j.IsDelete);
            if (job != null)
            {
                job.IsDelete = true;
                _context.Update(job);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception e)
        {
            return false;
        }
    }
    #endregion

    #region GetUserIdFromEmail
    public long GetUserIdFromEmail(string email){
        try
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email );
            if (user != null)
            {
                return user.UserId;
            }
            else
            {
                return 0;
            }
        }
        catch (Exception e)
        {
            return 0;
        }
    }
    #endregion
}
