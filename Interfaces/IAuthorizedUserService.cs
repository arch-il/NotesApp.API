using NotesApp.API.Database.Entities;

namespace NotesApp.API.Interfaces
{
    public interface IAuthorizedUserService
    {
        bool IsUserAuthorized();
        void SetUser(User user);
        User GetUser();
    }
}
