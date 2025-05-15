using DAL.ViewModels;

namespace BLL.Interfaces;

public interface ILoginService
{
     public string VerifyPassword(LoginViwModel loginViwModel);

     bool SaveUser(RegistrationViewModel registrationViewModel);
}
