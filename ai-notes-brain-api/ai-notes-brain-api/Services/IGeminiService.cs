namespace ai_notes_brain_api.Services
{
    public interface IGeminiService
    {
        Task<string> BrainstormNoteAsync(string noteContent, string promptInstruction);
    }
}
