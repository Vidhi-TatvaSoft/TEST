using DAL.ViewModels;

namespace BLL.Interfaces;

public interface IJobApplicationService
{
        public List<JobApplicationViewModel> GetJobApplicationsByJobId(int JobId);
        
    public JobApplicationViewModel GetJobDetailByJobId(int JobId , long userId);

    Task<bool> ApplyJob(JobApplicationViewModel jobApplicationViewModel);
}
