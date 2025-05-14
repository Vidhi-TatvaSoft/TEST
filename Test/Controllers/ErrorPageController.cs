using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Test.Controllers;
[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]

public class ErrorPageController : Controller
{


    [Route("ErrorPage/HandleError/{statusCode}")]
    public IActionResult HandleError(int statusCode)
    {
        switch (statusCode)
        {
            case 403:
                return View("Forbidden");  //dn
            case 404:
                return View("PageNotFound"); //dn
            case 500:
                return View("InternalServerError");
            default:
                return View("Generic");
        }
    }
}
