using ai_notes_brain_api.Data;
using ai_notes_brain_api.Models;
using Microsoft.EntityFrameworkCore;

namespace ai_notes_brain_api.Services
{
    public class HistoryService
    {
        private readonly ApplicationDbContext _dbContext;

        public HistoryService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<List<AskAIEntry>> GetHistory()
        {
            return await _dbContext.History.OrderByDescending(h => h.AskedAt).ToListAsync();
        }

        public async Task AddEntry(string question, string answer)
        {
            if (string.IsNullOrWhiteSpace(question))
                throw new ArgumentException("Question cannot be null or empty.", nameof(question));
            if (string.IsNullOrWhiteSpace(answer))
                throw new ArgumentException("Answer cannot be null or empty.", nameof(answer));
            var entry = new AskAIEntry
            {
                Question = question,
                Answer = answer,
                AskedAt = DateTime.UtcNow
            };
            _dbContext.History.Add(entry);
            await _dbContext.SaveChangesAsync();
        }
    }
}
