namespace CareerCopilot.Domain.Models;

public sealed class JobDescriptionAnalysis
{
    public string RoleTitle { get; set; } = string.Empty;
    public string SeniorityLevel { get; set; } = string.Empty;

    public List<string> RequiredSkills { get; set; } = [];
    public List<string> PreferredSkills { get; set; } = [];
    public List<string> Responsibilities { get; set; } = [];
    public List<string> Keywords { get; set; } = [];

    public string Summary { get; set; } = string.Empty;
}