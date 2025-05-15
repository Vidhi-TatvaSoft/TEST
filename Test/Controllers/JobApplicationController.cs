using BLL.Interfaces;
using BLL.Services;
using DAL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Test.Controllers;

public class JobApplicationController : Controller
{
    private readonly IJobApplicationService _JobApplicationService;

    public JobApplicationController(IJobApplicationService jobApplicationService)
    {
        _JobApplicationService = jobApplicationService;
    }


    #region GetJobApplications
    [Authorize(Roles = "Admin")]
    public IActionResult GetJobApplicationsByJobId(int JobId)
    {
        if (Request.Cookies["AuthToken"] == null)
        {
            Response.Cookies.Delete("Remember");
            return RedirectToAction("LoginPage", "Login");
        }
        List<JobApplicationViewModel> jobApplicationList = _JobApplicationService.GetJobApplicationsByJobId(JobId);
        return View(jobApplicationList);
    }
#endregion
}
