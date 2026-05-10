
using CareerCopilot.Domain.Models;
namespace CareerCopilot.Agents.Orchestration;

public sealed class CareerFitAnalysisResult
{
    public JobDescriptionAnalysis JobDescriptionAnalysis { get; set; } = new();
    public ResumeMatchResult ResumeMatchResult { get; set; } = new();
}