using MediatR;
using Microsoft.Extensions.Logging;
using ScoreCalculator.Domain.Model.Commands;
using ScoreCalculator.Domain.Repository;

namespace ScoreCalculator.Domain.CommandHandlers;

public class CancelCalculateScoreCommandHandler : IRequestHandler<CancelCalculateScore, Unit>
{
    private readonly ILogger _logger;
    private readonly CalculateProcessRepository _repository;

    public CancelCalculateScoreCommandHandler(CalculateProcessRepository repository, Logger<CancelCalculateScoreCommandHandler> logger)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<Unit> Handle(CancelCalculateScore request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handler from command {command} received", typeof(CancelCalculateScore));
        
        var calculateProcess = await _repository.GetByIdAsync(request.ProcessId) ?? 
            throw new Exception("Calculate Process Not Found");
            
        calculateProcess.CancelProcess();
        await _repository.UpdateAsync(calculateProcess);

        return Unit.Value;
    }
}