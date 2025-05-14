using BLL.Interfaces;
using DAL.Models;
using DAL.ViewModels;

namespace BLL.Services;

public class LoginService : ILoginService
{
    private readonly TestDatabaseContext _context;

    public LoginService(TestDatabaseContext context)
    {
        _context = context;
    }

    public bool VerifyPassword(LoginViwModel loginViwModel)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == loginViwModel.Email);
        if (user == null)
        {
            return false;
        }
        else
        {
            if (user.password == loginViwModel.Password)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
