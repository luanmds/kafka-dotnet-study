using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ScoreCalculator.Domain.MessageBus;
using ScoreCalculator.Domain.MessageBus.Settings;

namespace ScoreCalculator.Api;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "ScoreCalculator.Api", Version = "v1" });
        });
        
        var kafkaSettings = Configuration.GetRequiredSection("KafkaSettings").Get<KafkaSettings>();
        var schemaSettings = Configuration.GetRequiredSection("SchemaRegistrySettings").Get<SchemaRegistrySettings>();
      
        services.AddSingleton(kafkaSettings);
        services.AddSingleton(schemaSettings);
        services.AddTransient<SchemaRegistryService>();
        services.AddTransient<KafkaPublisherMessage>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ScoreCalculator.Api v1"));
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
