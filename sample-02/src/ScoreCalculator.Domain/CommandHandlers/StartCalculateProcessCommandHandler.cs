using MediatR;
using Microsoft.Extensions.Logging;
using ScoreCalculator.Domain.MessageBus;
using ScoreCalculator.Domain.Model.Commands;
using ScoreCalculator.Domain.Model.Entities;
using ScoreCalculator.Domain.Repository;

namespace ScoreCalculator.Domain.CommandHandlers;

public class StartCalculateProcessCommandHandler : IRequestHandler<StartCalculateProcess, Unit>
{
    private const long TotalCustomers = 10000;
    private readonly ILogger _logger;
    private readonly KafkaPublisherMessage _publisher;
    private readonly CalculateProcessRepository _repository;
    private readonly Random _random;

    public StartCalculateProcessCommandHandler(KafkaPublisherMessage publisher, CalculateProcessRepository repository, Logger<StartCalculateProcessCommandHandler> logger)
    {
        _logger = logger;
        _publisher = publisher;
        _repository = repository;
        _random = new Random();
    }

    public async Task<Unit> Handle(StartCalculateProcess request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handler from command {command} received", typeof(StartCalculateProcess));

        var calculateProcess = new CalculateProcess();
        _logger.LogInformation("Start Calculate Process with Id {id} for {total} customers ", 
            calculateProcess.Id, TotalCustomers);

        await _repository.SaveAsync(calculateProcess);

        for(long i = 0; i < TotalCustomers; i++)
        {
            var nextCommand = new CalculateScore(
                GenerateCustomerScore(),
                calculateProcess.Id,
                i == TotalCustomers - 1,
                request.SagaKey
            );
            await _publisher.Publish(nextCommand, default);
        }

        return Unit.Value;
    }

    private CustomerScore GenerateCustomerScore() => new(100 * _random.NextDouble());

}