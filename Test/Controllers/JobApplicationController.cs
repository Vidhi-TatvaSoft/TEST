using System.Threading.Tasks;
using BLL.Interfaces;
using BLL.Services;
using DAL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Test.Controllers;

public class JobApplicationController : Controller
{
    private readonly IJobApplicationService _JobApplicationService;
    private readonly IJWTTokenService _jwttokenService;
    private readonly IHomeService _homeService;

    public JobApplicationController(IJobApplicationService jobApplicationService, IJWTTokenService jWTTokenService, IHomeService homeService)
    {
        _JobApplicationService = jobApplicationService;
        _jwttokenService = jWTTokenService;
        _homeService = homeService;

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

    #region JobApply
    public IActionResult JobApply(int JobId)
    {
        if (Request.Cookies["AuthToken"] == null)
        {
            Response.Cookies.Delete("Remember");
            return RedirectToAction("LoginPage", "Login");
        }
        string token = Request.Cookies["AuthToken"];
        var claims = _jwttokenService.GetClaimsFromToken(token);
        var email = _jwttokenService.GetClaimValue(token, "email");
        var userId = _homeService.GetUserIdFromEmail(email);
        ViewBag.JobId = JobId;
        ViewBag.UserId = userId;
        JobApplicationViewModel jobApplicationViewModel = _JobApplicationService.GetJobDetailByJobId(JobId, userId);
        return View(jobApplicationViewModel);
    }
    #endregion

    #region ApplyJob
    [HttpPost]
    public async Task<IActionResult> JobApply(JobApplicationViewModel jobApplicationViewModel)
    {
        if (Request.Cookies["AuthToken"] == null)
        {
            Response.Cookies.Delete("Remember");
            return RedirectToAction("LoginPage", "Login");
        }
        if (jobApplicationViewModel.ResumePdf == null)
        {
            ViewData["ApplyMessage"] = "Please Upload Resume";
            return View(jobApplicationViewModel);
        }
        else
        {
            var extension = jobApplicationViewModel.ResumePdf.FileName.Split(".");
            if (extension[extension.Length - 1] == "pdf")
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

                //create folder if not exist
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string fileName = $"{Guid.NewGuid()}_{jobApplicationViewModel.ResumePdf.FileName}";
                string fileNameWithPath = Path.Combine(path, fileName);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    jobApplicationViewModel.ResumePdf.CopyTo(stream);
                }
                jobApplicationViewModel.Resume = $"/uploads/{fileName}";


                bool saveJob = await _JobApplicationService.ApplyJob(jobApplicationViewModel);
                if (saveJob == false)
                {
                    ViewData["ApplyMessage"] = "Somehing went wrong";
                    return RedirectToAction("JobApply", new { JobId = jobApplicationViewModel.JobId });
                }
                else
                {
                    TempData["SuccessMessage"] = "Applied in Job Successfully";
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                TempData["ErrorMessage"]  = "Please Upload file in PDF formate.";
                return RedirectToAction("JobApply", new { JobId = jobApplicationViewModel.JobId });
            }

        }
    }

    #endregion
}
