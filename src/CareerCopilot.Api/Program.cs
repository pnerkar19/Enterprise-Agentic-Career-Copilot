using CareerCopilot.Agents;
using CareerCopilot.Application.Abstractions;
using CareerCopilot.Application.UseCases.AnalyzeCareerFit;
using CareerCopilot.Domain.Models;
using CareerCopilot.Infrastructure.Ai;
using CareerCopilot.Agents.Orchestration;
using CareerCopilot.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using CareerCopilot.Application.Abstractions.Documents;
using CareerCopilot.Infrastructure.Documents;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddSingleton<IAiProvider, FakeAiProvider>();
builder.Services.AddSingleton<IAiProvider, OpenAiProvider>();
builder.Services.AddScoped<IAgent<JobDescriptionAnalyzerInput, JobDescriptionAnalysis>, JobDescriptionAnalyzerAgent>();
builder.Services.AddScoped<IAgent<ResumeMatchInput, ResumeMatchResult>,ResumeMatchAgent>();
builder.Services.AddScoped<CareerFitOrchestrator>();
builder.Services.AddScoped<IDocumentTextExtractor, SimpleDocumentTextExtractor>();

builder.Services.AddDbContext<CareerCopilotDbContext>(options =>
{
    options.UseSqlite("Data Source=career_copilot.db");
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/api/agents/analyze-job-description",
    async (
        JobDescriptionAnalyzerInput input,
        IAgent<JobDescriptionAnalyzerInput, JobDescriptionAnalysis> agent,
        CancellationToken cancellationToken) =>
    {
        var result = await agent.ExecuteAsync(input, cancellationToken);
        return Results.Ok(result);
    })
.WithName("AnalyzeJobDescription")
.WithOpenApi();

app.MapPost("/api/agents/resume-match",
    async (
        ResumeMatchInput input,
        IAgent<ResumeMatchInput, ResumeMatchResult> agent,
        CancellationToken cancellationToken) =>
    {
        var result = await agent.ExecuteAsync(input, cancellationToken);
        return Results.Ok(result);
    })
    .WithName("ResumeMatch")
    .WithOpenApi();

app.MapPost("/api/agents/analyze-career-fit",
    async (
        CareerFitRequest request,
        CareerFitOrchestrator orchestrator,
        CancellationToken cancellationToken) =>
    {
        var result = await orchestrator.AnalyzeAsync(request, cancellationToken);
        return Results.Ok(result);
    })
.WithName("AnalyzeCareerFit")
.WithOpenApi();

app.MapGet("/api/history",
    async (
        CareerCopilotDbContext dbContext,
        CancellationToken cancellationToken) =>
    {
        var history = await dbContext.AnalysisHistories
            .OrderByDescending(x => x.CreatedUtc)
            .Select(x => new
            {
                x.Id,
                x.MatchScore,
                x.CreatedUtc,
                JobDescriptionPreview = x.JobDescription.Length > 200
                    ? x.JobDescription.Substring(0, 200) + "..."
                    : x.JobDescription,
                ResumePreview = x.ResumeText.Length > 200
                    ? x.ResumeText.Substring(0, 200) + "..."
                    : x.ResumeText
            })
            .ToListAsync(cancellationToken);

        return Results.Ok(history);
    })
.WithName("GetAnalysisHistory")
.WithOpenApi();


app.MapPost("/api/documents/extract-text",
    async (
        IFormFile file,
        IDocumentTextExtractor extractor,
        CancellationToken cancellationToken) =>
    {
        if (file.Length == 0)
        {
            return Results.BadRequest("File is empty.");
        }

        await using var stream = file.OpenReadStream();

        var text = await extractor.ExtractTextAsync(
            stream,
            file.FileName,
            cancellationToken);

        return Results.Ok(new
        {
            file.FileName,
            file.Length,
            ExtractedText = text
        });
    })
.Accepts<IFormFile>("multipart/form-data")
.WithName("ExtractDocumentText")
.WithOpenApi();


app.Run();