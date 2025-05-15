using DAL.ViewModels;

namespace BLL.Interfaces;

public interface IHomeService
{
    public List<JobViewModel> GetAllJobs(string role, long userId);

    Task<bool> AddJob(JobViewModel jobViewModel);
    public JobViewModel GetJobDetais(int id);
    Task<bool> UpdateJob(JobViewModel jobViewModel);
    Task<bool> DeleteJob(int id);

    public long GetUserIdFromEmail(string email);


}
