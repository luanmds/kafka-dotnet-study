using Confluent.Kafka;

var config = new ProducerConfig
{
    BootstrapServers = "localhost:19092"
};

using (var producer = new ProducerBuilder<string, string>(config).Build())
{
    var deliveryResult = await producer.ProduceAsync(
        "topic-test", 
        new Message<string, string> { Key = "Example", Value="A random message" });

    if(deliveryResult.Status == PersistenceStatus.NotPersisted)
        Console.WriteLine("Produce not delivered message in Topic {0}.", deliveryResult.Topic);
    else
        Console.WriteLine($"Produce delivered message in Topic {deliveryResult.Topic} and Partition {deliveryResult.Partition}.");
}