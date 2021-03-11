namespace SmartApartment.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.OpenApi.Models;
    using Nest;
    using SmartApartment.Common.Abstraction;
    using SmartApartment.Common.Services;
    using SmartApartment.WebApi.Filters;
    using SmartApartment.WebApi.Options;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                    .AddJsonOptions(options => {
                        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    })
                    .AddMvcOptions(options => {
                        options.Filters.Add<JsonExceptionFilter>();
                    });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Smart Apartment Data API", Version = "v1" });
            });

            services.Configure<ElasticsearchOptions>(Configuration.GetSection("Elasticsearch"));

            services.AddSingleton<IElasticClient>((provider) => {
                var options = provider.GetRequiredService<IOptions<ElasticsearchOptions>>().Value;

                // configure default connection settings
                var connectionSettings = new ConnectionSettings(new Uri(options.Uri));

                // TODO: add mapping of we are goint to use this client connection to
                // index more documents.

                return new ElasticClient(connectionSettings);
            });

            services.AddScoped<ISearchService, SearchService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Smart Apartment Data API v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
