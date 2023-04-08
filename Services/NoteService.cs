using NotesApp.API.Interfaces;
using NotesApp.API.Models.NoteModels;
using NotesApp.API.Models;
using NotesApp.API.Database.Context;
using Microsoft.EntityFrameworkCore;
using NotesApp.API.Database.Entities;
using NotesApp.API.Models.UserModels;

namespace NotesApp.API.Services
{
    public class NoteService : INoteService
    {
        private readonly ILogger<NoteService> _logger;
        private readonly NotesAppContext db;
        private readonly IAuthorizedUserService _authorizedUserService;

        public NoteService(ILogger<NoteService> logger, NotesAppContext db, IAuthorizedUserService authorizedUserService)
        {
            _logger = logger;
            this.db = db;
            _authorizedUserService = authorizedUserService;
        }

        public async Task<CustomResponseModel<IEnumerable<NoteViewModel>>> GetAll()
        {
            try
            {
                if (!_authorizedUserService.IsUserAuthorized())
                    return new CustomResponseModel<IEnumerable<NoteViewModel>>()
                    {
                        StatusCode = 401,
                        ErrorMessage = "User not authorized"
                    };

                var notes = await db.Notes.Where(x => x.UserId == _authorizedUserService.GetUser().Id).ToArrayAsync();

                var noteViewModels = new List<NoteViewModel>();

                foreach (var note in notes)
                {
                    noteViewModels.Add(new NoteViewModel()
                    {
                        Id = note.Id,
                        User = new UserViewModel()
                        {
                            Id = _authorizedUserService.GetUser().Id,
                            Username = _authorizedUserService.GetUser().Username,
                            Email = _authorizedUserService.GetUser().Email,
                            Password = _authorizedUserService.GetUser().Password,
                            FailedLoginAttempts = _authorizedUserService.GetUser().FailedLoginAttempts
                        },
                        Title = note.Title,
                        Content = note.Content
                    });
                }

                return new CustomResponseModel<IEnumerable<NoteViewModel>>()
                {
                    StatusCode = 200,
                    Result = noteViewModels
                };
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);

                return new CustomResponseModel<IEnumerable<NoteViewModel>>()
                {
                    StatusCode = 400,
                    ErrorMessage = "Something went wrong"
                };
            }
        }

        public async Task<CustomResponseModel<NoteViewModel>> Get(int id)
        {
            try
            {
                if (!_authorizedUserService.IsUserAuthorized())
                    return new CustomResponseModel<NoteViewModel>()
                    {
                        StatusCode = 401,
                        ErrorMessage = "User not authorized"
                    };

                var note = await db.Notes.FirstOrDefaultAsync(x => x.Id == id);

                if (note == null || note.UserId != _authorizedUserService.GetUser().Id)
                    return new CustomResponseModel<NoteViewModel>()
                    {
                        StatusCode = 404,
                        ErrorMessage = "Not Found"
                    };

                var noteViewModel = new NoteViewModel()
                {
                    Id = id,
                    User = new UserViewModel()
                    {
                        Id = _authorizedUserService.GetUser().Id,
                        Username = _authorizedUserService.GetUser().Username,
                        Email = _authorizedUserService.GetUser().Email,
                        Password = _authorizedUserService.GetUser().Password,
                        FailedLoginAttempts = _authorizedUserService.GetUser().FailedLoginAttempts
                    },
                    Title = note.Title,
                    Content = note.Content
                };

                return new CustomResponseModel<NoteViewModel>()
                {
                    StatusCode = 200,
                    Result = noteViewModel
                };
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);

                return new CustomResponseModel<NoteViewModel>()
                {
                    StatusCode = 400,
                    ErrorMessage = "Something went wrong"
                };
            }
        }

        public async Task<CustomResponseModel<bool>> Create(NoteCreateModel noteCreateModel)
        {
            try
            {
                if (!_authorizedUserService.IsUserAuthorized())
                    return new CustomResponseModel<bool>()
                    {
                        StatusCode = 401,
                        ErrorMessage = "User not authorized",
                        Result = false
                    };

                var note = new Note()
                {
                    UserId = _authorizedUserService.GetUser().Id,
                    Title = noteCreateModel.Title,
                    Content = noteCreateModel.Content
                };

                await db.Notes.AddAsync(note);
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

        public async Task<CustomResponseModel<bool>> Update(NoteUpdateModel noteUpdateModel)
        {
            try
            {
                if (!_authorizedUserService.IsUserAuthorized())
                    return new CustomResponseModel<bool>()
                    {
                        StatusCode = 401,
                        ErrorMessage = "User not authorized",
                        Result = false
                    };

                var note = await db.Notes.FirstOrDefaultAsync(x => x.Id == noteUpdateModel.Id);

                if (note == null || note.UserId != _authorizedUserService.GetUser().Id)
                    return new CustomResponseModel<bool>()
                    {
                        StatusCode = 404,
                        ErrorMessage = "Not Found"
                    };

                note.Title = noteUpdateModel.Title;
                note.Content = noteUpdateModel.Content;

                db.Notes.Update(note);
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

        public async Task<CustomResponseModel<NoteViewModel>> Delete(int id)
        {
            try
            {
                if (!_authorizedUserService.IsUserAuthorized())
                    return new CustomResponseModel<NoteViewModel>()
                    {
                        StatusCode = 401,
                        ErrorMessage = "User not authorized"
                    };

                var note = await db.Notes.FirstOrDefaultAsync(x => x.Id == id);

                if (note == null || note.UserId != _authorizedUserService.GetUser().Id)
                    return new CustomResponseModel<NoteViewModel>()
                    {
                        StatusCode = 404,
                        ErrorMessage = "Not Found"
                    };

                db.Notes.Remove(note);
                await db.SaveChangesAsync();

                var noteViewModel = new NoteViewModel()
                {
                    Id = note.Id,
                    User = new UserViewModel()
                    {
                        Id = _authorizedUserService.GetUser().Id,
                        Username = _authorizedUserService.GetUser().Username,
                        Email = _authorizedUserService.GetUser().Email,
                        Password = _authorizedUserService.GetUser().Password,
                        FailedLoginAttempts = _authorizedUserService.GetUser().FailedLoginAttempts
                    },
                    Title = note.Title,
                    Content = note.Content
                };

                return new CustomResponseModel<NoteViewModel>()
                {
                    StatusCode = 200,
                    Result = noteViewModel
                };
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);

                return new CustomResponseModel<NoteViewModel>()
                {
                    StatusCode = 400,
                    ErrorMessage = "Something went wrong"
                };
            }
        }
    }
}
