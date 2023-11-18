using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ScoreCalculator.Domain.MessageBus.Settings;
using ScoreCalculator.Domain.Model.Commands;
using ScoreCalculator.Domain.Model.Messages;

namespace ScoreCalculator.Domain.MessageBus;

public class KafkaPublisherMessage
{
    private readonly ILogger<KafkaPublisherMessage> _logger;
    private readonly ProducerConfig _config;
    private readonly string _topicName;
    private readonly SchemaRegistryService _schemaRegistryService;

    public KafkaPublisherMessage(KafkaSettings settings, SchemaRegistryService schemaRegistryService, ILogger<KafkaPublisherMessage> logger)
    {
        _logger = logger;
        _topicName = settings.PublishTopicName ?? "";
        _schemaRegistryService = schemaRegistryService;

        _config = new ProducerConfig
        {
            BootstrapServers = settings.ConnString
        };
    }

    public async Task Publish(Command message, CancellationToken stoppingToken)
    {
        using var producer = new ProducerBuilder<string, MessageData>(_config)
            .SetValueSerializer(_schemaRegistryService.GetSerializer())
            .Build();

        var data = ConvertToMessage(message);
        
        var deliveryResult = await producer.ProduceAsync(
            _topicName, 
            new Message<string, MessageData> { Key = data.Id, Value = data }, stoppingToken);

        if (deliveryResult.Status == PersistenceStatus.NotPersisted)
            _logger.LogError("Produce not delivered message in Topic {topic}", deliveryResult.Topic);
        else
            _logger.LogInformation("Produce delivered message in Topic {topic} and Partition {partition}", 
                deliveryResult.Topic, deliveryResult.Partition);

    }

    private static MessageData ConvertToMessage(Command message) => new()
    {
        Id = message.Id,
        Message = JsonConvert.SerializeObject(message),
        MessageType = message.GetType().Name
    };
}
