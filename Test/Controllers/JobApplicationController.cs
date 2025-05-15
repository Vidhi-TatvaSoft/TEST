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
            TempData["ErrorMessage"] = "Please Upload Resume";
            return RedirectToAction("JobApply", new { JobId = jobApplicationViewModel.JobId });
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
                    TempData["ErrorMessage"] = "Somehing went wrong";
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
                TempData["ErrorMessage"] = "Please Upload file in PDF formate.";
                return RedirectToAction("JobApply", new { JobId = jobApplicationViewModel.JobId });
            }

        }
    }

    #endregion

    #region GetAllJobApplications
    [Authorize(Roles = "Admin")]
    public IActionResult GetJobApplicationList()
    {
        if (Request.Cookies["AuthToken"] == null)
        {
            Response.Cookies.Delete("Remember");
            return RedirectToAction("LoginPage", "Login");
        }
        List<JobApplicationViewModel> AllJobApplication = _JobApplicationService.GetAllJobApplications();
        return View(AllJobApplication);
    }
    #endregion

    #region UpdateJobApplication
    [Authorize(Roles = "Admin")]
    public IActionResult UpdateJobApplication(int id)
    {
        if (Request.Cookies["AuthToken"] == null)
        {
            Response.Cookies.Delete("Remember");
            return RedirectToAction("LoginPage", "Login");
        }
        JobApplicationViewModel jobApplication = _JobApplicationService.GetJobDetail(id);
        if (jobApplication == null)
        {
            TempData["ErrorMessage"] = "Something Went Wrong";
            return RedirectToAction("GetJobApplicationList");
        }
        return View(jobApplication);
    }
    #endregion

    #region  UpdateJobApplication
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> UpdateJobApplication(JobApplicationViewModel jobApplicationViewModel)
    {
        if (Request.Cookies["AuthToken"] == null)
        {
            Response.Cookies.Delete("Remember");
            return RedirectToAction("LoginPage", "Login");
        }
        bool updateJobApplication = await _JobApplicationService.UpdateJobApplication(jobApplicationViewModel);
        if (updateJobApplication == false)
        {
            TempData["ErrorMessage"] = "Something Went Wrong";
            return RedirectToAction("GetJobApplicationList");
        }
        else
        {
            TempData["SuccessMessage"] = "Job Application Updated Successfully";
            return RedirectToAction("GetJobApplicationList");
        }
    }
    #endregion

    #region DeleteJobApplication
    public async Task<IActionResult> DeleteJobApplication(int AppId)
    {
        if (AppId == 0)
        {
            TempData["ErrorMessage"] = "Something Went wrong";
            return RedirectToAction("GetJobApplicationList");
        }
        else
        {
            bool jobApplicationDeleteStatus = await _JobApplicationService.DeleteJobApplication(AppId);
            if (jobApplicationDeleteStatus)
            {
                TempData["SuccessmMessage"] = "Job Application deleted successfully";
                return RedirectToAction("GetJobApplicationList");
            }
            else
            {
                TempData["ErrorMessage"] = "Something went wrong";
                return RedirectToAction("GetJobApplicationList");
            }
        }
    }
    #endregion

    #region UpdateJobApplication
    [Authorize(Roles = "Admin")]
    public IActionResult UpdateJobApplicationForJob(int id)
    {
        if (Request.Cookies["AuthToken"] == null)
        {
            Response.Cookies.Delete("Remember");
            return RedirectToAction("LoginPage", "Login");
        }
        JobApplicationViewModel jobApplication = _JobApplicationService.GetJobDetail(id);
        if (jobApplication == null)
        {
            TempData["ErrorMessage"] = "Something Went Wrong";
            return RedirectToAction("GetJobApplicationsByJobId", new { JobId = jobApplication.JobId });
        }
        return View(jobApplication);
    }
    #endregion

    #region  UpdateJobApplication
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> UpdateJobApplicationForJob(JobApplicationViewModel jobApplicationViewModel)
    {
        if (Request.Cookies["AuthToken"] == null)
        {
            Response.Cookies.Delete("Remember");
            return RedirectToAction("LoginPage", "Login");
        }
        bool updateJobApplication = await _JobApplicationService.UpdateJobApplication(jobApplicationViewModel);
        if (updateJobApplication == false)
        {
            TempData["ErrorMessage"] = "Something Went Wrong";
            return RedirectToAction("GetJobApplicationsByJobId", new { JobId = jobApplicationViewModel.JobId });
        }
        else
        {
            TempData["SuccessMessage"] = "Job Application Updated Successfully";
            return RedirectToAction("GetJobApplicationsByJobId", new { JobId = jobApplicationViewModel.JobId });
        }
    }
    #endregion

    #region DeleteJobApplication
    public async Task<IActionResult> DeleteJobApplicationforJob(int AppId)
    {
        if (AppId == 0)
        {
            TempData["ErrorMessage"] = "Something Went wrong";
            return RedirectToAction("Index", "Home");
        }
        else
        {
            int jobId = _JobApplicationService.getJobIdByAppId(AppId);
            bool jobApplicationDeleteStatus = await _JobApplicationService.DeleteJobApplication(AppId);
            if (jobApplicationDeleteStatus)
            {
                TempData["SuccessmMessage"] = "Job Application deleted successfully";
                return RedirectToAction("GetJobApplicationsByJobId", new { JobId = jobId });
            }
            else
            {
                TempData["ErrorMessage"] = "Something went wrong";
                return RedirectToAction("GetJobApplicationsByJobId", new { JobId = jobId });
            }
        }
    }
    #endregion



}
