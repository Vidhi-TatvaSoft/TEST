using BLL.Interfaces;
using DAL.Models;
using DAL.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services;

public class LoginService : ILoginService
{
    private readonly TestDatabaseContext _context;

    private readonly IJWTTokenService _jWTTokenService;

    public LoginService(TestDatabaseContext context, IJWTTokenService jWTTokenService)
    {
        _context = context;
        _jWTTokenService = jWTTokenService;
    }

    public string VerifyPassword(LoginViwModel loginViwModel)
    {
        try
        {
            var user = _context.Users.FirstOrDefault(u => u.Email.ToLower().Trim() == loginViwModel.Email.ToLower().Trim());
            if (user == null)
            {
                return null!;
            }
            else
            {
                if (user.password == loginViwModel.Password)
                {
                    Roles Role = _context.Roles.FirstOrDefault(r => r.RoleId == user.RoleId)!;
                    string token = _jWTTokenService.GenerateToken(user.Email, Role.RoleName);
                    return token;
                }
                else
                {
                    return null!;
                }
            }
        }
        catch (Exception e)
        {
            return null!;
        }
    }
}
