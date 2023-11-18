using MediatR;
using Microsoft.Extensions.Logging;
using ScoreCalculator.Domain.Model.Commands;
using ScoreCalculator.Domain.Model.Enumerations;
using ScoreCalculator.Domain.Repository;

namespace ScoreCalculator.Domain.CommandHandlers;

public class CalculateScoreCommandHandler : IRequestHandler<CalculateScore, Unit>
{
    private readonly ILogger _logger;
    private readonly CalculateProcessRepository _repository;

    public CalculateScoreCommandHandler(CalculateProcessRepository repository, Logger<CalculateScoreCommandHandler> logger)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<Unit> Handle(CalculateScore request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handler from command {command} received", typeof(CalculateScore));

        var process = await _repository.GetByIdAsync(request.ProcessId) ??
            throw new Exception("Calculate Process Not Found");

        if(process.Status == ProcessStatus.CANCELLED)
        {
            _logger.LogInformation("Calculate Process has been cancelled. Customer {customerId}", request.CustomerScore.Id);
            return Unit.Value;
        }

        var score = GetScore(request.CustomerScore.Debts);

        _logger.LogInformation("Calculate Score to customer {}. Debts = {debts}, Score = {score}", 
            request.CustomerScore.Id, request.CustomerScore.Debts, score);

        return Unit.Value;
    }

    private static int GetScore(double debts) 
    {
        if(debts >= 10 && debts < 50) return 10;
        else if(debts >= 50 && debts < 80) return 20;
        else if(debts >= 80) return 30;
        else return 0;
    }
        
}