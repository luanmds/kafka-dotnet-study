
using Newtonsoft.Json;

namespace ScoreCalculator.Domain.Model.Messages;

public class MessageData
{
    [JsonRequired]
    [JsonProperty("id")]
    public required string Id { get; set; }

    [JsonRequired]
    [JsonProperty("message")]
    public required string Message { get; set; }

    [JsonRequired]
    [JsonProperty("message_type")]
    public required string MessageType { get; set; }
}