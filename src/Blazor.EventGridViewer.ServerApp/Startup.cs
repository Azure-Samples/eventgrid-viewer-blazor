using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BlazorStrap;
using Blazor.EventGridViewer.Services.Interfaces;
using Blazor.EventGridViewer.Services;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authentication;
using Blazor.EventGridViewer.Services.Adapters;
using Microsoft.Azure.EventGrid.Models;
using Blazor.EventGridViewer.Core.Models;
using System.Collections.Generic;

namespace Blazor.EventGridViewer.ServerApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Check to see if AzureAD Auth is enabled
            if (EnableAuth())
            {
                services.AddAuthentication(AzureADDefaults.AuthenticationScheme)
                   .AddAzureAD(options => Configuration.Bind("AzureAd", options));

                services.AddControllersWithViews(options =>
                {
                    var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();
                    options.Filters.Add(new AuthorizeFilter(policy));
                });
            }

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddBootstrapCss();
            services.AddScoped<IEventGridIdentifySchemaService, EventGridIdentifySchemaService>();
            services.AddScoped<IAdapter<string, List<EventGridEventModel>>, EventGridSchemaAdapter>();
            services.AddSingleton<IAdapter<EventGridEventModel, EventGridViewerEventModel>, EventGridEventModelAdapter>();
            services.AddSingleton<IEventGridService, EventGridService>();
            services.AddScoped<IEventGridViewerService, EventGridViewerService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseHttpsRedirection();
            }


            app.UseStaticFiles();

            app.UseRouting();

            // Check to see if AzureAD Auth is enabled
            if (EnableAuth())
            {
                app.UseAuthentication();
                app.UseAuthorization();
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
                endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        /// <summary>
        /// Check EnableAuth value
        /// </summary>
        /// <returns>boolean</returns>
        private bool EnableAuth() => Configuration["EnableAuth"] == "true";
    }
}
