using ai_notes_brain_api.Models;
using ai_notes_brain_api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ai_notes_brain_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {

        private readonly NotesService notesService;

        public NotesController(NotesService service)
        {
            notesService = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet]
        public async Task<ActionResult<List<Note>>> GetNotes([FromQuery] string? search = null)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                var notes = await notesService.GetNotes();
                return Ok(notes);
            }
            else
            {
                var notes = await notesService.SearchNotes(search);
                return Ok(notes);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Note>> CreateNote(Note note)
        {
            var createdNote = await notesService.CreateNote(note);
            return CreatedAtAction(nameof(GetNotes), new { id = createdNote.Id }, createdNote);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteNote(int id)
        {
            await notesService.DeleteNote(id);
            return NoContent();
        }

        [HttpPost("Ask")]
        public async Task<ActionResult<AskResponse>> Ask(AskRequest request)
        {
            var answer = await notesService.Ask(request.Question);
            return Ok(answer);
        }
    }
}
