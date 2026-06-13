namespace ai_notes_brain_api.Models
{
    public class AskAIEntry
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public DateTime AskedAt { get; set; } = DateTime.UtcNow;
    }
}
