namespace ScoreCalculator.Domain.MessageBus;

public class KafkaSettings
{
    public string ConnString { get; set; } = null!;

    public string? ConsumerTopicName { get; set; } = null!;

    public string? PublishTopicName { get; set; } = null!;
}