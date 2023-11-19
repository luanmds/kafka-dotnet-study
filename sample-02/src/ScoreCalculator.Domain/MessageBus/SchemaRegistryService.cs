using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using ScoreCalculator.Domain.MessageBus.Settings;
using ScoreCalculator.Domain.Model.Messages;

namespace ScoreCalculator.Domain.MessageBus;

public class SchemaRegistryService 
{
    private readonly SchemaRegistryConfig _config;

    public SchemaRegistryService(SchemaRegistrySettings settings)
    {
        _config = new SchemaRegistryConfig 
        {
            Url = settings.SchemaRegistryUrl
        };
    }

    public IAsyncSerializer<MessageData> GetSerializer()
    {
        var client = new CachedSchemaRegistryClient(_config);

        return new JsonSerializer<MessageData>(client);
    }    

    public static IAsyncDeserializer<MessageData> GetDeserializer() => new JsonDeserializer<MessageData>();
}