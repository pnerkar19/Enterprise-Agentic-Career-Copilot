using System.Text.Json;
using CareerCopilot.Application.Abstractions;
using CareerCopilot.Domain.Models;

namespace CareerCopilot.Agents;

public sealed class JobDescriptionAnalyzerAgent 
    : IAgent<JobDescriptionAnalyzerInput, JobDescriptionAnalysis>
{
    private readonly IAiProvider _aiProvider;

    public JobDescriptionAnalyzerAgent(IAiProvider aiProvider)
    {
        _aiProvider = aiProvider;
    }

    public async Task<JobDescriptionAnalysis> ExecuteAsync(
        JobDescriptionAnalyzerInput input,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(input.JobDescription))
        {
            throw new ArgumentException("Job description cannot be empty.");
        }

        const string systemPrompt = """
        You are an expert technical recruiter and software engineering career coach.

        Analyze the job description and return ONLY valid JSON.
        Do not include markdown.
        Do not include explanations outside JSON.

        JSON schema:
        {
          "roleTitle": "",
          "seniorityLevel": "",
          "requiredSkills": [],
          "preferredSkills": [],
          "responsibilities": [],
          "keywords": [],
          "summary": ""
        }
        """;

        var userPrompt = $"""
        Analyze this job description:

        {input.JobDescription}
        """;

        var response = await _aiProvider.GenerateAsync(
            systemPrompt,
            userPrompt,
            cancellationToken);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var analysis = JsonSerializer.Deserialize<JobDescriptionAnalysis>(
            response,
            options);

        return analysis ?? throw new InvalidOperationException(
            "AI response could not be parsed into JobDescriptionAnalysis.");
    }
}