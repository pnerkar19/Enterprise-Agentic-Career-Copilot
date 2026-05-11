namespace CareerCopilot.Domain.Entities;

public sealed class AnalysisHistory
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string JobDescription { get; set; } = string.Empty;

    public string ResumeText { get; set; } = string.Empty;

    public int MatchScore { get; set; }

    public string JobAnalysisJson { get; set; } = string.Empty;

    public string ResumeMatchJson { get; set; } = string.Empty;

    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
}