using CareerCopilot.Application.Abstractions;

namespace CareerCopilot.Infrastructure.Ai;

public sealed class FakeAiProvider : IAiProvider
{
    public Task<string> GenerateAsync(
        string systemPrompt,
        string userPrompt,
        CancellationToken cancellationToken)
    {
        var json = """
        {
           "matchScore": 72,
           "matchedSkills": ["C#", ".NET", "SQL Server"],
          "missingSkills": ["Azure", "Docker"],
          "recommendedImprovements": ["Add Azure experience", "Highlight API scalability"],
          "tailoredResumeBullets": ["Designed scalable .NET APIs handling high throughput"],
          "interviewQuestions": ["Explain async/await in .NET", "How do you scale APIs?"],
         "summary": "Good backend experience but missing cloud depth."
          }
        
        """;

        return Task.FromResult(json);
    }
}