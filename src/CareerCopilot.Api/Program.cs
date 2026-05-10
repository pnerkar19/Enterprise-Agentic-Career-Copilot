using CareerCopilot.Agents;
using CareerCopilot.Application.Abstractions;
using CareerCopilot.Application.UseCases.AnalyzeCareerFit;
using CareerCopilot.Domain.Models;
using CareerCopilot.Infrastructure.Ai;
using CareerCopilot.Agents.Orchestration;

;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddSingleton<IAiProvider, FakeAiProvider>();
builder.Services.AddSingleton<IAiProvider, OpenAiProvider>();
builder.Services.AddScoped<IAgent<JobDescriptionAnalyzerInput, JobDescriptionAnalysis>, JobDescriptionAnalyzerAgent>();
builder.Services.AddScoped<IAgent<ResumeMatchInput, ResumeMatchResult>,ResumeMatchAgent>();
builder.Services.AddScoped<CareerFitOrchestrator>();

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


app.Run();