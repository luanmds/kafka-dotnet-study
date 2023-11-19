using System.Reflection;
using MediatR;
using MongoDB.Driver;
using ScoreCalculator.Domain.CommandHandlers;
using ScoreCalculator.Domain.MessageBus;
using ScoreCalculator.Domain.MessageBus.Settings;
using ScoreCalculator.Domain.Model.Commands;
using ScoreCalculator.Domain.Repository;

var handlersAssemblies = Array.Empty<Assembly>();

IHostBuilder builder = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostContext, configuration) =>
    {
        configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
    })
    .ConfigureServices((hostContext, services) =>
    {            
        var dbSettings = hostContext.Configuration.GetRequiredSection("ScoreDatabaseSettings").Get<DatabaseSettings>();
        var kafkaSettings = hostContext.Configuration.GetRequiredSection("KafkaSettings").Get<KafkaSettings>();
        var schemaSettings = hostContext.Configuration.GetRequiredSection("SchemaRegistrySettings").Get<SchemaRegistrySettings>();
      
        services.AddSingleton(dbSettings!);
        services.AddSingleton<IMongoClient>(s => new MongoClient(dbSettings!.ConnectionString));
        services.AddTransient<CalculateProcessRepository>();
        
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(IMediator).Assembly));

        services.AddTransient(typeof(IRequestHandler<CalculateScore, Unit>), typeof(CalculateScoreCommandHandler));

        services.AddSingleton(kafkaSettings!);
        services.AddSingleton(schemaSettings!);
        services.AddTransient<SchemaRegistryService>();
        services.AddHostedService<KafkaConsumerMessage>();
    });

var host = builder.Build();

host.Run();
