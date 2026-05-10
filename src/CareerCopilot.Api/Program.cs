using CareerCopilot.Agents;
using CareerCopilot.Agents.ResumeMatch;
using CareerCopilot.Application.Abstractions;
using CareerCopilot.Domain.Models;
using CareerCopilot.Infrastructure.Ai;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IAiProvider, FakeAiProvider>();
builder.Services.AddScoped<IAgent<JobDescriptionAnalyzerInput, JobDescriptionAnalysis>, JobDescriptionAnalyzerAgent>();
builder.Services.AddScoped<IAgent<ResumeMatchInput, ResumeMatchResult>,ResumeMatchAgent>();

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


app.Run();