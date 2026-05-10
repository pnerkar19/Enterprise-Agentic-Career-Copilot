using CareerCopilot.Domain.Models;

namespace CareerCopilot.Agents.ResumeMatch;

public sealed class ResumeMatchInput
{
    public string ResumeText { get; set; } = string.Empty;
    public JobDescriptionAnalysis JobAnalysis { get; set; } = new();
}