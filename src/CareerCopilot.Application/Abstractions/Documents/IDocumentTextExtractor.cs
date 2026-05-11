namespace CareerCopilot.Application.Abstractions.Documents;

public interface IDocumentTextExtractor
{
    Task<string> ExtractTextAsync(
        Stream fileStream,
        string fileName,
        CancellationToken cancellationToken);
}