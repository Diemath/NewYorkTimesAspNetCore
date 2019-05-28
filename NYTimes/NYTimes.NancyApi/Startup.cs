using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nancy.Owin;
using Services.Abstractions.Configurations;

namespace NancyApi
{
  public class Startup
  {
    private readonly IConfiguration configuration;

    public Startup(IHostingEnvironment env)
    {
      configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .SetBasePath(env.ContentRootPath)
        .Build();
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    { 
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      var appConfig = new AppConfig();
      ConfigurationBinder.Bind(configuration, appConfig);

      app.UseOwin(b => b.UseNancy(opt => opt.Bootstrapper = new Bootstrapper(appConfig)));

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
    }
  }
}
