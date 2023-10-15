using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using UrlShortener.Domain.Model.Commands;
using UrlShortener.Domain.Model.Queries;
using UrlShortener.Domain.Repository;

namespace UrlShortener.Api
{
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "UrlShortener.Api", Version = "v1" });
            });

            var databaseSettings = Configuration.GetSection("UrlDatabaseSettings").Get<DatabaseSettings>();
            
            services.AddSingleton<DatabaseSettings>(databaseSettings);
            services.AddSingleton<IMongoClient>(s => new MongoClient(databaseSettings.ConnectionString));
            services.AddTransient<GenerateURL>();
            services.AddTransient<GetUrlGeneratedByIdQueryHandler>();
            services.AddTransient<GetOriginalUrlByCodeUrlQueryHandler>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UrlShortener.Api v1"));
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
