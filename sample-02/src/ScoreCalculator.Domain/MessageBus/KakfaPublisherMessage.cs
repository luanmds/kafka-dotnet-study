using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ScoreCalculator.Domain.Model.Commands;

namespace ScoreCalculator.Domain.MessageBus;

public class KafkaPublisherMessage
{
    private readonly ILogger<KafkaPublisherMessage> _logger;
    private readonly ProducerConfig _config;
    private readonly string _topicName;

    public KafkaPublisherMessage(KafkaSettings settings, ILogger<KafkaPublisherMessage> logger)
    {
        _logger = logger;
        _topicName = settings.PublishTopicName ?? "";

        _config = new ProducerConfig
        {
            BootstrapServers = settings.ConnString
        };
    }

    public async Task Publish(Command message, CancellationToken stoppingToken)
    {
        using var producer = new ProducerBuilder<string, string>(_config).Build();

        var data = ConvertToMessage(message);
        
        var deliveryResult = await producer.ProduceAsync(
            _topicName, 
            new Message<string, string> { Key = data.Id, Value =  JsonConvert.SerializeObject(data) }, stoppingToken);

        if (deliveryResult.Status == PersistenceStatus.NotPersisted)
            _logger.LogError("Produce not delivered message in Topic {topic}", deliveryResult.Topic);
        else
            _logger.LogInformation("Produce delivered message in Topic {topic} and Partition {partition}", 
                deliveryResult.Topic, deliveryResult.Partition);

    }

    private static MessageData ConvertToMessage(Command message) => new()
    {
        Id = message.Id,
        Message = message,
        MessageType = nameof(message)
    };
}
