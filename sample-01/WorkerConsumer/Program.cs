using WorkerConsumer;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<KafkaConsumer>();
    })
    .Build();

host.Run();

Console.WriteLine("WorkerConsumer started successfully");
