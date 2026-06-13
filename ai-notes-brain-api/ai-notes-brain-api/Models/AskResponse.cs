namespace ai_notes_brain_api.Models
{
    public class AskResponse
    {
        public string Answer { get; set; }

        public AskResponse(string answer)
        {
            Answer = answer;
        }
    }
}
