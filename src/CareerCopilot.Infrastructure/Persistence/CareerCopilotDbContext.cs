using CareerCopilot.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CareerCopilot.Infrastructure.Persistence;

public sealed class CareerCopilotDbContext : DbContext
{
    public CareerCopilotDbContext(
        DbContextOptions<CareerCopilotDbContext> options)
        : base(options)
    {
    }

    public DbSet<AnalysisHistory> AnalysisHistories => Set<AnalysisHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AnalysisHistory>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.JobDescription)
                .IsRequired();

            entity.Property(x => x.ResumeText)
                .IsRequired();

            entity.Property(x => x.MatchScore);

            entity.Property(x => x.JobAnalysisJson)
                .IsRequired();

            entity.Property(x => x.ResumeMatchJson)
                .IsRequired();
        });
    }
}