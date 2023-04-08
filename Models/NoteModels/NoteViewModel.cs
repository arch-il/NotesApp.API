using NotesApp.API.Database.Entities;
using NotesApp.API.Models.UserModels;

namespace NotesApp.API.Models.NoteModels
{
    public class NoteViewModel
    {
        public int Id { get; set; }
        public UserViewModel User { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
