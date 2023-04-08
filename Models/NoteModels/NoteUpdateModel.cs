using NotesApp.API.Database.Entities;

namespace NotesApp.API.Models.NoteModels
{
    public class NoteUpdateModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
