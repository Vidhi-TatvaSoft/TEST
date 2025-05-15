using BLL.Interfaces;
using DAL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Test.Controllers;

public class LoginController : Controller
{
    private readonly ILoginService _loginService;

    public LoginController(ILoginService loginService)
    {
        _loginService = loginService;
    }

    // public IActionResult LoginPage()
    // {
    //     return View();
    // }

    public IActionResult LoginPage()
    {
        if (Request.Cookies["Remember"] != null)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

    public IActionResult Registration(){
        return View();
    }

    [HttpPost]
    public IActionResult Registration(RegistrationViewModel registrationViewModel)
    {
        if (registrationViewModel.Email == null || registrationViewModel.Password == null)
        {
            ViewData["RegisterMessage"] = "Enter Valid Credentials";
            return View();
        }
        else
        {
            bool saveUser = _loginService.SaveUser(registrationViewModel);
            if (saveUser == false)
            {
                ViewData["RegisterMessage"] = "Email Already Exists";
                return View();
            }
            else
            {
                TempData["SuccessMessage"] = "Registration Successful";
                return RedirectToAction("LoginPage");
            }
           
        }
    }

    [HttpPost]
    public IActionResult LoginPage(LoginViwModel loginViwModel)
    {
        // try
        // {
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddDays(10);
            if (loginViwModel.Email == null || loginViwModel.Password == null)
            {
                ViewData["LoginMessage"] = "Enter Valid Credentials";
                return View();
            }
            else
            {
                string token = _loginService.VerifyPassword(loginViwModel);
                if (token != null)
                {
                    if (loginViwModel.RememberMe)
                    {
                        Response.Cookies.Append("Remember", loginViwModel.Email);
                    }
                    Response.Cookies.Append("AuthToken", token, options);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewData["LoginMessage"] = "Enter Valid Credentials";
                    return View();
                }
            }
        // }
        // catch (Exception e)
        // {
        //     return View();
        // }
    }

    public IActionResult Logout()
    {
        // try
        // {
            Response.Cookies.Delete("AuthToken");
            Response.Cookies.Delete("Remember");
            return RedirectToAction("LoginPage");
        // }
        // catch (Exception e)
        // {
        //     return RedirectToAction("LoginPage");
        // }
    }


}
