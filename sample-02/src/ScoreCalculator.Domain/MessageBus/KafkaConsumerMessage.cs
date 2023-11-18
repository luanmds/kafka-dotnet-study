using System.Text.Json;
using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ScoreCalculator.Domain.MessageBus.Settings;
using ScoreCalculator.Domain.Model.Messages;

namespace ScoreCalculator.Domain.MessageBus;

public class KafkaConsumerMessage : BackgroundService
{
    private readonly IMediator _mediator;
    private readonly ILogger<KafkaConsumerMessage> _logger;
    private readonly ConsumerConfig _config;
    private readonly string _topicName;

    public KafkaConsumerMessage(KafkaSettings settings, IMediator mediator, ILogger<KafkaConsumerMessage> logger)
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

        using (var consumer = new ConsumerBuilder<string, MessageData>(_config).Build())
        {
            consumer.Subscribe(new List<string>{_topicName});

            _logger.LogInformation("Consuming messages in topic {topicName}", _topicName);

            try {

                while (true)
                {
                    var consumeResult = consumer.Consume(stoppingToken);
                    var messageData = consumeResult.Message.Value;
                    _logger.LogInformation("Message received with Key {key} and Type {messageType}", 
                        consumeResult.Message.Key, messageData.MessageType);

                    var commandType = Type.GetType($"ScoreCalculator.Domain.Model.Commands.{messageData.MessageType}");

                    if(commandType is null)
                    {
                        _logger.LogInformation("Message Type {type} not exists", messageData.MessageType);
                        continue;
                    }

                    var data = JsonSerializer.Deserialize(messageData.Message, commandType) ?? new object();

                    await _mediator.Send(data, default);

                    _logger.LogInformation("Message Data: \nPartition: {partition} \n Key: {key} \n Value: {value} ",
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
}
