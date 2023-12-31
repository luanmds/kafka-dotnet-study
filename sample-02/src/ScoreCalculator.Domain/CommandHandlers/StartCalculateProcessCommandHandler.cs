using MediatR;
using Microsoft.Extensions.Logging;
using ScoreCalculator.Domain.MessageBus;
using ScoreCalculator.Domain.Model.Commands;
using ScoreCalculator.Domain.Model.Entities;
using ScoreCalculator.Domain.Repository;

namespace ScoreCalculator.Domain.CommandHandlers;

public class StartCalculateProcessCommandHandler : IRequestHandler<StartCalculateProcess, Unit>
{
    private const long TotalCustomers = 10;
    private readonly ILogger<StartCalculateProcessCommandHandler> _logger;
    private readonly KafkaPublisherMessage _publisher;
    private readonly CalculateProcessRepository _repository;
    private readonly Random _random;

    public StartCalculateProcessCommandHandler(KafkaPublisherMessage publisher, CalculateProcessRepository repository, ILogger<StartCalculateProcessCommandHandler> logger)
    {
        _logger = logger;
        _publisher = publisher;
        _repository = repository;
        _random = new Random();
    }

    public async Task<Unit> Handle(StartCalculateProcess request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[StartCalculateProcess] Handler from command {command} received", typeof(StartCalculateProcess));

        _logger.LogInformation("[StartCalculateProcess] Start Calculate Process with Id {id} for {total} customers ", 
            request.CalculateProcess.Id, TotalCustomers);

        await _repository.SaveAsync(request.CalculateProcess);

        for(long i = 0; i < TotalCustomers; i++)
        {
            var nextCommand = new CalculateScore(
                GenerateCustomerScore(),
                request.CalculateProcess.Id,
                i == TotalCustomers - 1,
                request.SagaKey
            );
            await _publisher.Publish(nextCommand, default);
        }

        return Unit.Value;
    }

    private CustomerScore GenerateCustomerScore() => new(100 * _random.NextDouble());

}