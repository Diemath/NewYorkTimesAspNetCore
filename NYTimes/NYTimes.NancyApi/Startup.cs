using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nancy.Owin;
using NYTimes.NancyApi.NancyModules;
using NYTimes.Services;
using NYTimes.Services.Abstractions;
using Services.Abstractions;
using Services.Abstractions.Configurations;
using Services.Concrete;

namespace NancyApi
{
    public class Startup
    {
        public IServiceCollection Services { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IRestClientFactory, RestClientFactory>();
            services.AddTransient<IArticleService, ArticleService>();
            services.Configure<ApiConfig>(Configuration.GetSection("Api"));

            Services = services;
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var appConfig = new AppConfig();
            ConfigurationBinder.Bind(Configuration, appConfig);

            app.UseOwin().UseNancy(x => x.Bootstrapper = new NancyBootstraper(Services));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
        }
    }
}
