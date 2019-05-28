using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nancy.Owin;
using NYTimes.NancyApi.NancyModules;
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

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IHttpRequestHelper, HttpRequestHelper>();
            services.AddTransient<IArticleService, ArticleService>();

            var appConfig = new AppConfig();
            ConfigurationBinder.Bind(Configuration, appConfig);
            services.AddTransient<IConfigProvider>(s => new ConfigProvider(appConfig));

            Services = services;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
