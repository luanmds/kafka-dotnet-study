using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Bson.IO;
using Newtonsoft.Json;

namespace ScoreCalculator.Domain.MessageBus;

public class KafkaMessageHandler : BackgroundService
{
    private readonly IMediator _mediator;
    private readonly ILogger<KafkaMessageHandler> _logger;
    private readonly ConsumerConfig _config;
    private readonly string _topicName;

    public KafkaMessageHandler(KafkaSettings settings, IMediator mediator, ILogger<KafkaMessageHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
        _topicName = settings.ConsumerTopicName ?? "";

        _config = new ConsumerConfig
        {
            BootstrapServers = settings.ConnString,
            GroupId = $"{_topicName}-group-0",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {       
        await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);

        using (var consumer = new ConsumerBuilder<string, string>(_config).Build())
        {
            consumer.Subscribe(new List<string>{_topicName});

            _logger.LogInformation("Consuming messages in topic {0}", _topicName);

            try {

                while (true)
                {
                    var consumeResult = consumer.Consume(stoppingToken);
                    ConvertToMessageData(consumeResult.Message.Value);

                    // TODO HOW convert message
                    _logger.LogInformation("Message received!");
                    _logger.LogInformation("Partition: {0} \n Key: {1} \n Value: {2} ",
                        consumeResult.Partition,
                        consumeResult.Message.Key, 
                        consumeResult.Message.Value);

                }
            }
            catch (OperationCanceledException)
            {
                consumer.Close();
                _logger.LogWarning("Consumer Finished...");
            }
        }

    }

    private static MessageData ConvertToMessageData(string message)
    {
    //TODO 
    }
}
