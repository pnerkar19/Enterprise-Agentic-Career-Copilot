using System.Text.Json;
using CareerCopilot.Application.Abstractions;
using CareerCopilot.Domain.Models;

namespace CareerCopilot.Agents.ResumeMatch;

public sealed class ResumeMatchAgent 
    : IAgent<ResumeMatchInput, ResumeMatchResult>
{
    private readonly IAiProvider _aiProvider;

    public ResumeMatchAgent(IAiProvider aiProvider)
    {
        _aiProvider = aiProvider;
    }

    public async Task<ResumeMatchResult> ExecuteAsync(
        ResumeMatchInput input,
        CancellationToken cancellationToken)
    {
        const string systemPrompt = """
        You are an expert ATS system and career coach.

        Compare resume with job description.

        Return ONLY valid JSON.

        JSON schema:
        {
          "matchScore": 0,
          "matchedSkills": [],
          "missingSkills": [],
          "recommendedImprovements": [],
          "tailoredResumeBullets": [],
          "interviewQuestions": [],
          "summary": ""
        }
        """;

        var userPrompt = $"""
        Job Analysis:
        {JsonSerializer.Serialize(input.JobAnalysis)}

        Resume:
        {input.ResumeText}
        """;

        var response = await _aiProvider.GenerateAsync(
            systemPrompt,
            userPrompt,
            cancellationToken);

        var result = JsonSerializer.Deserialize<ResumeMatchResult>(
            response,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return result ?? new ResumeMatchResult();
    }
}