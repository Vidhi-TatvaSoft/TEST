using DAL.ViewModels;

namespace BLL.Interfaces;

public interface IJobApplicationService
{
        public List<JobApplicationViewModel> GetJobApplicationsByJobId(int JobId);
}
