using Google.GenAI;
using Microsoft.Extensions.Options;
namespace ai_notes_brain_api.Services
{
    public class GeminiService : IGeminiService
    {

        private readonly string _apiKey;
        private const string ModelName = "gemini-2.5-flash";
        private readonly HistoryService _historyService;

        public GeminiService(IConfiguration configuration, HistoryService historyService)
        {
            _apiKey = configuration["GeminiSettings:ApiKey"]
                ?? throw new ArgumentNullException("Gemini API Key is missing in configuration.");
            _historyService = historyService ?? throw new ArgumentNullException(nameof(historyService));
        }
        public async Task<string> BrainstormNoteAsync(string noteContent, string promptInstruction)
        {
            var client = new Google.GenAI.Client(apiKey: _apiKey);

            string fullPrompt = $"{promptInstruction}\n\nNote Content:\n{noteContent}";

            int maxRetries = 3;
            int delayMilliseconds = 2000;

            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    var response = await client.Models.GenerateContentAsync(ModelName, fullPrompt);
                    if (response.Text != null)
                    {
                        await _historyService.AddEntry(promptInstruction, response.Text);
                        return response.Text;
                    }

                    return "No response generated";

                }
                catch (Exception ex) when (ex.Message.Contains("high demand") || ex.Message.Contains("429") || ex.Message.Contains("503"))
                {
                    if (i == maxRetries - 1) throw;

                    await Task.Delay(delayMilliseconds * (i + 1));
                }
            }

            return "Server was too busy.";
        }
    }
}

