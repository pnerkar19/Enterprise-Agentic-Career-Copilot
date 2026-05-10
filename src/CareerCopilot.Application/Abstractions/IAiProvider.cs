namespace CareerCopilot.Application.Abstractions;

public interface IAiProvider
{
    Task<string> GenerateAsync(
        string systemPrompt,
        string userPrompt,
        CancellationToken cancellationToken);
}