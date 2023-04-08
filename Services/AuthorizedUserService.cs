using NotesApp.API.Database.Entities;
using NotesApp.API.Interfaces;

namespace NotesApp.API.Services
{
    public class AuthorizedUserService : IAuthorizedUserService
    {
        private User User = null;
        public User GetUser() => User;
        public void SetUser(User user) => this.User = user; 
        public bool IsUserAuthorized() => this.User != null;
    }
}
