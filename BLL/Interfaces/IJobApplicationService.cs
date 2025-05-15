using DAL.ViewModels;

namespace BLL.Interfaces;

public interface IJobApplicationService
{
        public List<JobApplicationViewModel> GetJobApplicationsByJobId(int JobId);
        
    public JobApplicationViewModel GetJobDetailByJobId(int JobId , long userId);

    Task<bool> ApplyJob(JobApplicationViewModel jobApplicationViewModel);

    public List<JobApplicationViewModel> GetAllJobApplications();
    public JobApplicationViewModel GetJobDetail(int id);
    Task<bool> UpdateJobApplication(JobApplicationViewModel jobApplicationViewModel);

    Task<bool> DeleteJobApplication(int AppId);
    public int getJobIdByAppId(int AppId);
}
