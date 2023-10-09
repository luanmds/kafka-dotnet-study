using Confluent.Kafka;

namespace WorkerConsumer;

public class KafkaConsumer : BackgroundService
{
    private readonly ILogger<KafkaConsumer> _logger;
    private readonly ConsumerConfig _config;
    private readonly string _topicName = "topic-test";

    public KafkaConsumer(ILogger<KafkaConsumer> logger)
    {
        _logger = logger;
        _config = new ConsumerConfig
        {
            BootstrapServers = "localhost:19092",
            GroupId = $"{_topicName}-group-0",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {       
        await Task.Delay(TimeSpan.FromSeconds(1));

        using (var consumer = new ConsumerBuilder<string, string>(_config).Build())
        {
            consumer.Subscribe(new List<string>{_topicName});

            _logger.LogInformation("Consuming messages in topic {0}", _topicName);

            try {

                while (true)
                {
                    var consumeResult = consumer.Consume(stoppingToken);
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
                _logger.LogWarning("Consumer Cancelled...");
            }
        }

    }
}
