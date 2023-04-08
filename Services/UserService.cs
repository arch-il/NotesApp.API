using Microsoft.EntityFrameworkCore;
using NotesApp.API.Database.Context;
using NotesApp.API.Database.Entities;
using NotesApp.API.Interfaces;
using NotesApp.API.Models;
using NotesApp.API.Models.UserModels;

namespace NotesApp.API.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly NotesAppContext db;
        private readonly IAuthorizedUserService _authorizedUserService;

        public UserService(ILogger<UserService> logger, NotesAppContext db, IAuthorizedUserService authorizedUserService)
        {
            _logger = logger;
            this.db = db;
            _authorizedUserService = authorizedUserService;
        }

        public async Task<CustomResponseModel<bool>> Login(UserLogInModel userLogInModel)
        {
            try
            {
                if (userLogInModel == null)
                    return new CustomResponseModel<bool>()
                    {
                        StatusCode = 400,
                        ErrorMessage = "Invalid input",
                        Result = false
                    };

                var User = await db.Users.FirstOrDefaultAsync(x => x.Username == userLogInModel.UsernameOrEmail);

                if (User == null)
                {
                    User = await db.Users.FirstOrDefaultAsync(x => x.Email == userLogInModel.UsernameOrEmail);
                    if (User == null)
                        return new CustomResponseModel<bool>()
                        {
                            StatusCode = 404,
                            ErrorMessage = "Incorrect Email or Username",
                            Result = false
                        };
                }

                if (User.Password != userLogInModel.Password)
                {
                    User.FailedLoginAttempts++;

                    db.Users.Update(User);
                    await db.SaveChangesAsync();

                    if (User.FailedLoginAttempts >= 3)
                    {
                        // write email send logic here
                        // PS. Counldn't figure out how to send mail
                    }

                    return new CustomResponseModel<bool>()
                    {
                        StatusCode = 400,
                        ErrorMessage = "Incorrect password",
                        Result = false
                    };
                }

                _authorizedUserService.SetUser(User);

                return new CustomResponseModel<bool>()
                {
                    StatusCode = 200,
                    Result = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);

                return new CustomResponseModel<bool>()
                {
                    StatusCode = 400,
                    ErrorMessage = "Something went wrong",
                    Result = false
                };
            }
        }

        public async Task<CustomResponseModel<bool>> Register(UserRegisterModel userRegisterModel)
        {
            try
            {
                if (userRegisterModel == null)
                    return new CustomResponseModel<bool>()
                    {
                        StatusCode = 400,
                        ErrorMessage = "Invalid input",
                        Result = false
                    };

                if (await db.Users.FirstOrDefaultAsync(x => x.Username == userRegisterModel.Username) != null)
                    return new CustomResponseModel<bool>()
                    {
                        StatusCode = 400,
                        ErrorMessage = "Username already in use",
                        Result = false
                    };

                if (await db.Users.FirstOrDefaultAsync(x => x.Email == userRegisterModel.Email) != null)
                    return new CustomResponseModel<bool>()
                    {
                        StatusCode = 400,
                        ErrorMessage = "Email already in use",
                        Result = false
                    };

                var User = new User()
                {
                    Username = userRegisterModel.Username,
                    Email = userRegisterModel.Email,
                    Password = userRegisterModel.Password,
                    FailedLoginAttempts = 0
                };

                await db.Users.AddAsync(User);
                await db.SaveChangesAsync();

                return new CustomResponseModel<bool>()
                {
                    StatusCode = 200,
                    Result = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);

                return new CustomResponseModel<bool>()
                {
                    StatusCode = 400,
                    ErrorMessage = "Something went wrong",
                    Result = false
                };
            }
        }
    }
}
