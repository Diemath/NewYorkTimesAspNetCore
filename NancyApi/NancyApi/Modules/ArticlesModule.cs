using Nancy;
using Nancy.ModelBinding;
using NancyApi.Models;
using Services.Abstractions;

namespace NancyApi.Modules
{
  public class ArticlesModule : NancyModule
  {
    public ArticlesModule(IArticleService articleService)
    {
      Get("/", _ => {
        return "Hello World!!!";
      });

      Get("/list/{section}", _ => {
        var param = this.Bind<ArticleInput>();
        return Response.AsJson(new ArticleView[] { });
      });

      Get("/list/{section}/first", _ => {
        var param = this.Bind<ArticleInput>();
        return Response.AsJson(new ArticleView[] { });
      });

      Get("/list/{section}/{updatedDate}", _ => {
        var param = this.Bind<ArticleInput>();
        return Response.AsJson(new ArticleView[] { });
      });

      Get("/article/{shortUrl}", _ => {
        var param = this.Bind<ArticleInput>();
        return Response.AsJson(new ArticleView { });
      });

      Get("/group/{section}", _ => {
        var param = this.Bind<ArticleInput>();
        return Response.AsJson(new ArticleGroupByDateView[] { });
      });
    }
  }
}
