using ai_notes_brain_api.Models;
using ai_notes_brain_api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ai_notes_brain_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesAIController : ControllerBase
    {
        private readonly IGeminiService _geminiService;
        private readonly NotesService _notesService;
        private readonly HistoryService _historyService;
        public NotesAIController(IGeminiService geminiService, NotesService notesService, HistoryService historyService)
        {
            _geminiService = geminiService;
            _notesService = notesService;
            _historyService = historyService;
        }

        [HttpGet("History")]
        public async Task<ActionResult<List<AskAIEntry>>> GetHistory()
        {
            var history = await _historyService.GetHistory();
            return Ok(history);
        }


        [HttpPost("brainstorm")]
        public async Task<ActionResult<AskResponse>> BrainstormNote(AskRequest request)
        {
            if (string.IsNullOrEmpty(request.Question))
                return BadRequest("Question can't be Empty.");


            try
            {
                var notes = await _notesService.GetNotes();
                var notesContent = string.Join("\n", notes.Select(n => $"{n.Title}: {n.Content}"));
                var aiResult = await _geminiService.BrainstormNoteAsync(notesContent, request.Question);
                return Ok(new AskResponse(aiResult));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server error: {ex.Message}");
            }

        }
    }
}
