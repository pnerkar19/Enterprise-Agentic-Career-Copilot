namespace CareerCopilot.Domain.Models;

public sealed class ResumeMatchResult
{
    public int MatchScore { get; set; }

    public List<string> MatchedSkills { get; set; } = [];
    public List<string> MissingSkills { get; set; } = [];
    public List<string> RecommendedImprovements { get; set; } = [];
    public List<string> TailoredResumeBullets { get; set; } = [];
    public List<string> InterviewQuestions { get; set; } = [];

    public string Summary { get; set; } = string.Empty;
}