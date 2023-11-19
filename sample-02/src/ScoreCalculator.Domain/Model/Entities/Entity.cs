using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace ScoreCalculator.Domain.Model.Entities;

public class Entity
{
    [BsonId]
    [JsonProperty(PropertyName = "Id")]
    public string Id { get; set; }

}