namespace CareerCopilot.Application.Abstractions;

public interface IAgent<TInput, TOutput>
{
    Task<TOutput> ExecuteAsync(TInput input, CancellationToken cancellationToken);
}