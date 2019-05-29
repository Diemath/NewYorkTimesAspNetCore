using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nancy.Owin;
using NYTimes.NancyApi.NancyModules;
using NYTimes.Services;
using NYTimes.Services.Abstractions;
using NYTimes.Services.Abstractions.Configurations;
using Serilog;

namespace NYTimes.NancyApi
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
        }

        public IConfiguration Configuration { get; }
        public IServiceCollection Services { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IRestClientFactory, RestClientFactory>();
            services.AddTransient<IArticleService, ArticleService>();
            services.Configure<ApiConfig>(Configuration.GetSection("Api"));

            Services = services;
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseOwin().UseNancy(x => x.Bootstrapper = new NancyBootstraper(Services));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
        }
    }
}
