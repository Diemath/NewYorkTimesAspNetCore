using Nancy;
using Nancy.ModelBinding;
using System;

namespace NancyApi
{
  public class ArticlesModule : NancyModule
  {
    public ArticlesModule()
    {
      Get("/", _ => {
        return "Hello World!!!";
      });

      Get("/list/{section}", _ => {
        var param = this.Bind<ArticleInputParams>();
        return $"section: {param.Section}";
      });

      Get("/list/{section}/first", _ => {
        var param = this.Bind<ArticleInputParams>();
        return $"section: {param.Section}";
      });

      Get("/list/{section}/{updatedDate}", _ => {
        var param = this.Bind<ArticleInputParams>();
        return $"section: {param.Section}; updatedDate: {param.UpdatedDate}";
      });

      Get("/article/{shortUrl}", _ => {
        var param = this.Bind<ArticleInputParams>();
        return $"shortUrl: {param.ShortUrl}";
      });

      Get("/group/{section}", _ => {
        var param = this.Bind<ArticleInputParams>();
        return $"section: {param.Section}";
      });
    }
  }

  public class ArticleInputParams
  {
    public string Section { get; set; } 
    public DateTime UpdatedDate { get; set; }
    public string ShortUrl { get; set; }
  }
}
