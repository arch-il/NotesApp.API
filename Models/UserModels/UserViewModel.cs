namespace NotesApp.API.Models.UserModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int FailedLoginAttempts { get; set; }
    }
}
