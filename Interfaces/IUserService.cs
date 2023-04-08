using NotesApp.API.Models;
using NotesApp.API.Models.UserModels;

namespace NotesApp.API.Interfaces
{
    public interface IUserService
    {
        Task<CustomResponseModel<bool>> Login(UserLogInModel userLogInModel);
        Task<CustomResponseModel<bool>> Register(UserRegisterModel userRegisterModel);
    }
}
