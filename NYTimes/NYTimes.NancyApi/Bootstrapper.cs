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
  //public class Bootstrapper : DefaultNancyBootstrapper
  //{
  //  private readonly AppConfig _appConfig;

  //  public Bootstrapper()
  //  {
  //  }

  //  public Bootstrapper(AppConfig appConfig)
  //  {
  //    _appConfig = appConfig;
  //  }

  //  protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
  //  {
  //    container.Register<IHttpRequestHelper, HttpRequestHelper>();
  //    container.Register<IArticleService, ArticleService>();
  //    container.Register<IConfigProvider>(new ConfigProvider(_appConfig));
  //  }
  //}
}
