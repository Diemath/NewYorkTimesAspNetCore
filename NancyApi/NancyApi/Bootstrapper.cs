using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using Services.Abstractions;
using Services.Concrete;

namespace NancyApi
{
  public class Bootstrapper : DefaultNancyBootstrapper
  {
    protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
    {
      container.Register<IArticleService, ArticleService>();
    }
  }
}
