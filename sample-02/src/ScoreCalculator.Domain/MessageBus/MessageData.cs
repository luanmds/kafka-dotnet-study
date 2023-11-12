namespace ScoreCalculator.Domain.MessageBus;

public class MessageData
{
    public required string Id { get; set; }
    public required object Message { get; set; }
    public required string MessageType { get; set; }
}