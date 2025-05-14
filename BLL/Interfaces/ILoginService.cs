using DAL.ViewModels;

namespace BLL.Interfaces;

public interface ILoginService
{
     public bool VerifyPassword(LoginViwModel loginViwModel);
}
