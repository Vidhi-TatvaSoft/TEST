using System.Diagnostics;
using System.Threading.Tasks;
using BLL.Interfaces;
using DAL.Models;
using DAL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Test.Models;

namespace Test.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly IHomeService _homeService;

    private readonly IJWTTokenService _jwttokenService;

    public HomeController(ILogger<HomeController> logger, IHomeService homeService, IJWTTokenService jWTTokenService)
    {
        _logger = logger;
        _homeService = homeService;
        _jwttokenService = jWTTokenService;
    }

    public IActionResult Index()
    {
        if (Request.Cookies["AuthToken"] == null)
        {
            Response.Cookies.Delete("Remember");
            return RedirectToAction("LoginPage", "Login");
        }
        string token = Request.Cookies["AuthToken"];
        var claims = _jwttokenService.GetClaimsFromToken(token);
        var role = _jwttokenService.GetClaimValue(token, "role");
        var email = _jwttokenService.GetClaimValue(token, "email");
        var userId = _homeService.GetUserIdFromEmail(email);

        List<JobViewModel> jobList = _homeService.GetAllJobs(role);
        return View(jobList);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Dashboard()
    {
        if (Request.Cookies["AuthToken"] == null)
        {
            Response.Cookies.Delete("Remember");
            return RedirectToAction("LoginPage", "Login");
        }
        return View();
    }

#region AddJob get
    [Authorize(Roles = "Admin")]
    public IActionResult AddJob()
    {
        if (Request.Cookies["AuthToken"] == null)
        {
            Response.Cookies.Delete("Remember");
            return RedirectToAction("LoginPage", "Login");
        }
        return View();
    }
#endregion

#region AddJob post
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult>  AddJob(JobViewModel jobViewModel){
        bool addJobStatus = await _homeService.AddJob(jobViewModel);
        if (addJobStatus == false)
        {
            ViewData["ErrorMessage"] = "Something went wroong. Try Again";
            return View();
        }
        else
        {
            TempData["SuccessMessage"] = "Job Added Successfully";
            return RedirectToAction("Index");
        }
    }
#endregion

#region UpdateJob
    [Authorize(Roles = "Admin")]
    public IActionResult UpdateJob(int id)
    {
        if (Request.Cookies["AuthToken"] == null)
        {
            Response.Cookies.Delete("Remember");
            return RedirectToAction("LoginPage", "Login");
        }
        JobViewModel job = _homeService.GetJobDetais(id);
        return View(job);
    }
#endregion

#region UpdateJob
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> UpdateJob(JobViewModel jobViewModel)
    {
        if (Request.Cookies["AuthToken"] == null)
        {
            Response.Cookies.Delete("Remember");
            return RedirectToAction("LoginPage", "Login");
        }
        bool UpdateJobStatus =await  _homeService.UpdateJob(jobViewModel);
        if (UpdateJobStatus == false)
        {
            ViewData["ErrorMessage"] = "Something went wroong. Try Again";
            return RedirectToAction("Index");
        }
        else
        {
            TempData["SuccessMessage"] = "Job Updated Successfully";
            return RedirectToAction("Index");
        }
    }
#endregion

#region DeleteJob
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteJob(int id)
    {
        if (Request.Cookies["AuthToken"] == null)
        {
            Response.Cookies.Delete("Remember");
            return RedirectToAction("LoginPage", "Login");
        }
        bool deleteJobStatus = await _homeService.DeleteJob(id);
        if (deleteJobStatus == false)
        {
            ViewData["ErrorMessage"] = "Something went wroong. Try Again";
            return RedirectToAction("Index");
        }
        else
        {
            TempData["SuccessMessage"] = "Job Deleted Successfully";
            return RedirectToAction("Index");
        }
        
    }
    #endregion


}
