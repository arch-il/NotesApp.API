using Microsoft.AspNetCore.Mvc;
using NotesApp.API.Interfaces;
using NotesApp.API.Models.UserModels;
using NotesApp.API.Models;

namespace NotesApp.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            this._userService = userService;
        }

        [HttpPost("[action]")]
        public async Task<CustomResponseModel<bool>> Login([FromQuery]UserLogInModel userLogInModel) => await _userService.Login(userLogInModel);

        [HttpPost("[action]")]
        public async Task<CustomResponseModel<bool>> Register([FromQuery] UserRegisterModel userRegisterModel) => await _userService.Register(userRegisterModel);


    }
}
