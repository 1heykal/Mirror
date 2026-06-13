using ai_notes_brain_api.Data;
using ai_notes_brain_api.Models;
using Microsoft.EntityFrameworkCore;

namespace ai_notes_brain_api.Services
{
    public class NotesService
    {
        private readonly ApplicationDbContext dbContext;

        public NotesService(ApplicationDbContext context)
        {
            dbContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<Note>> GetNotes()
        {
            return await dbContext.Notes.ToListAsync();
        }

        public async Task<Note?> CreateNote(Note newNote)
        {
            var note = await dbContext.Notes.AddAsync(newNote);
            await dbContext.SaveChangesAsync();
            return note.Entity;
        }

        public async Task DeleteNote(int id)
        {
            var note = await dbContext.Notes.FindAsync(id);
            if (note != null)
            {
                dbContext.Notes.Remove(note);
                await dbContext.SaveChangesAsync();
            }

        }

        public async Task<List<Note>> SearchNotes(string term)
        {
            term = term.ToLower();
            return await dbContext.Notes.Where(n => n.Title.ToLower().Contains(term) || n.Content.ToLower().Contains(term))
                .ToListAsync();
        }

        public async Task<AskResponse> Ask(string question)
        {
            if (string.IsNullOrWhiteSpace(question))
                return new AskResponse("Please provide a valid question.");

            if (question.ToLower().Contains("how many notes do i have?") || question.ToLower().Contains("how many notes"))
                return new AskResponse($"You currently have {await dbContext.Notes.CountAsync()} notes.");

            if (question.ToLower().Contains("show me notes mentioning army."))
            {
                var notes = await dbContext.Notes.Where(n => n.Content.ToLower().Contains("army") || n.Title.ToLower().Contains("army")).ToListAsync();
                if (notes.Count != 0)
                    return new AskResponse($"Here are the notes mentioning 'army':\n{string.Join("\n", notes.Select(n => $"- {n.Title}: {n.Content}"))}");

                return new AskResponse("No notes mentioning 'army' were found.");
            }

            if (question.ToLower().Contains("what's my latest note?") || question.ToLower().Contains("latest note"))
            {
                var note = await dbContext.Notes.OrderBy(n => n.CreatedAt).LastOrDefaultAsync();
                if (note is null)
                    return new AskResponse("You don't have any notes");

                return new AskResponse($"Your latest note is: \\n - {note.Title}: {note.Content}");
            }

            return new AskResponse($"You currently have {await dbContext.Notes.CountAsync()} notes.");

        }
    }
}
