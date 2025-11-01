using System.Collections.Generic;
using Blazor.EventGridViewer.Core.Models;
using Blazor.EventGridViewer.Services;
using Blazor.EventGridViewer.Services.Adapters;
using Blazor.EventGridViewer.Services.Interfaces;
using BlazorStrap;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Blazor.EventGridViewer.ServerApp
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Add Application Insights (only if connection string is provided)
            var aiConnectionString = Configuration.GetConnectionString("ApplicationInsights")
                ?? Configuration["ApplicationInsights:ConnectionString"];

            if (!string.IsNullOrEmpty(aiConnectionString))
            {
                services.AddApplicationInsightsTelemetry(options =>
                {
                    options.ConnectionString = aiConnectionString;
                });

                // Configure sampling only in Production to reduce costs
                if (_env.IsProduction())
                {
                    services.Configure<TelemetryConfiguration>(telemetryConfig =>
                    {
                        telemetryConfig.DefaultTelemetrySink.TelemetryProcessorChainBuilder
                            .UseSampling(5.0)
                            .Build();
                    });
                }
            }

            services.AddControllersWithViews();

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddBlazorStrap();
            services.AddScoped<IEventGridIdentifySchemaService, EventGridIdentifySchemaService>();
            services.AddScoped<IAdapter<string, List<EventGridEventModel>>, EventGridSchemaAdapter>();
            services.AddSingleton<IAdapter<EventGridEventModel, EventGridViewerEventModel>, EventGridEventModelAdapter>();
            services.AddSingleton<IEventGridService, EventGridService>();
            services.AddScoped<IEventGridViewerService, EventGridViewerService>();
        }        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            logger.LogInformation("Configuring application pipeline. Environment: {Environment}", env.EnvironmentName);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                logger.LogInformation("Development environment detected - using developer exception page");
            }
            else
            {
                // Temporarily enable detailed errors for troubleshooting
                if (Configuration.GetValue<bool>("DetailedErrors"))
                {
                    app.UseDeveloperExceptionPage();
                    logger.LogWarning("TEMPORARY: Detailed errors enabled in production for troubleshooting");
                }
                else
                {
                    app.UseExceptionHandler("/Error");
                }

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseHttpsRedirection();
                logger.LogInformation("Production environment detected - using standard error handling");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
                endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            logger.LogInformation("Application pipeline configuration completed");
        }
    }
}
