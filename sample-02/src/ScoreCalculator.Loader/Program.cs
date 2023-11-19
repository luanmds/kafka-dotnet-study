using MongoDB.Driver;
using ScoreCalculator.Domain.Repository;
using ScoreCalculator.Loader;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {            
        var dbSettings = hostContext.Configuration
            .GetRequiredSection("ScoreDatabaseSettings")
            .Get<DatabaseSettings>();
        services.AddSingleton(dbSettings!);
        services.AddSingleton<IMongoClient>(s => new MongoClient(dbSettings!.ConnectionString));
        services.AddTransient<CalculateProcessRepository>();

        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();
