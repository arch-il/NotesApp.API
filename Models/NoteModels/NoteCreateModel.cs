using NotesApp.API.Database.Entities;

namespace NotesApp.API.Models.NoteModels
{
    public class NoteCreateModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
