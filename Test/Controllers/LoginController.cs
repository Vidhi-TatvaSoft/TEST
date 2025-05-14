using BLL.Interfaces;
using DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Test.Controllers;

public class LoginController : Controller
{
    private readonly ILoginService _loginService;

    public LoginController(ILoginService loginService)
    {
        _loginService = loginService;
    }

    public IActionResult LoginPage()
    {
        return View();
    }

    public IActionResult VerifyPassword(LoginViwModel loginViwModel)
    {
        if (loginViwModel.Email == null || loginViwModel.Password == null)
        {
            return View("LoginPage");
        }
        else
        {
            if (_loginService.VerifyPassword(loginViwModel))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View("LoginPage");
            }
        }
    }
}
