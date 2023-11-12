using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ScoreCalculator.Domain.Model.Entities;

public class Entity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public required string Id { get; set; }
}