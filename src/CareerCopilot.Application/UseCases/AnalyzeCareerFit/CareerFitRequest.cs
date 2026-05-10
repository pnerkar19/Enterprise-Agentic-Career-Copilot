namespace CareerCopilot.Application.UseCases.AnalyzeCareerFit;

public sealed class CareerFitRequest
{
    public string JobDescription { get; set; } = string.Empty;
    public string ResumeText { get; set; } = string.Empty;
}