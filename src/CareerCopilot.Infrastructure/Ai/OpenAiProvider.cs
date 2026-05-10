using CareerCopilot.Application.Abstractions;
using OpenAI.Chat;

namespace CareerCopilot.Infrastructure.Ai;

public sealed class OpenAiProvider : IAiProvider
{
    private readonly ChatClient _chatClient;

    public OpenAiProvider()
    {
        var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY")
                     ?? throw new InvalidOperationException(
                         "OPENAI_API_KEY environment variable not found.");

        _chatClient = new ChatClient(
            model: "gpt-4.1-mini",
            apiKey: apiKey);
    }

    public async Task<string> GenerateAsync(
        string systemPrompt,
        string userPrompt,
        CancellationToken cancellationToken)
    {
        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(systemPrompt),
            new UserChatMessage(userPrompt)
        };

        var response = await _chatClient.CompleteChatAsync(
            messages,
            cancellationToken: cancellationToken);

        return response.Value.Content[0].Text;
    }
}