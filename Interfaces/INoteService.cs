using NotesApp.API.Models;
using NotesApp.API.Models.NoteModels;

namespace NotesApp.API.Interfaces
{
    public interface INoteService
    {
        Task<CustomResponseModel<IEnumerable<NoteViewModel>>> GetAll();
        Task<CustomResponseModel<NoteViewModel>> Get(int id);
        Task<CustomResponseModel<bool>> Create(NoteCreateModel noteCreateModel);
        Task<CustomResponseModel<bool>> Update(NoteUpdateModel noteUpdateModel);
        Task<CustomResponseModel<NoteViewModel>> Delete(int id);
    }
}
