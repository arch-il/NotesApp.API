using Microsoft.AspNetCore.Mvc;
using NotesApp.API.Interfaces;
using NotesApp.API.Models.NoteModels;
using NotesApp.API.Models;

namespace NotesApp.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NoteController(INoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpGet("[action]")]
        public async Task<CustomResponseModel<IEnumerable<NoteViewModel>>> GetAll() => await _noteService.GetAll();

        [HttpGet("[action]")]
        public async Task<CustomResponseModel<NoteViewModel>> Get(int id) => await _noteService.Get(id);

        [HttpPost("[action]")]
        public async Task<CustomResponseModel<bool>> Create([FromQuery] NoteCreateModel noteCreateModel) => await _noteService.Create(noteCreateModel);

        [HttpPut("[action]")]
        public async Task<CustomResponseModel<bool>> Update([FromQuery] NoteUpdateModel noteUpdateModel) => await _noteService.Update(noteUpdateModel);

        [HttpDelete("[action]")]
        public async Task<CustomResponseModel<NoteViewModel>> Delete(int id) => await _noteService.Delete(id);

    }
}
