
using CareerCopilot.Application.Abstractions;
using CareerCopilot.Application.UseCases.AnalyzeCareerFit;
using CareerCopilot.Domain.Models;

namespace CareerCopilot.Agents.Orchestration;

public sealed class CareerFitOrchestrator
{
    private readonly IAgent<JobDescriptionAnalyzerInput, JobDescriptionAnalysis> _jdAnalyzerAgent;
    private readonly IAgent<ResumeMatchInput, ResumeMatchResult> _resumeMatchAgent;

    public CareerFitOrchestrator(
        IAgent<JobDescriptionAnalyzerInput, JobDescriptionAnalysis> jdAnalyzerAgent,
        IAgent<ResumeMatchInput, ResumeMatchResult> resumeMatchAgent)
    {
        _jdAnalyzerAgent = jdAnalyzerAgent;
        _resumeMatchAgent = resumeMatchAgent;
    }

    public async Task<CareerFitAnalysisResult> AnalyzeAsync(
        CareerFitRequest request,
        CancellationToken cancellationToken)
    {
        var jdAnalysis = await _jdAnalyzerAgent.ExecuteAsync(
            new JobDescriptionAnalyzerInput
            {
                JobDescription = request.JobDescription
            },
            cancellationToken);

        var resumeMatch = await _resumeMatchAgent.ExecuteAsync(
            new ResumeMatchInput
            {
                ResumeText = request.ResumeText,
                JobAnalysis = jdAnalysis
            },
            cancellationToken);

        return new CareerFitAnalysisResult
        {
            JobDescriptionAnalysis = jdAnalysis,
            ResumeMatchResult = resumeMatch
        };
    }
}
