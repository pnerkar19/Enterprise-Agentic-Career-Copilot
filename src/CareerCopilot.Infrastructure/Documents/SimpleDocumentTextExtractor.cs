using CareerCopilot.Application.Abstractions.Documents;

namespace CareerCopilot.Infrastructure.Documents;

public sealed class SimpleDocumentTextExtractor : IDocumentTextExtractor
{
    public async Task<string> ExtractTextAsync(
        Stream fileStream,
        string fileName,
        CancellationToken cancellationToken)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();

        if (extension != ".txt")
        {
            throw new NotSupportedException(
                "Only .txt files are supported right now. PDF and DOCX support will be added next.");
        }

        using var reader = new StreamReader(fileStream);
        return await reader.ReadToEndAsync(cancellationToken);
    }
}