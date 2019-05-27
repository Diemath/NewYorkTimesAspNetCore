using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using Services.Abstractions;
using Services.Abstractions.Configurations;
using Services.Concrete;
using System.IO;

namespace NancyApi
{
  public class Bootstrapper : DefaultNancyBootstrapper
  {
    protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
    {
      container.Register<IHttpGetter, HttpGetter>();
      container.Register<IArticleService, ArticleService>();

      var appConfig = JsonConvert.DeserializeObject<AppConfig>(File.ReadAllText("appsettings.json"));
      container.Register<IConfigProvider, ConfigProvider>(new ConfigProvider(appConfig));
    }
  }
}
