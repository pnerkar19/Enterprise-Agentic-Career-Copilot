using System.Text.Json;
using CareerCopilot.Application.Abstractions;
using CareerCopilot.Domain.Models;
using System.Text.Json.Serialization;
using System.Text.Json.Nodes;

namespace CareerCopilot.Agents
{
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
        
        Rules:
        - matchScore must be a number only.
        - Do not return matchScore as text.
        - Do not include %, /100, or quotes.
        - Correct: "matchScore": 85
        - Incorrect: "matchScore": "85%"

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

            var node = JsonNode.Parse(response)
            ?? throw new InvalidOperationException("AI response was not valid JSON.");

            var scoreText = node["matchScore"]?.ToString() ?? "0";

            // Handles "85", "85%", "85/100"
            var digits = new string(scoreText.TakeWhile(char.IsDigit).ToArray());

            if (!int.TryParse(digits, out var score))
            {
                score = 0;
            }

            score = Math.Clamp(score, 0, 100);

            node["matchScore"] = score;

            var result = node.Deserialize<ResumeMatchResult>(
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

return result ?? new ResumeMatchResult();

            return result ?? new ResumeMatchResult();
        }
    }
}