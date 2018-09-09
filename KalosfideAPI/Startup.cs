using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KalosfideAPI.Démarrage;
using KalosfideAPI.Erreurs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace KalosfideAPI
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
            BaseDeDonnées.ConfigureService(services, Configuration);

            ServicesDeDonnées.ConfigureServices(services);

            Authentification.ConfigureServices(services, Configuration);

            services.AddCors(options =>
            {
            options.AddPolicy("AutoriseLocalhost",
                builder => builder
                    .WithOrigins("https://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .Build()
                );
            });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseMiddleware<ErrorWrappingMiddleware>();

            // Cross - Origin Read Blocking(CORB) blocked cross-origin response
            app.UseCors("AutoriseLocalhost");

            // 
            BaseDeDonnées.Configure(Configuration, env, app.ApplicationServices);

            app.UseAuthentication();

            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.UseMvc();

        }
    }
}
