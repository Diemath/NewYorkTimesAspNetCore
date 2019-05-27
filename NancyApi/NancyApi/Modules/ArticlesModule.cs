using Nancy;
using Nancy.Configuration;
using Nancy.ModelBinding;
using NancyApi.Models;
using Services.Abstractions;
using Services.Abstractions.Dto;
using System;
using System.Globalization;
using System.Linq;

namespace NancyApi.Modules
{
  public class ArticlesModule : NancyModule
  {
    private readonly IArticleService _articleService;

    public ArticlesModule(IArticleService articleService, INancyEnvironment environment)
    {
      _articleService = articleService;

      Get("/", _ => {
        return "Hello World!!!";
      });

      Get("/list/{section}", async _ => {
        var dtos = await _articleService.FilterArticlesAsync(
          this.Bind<ArticleInput>().Section
        );
        return Response.AsJson(dtos.Select(MapToVm));
      });

      Get("/list/{section}/first", async _ => {
        var dto = await _articleService.GetArticleAsync(
          this.Bind<ArticleInput>().Section
        );
        return Response.AsJson(MapToVm(dto));
      });

      Get("/list/{section}/{updatedDate}", async _ => {
        var param = this.Bind<ArticleInput>();
        var dtos = await _articleService.FilterArticlesAsync(
          param.Section, 
          DateTime.ParseExact(param.UpdatedDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)
        );
        return Response.AsJson(dtos.Select(MapToVm));
      });

      Get("/article/{shortUrl}", async _ => {
        var dto = await _articleService.GetArticleAsync(
          this.Bind<ArticleInput>().ShortUrl
        );
        return Response.AsJson(MapToVm(dto));
      });

      Get("/group/{section}", async _ => {
        var dtos = await _articleService.GetGroupsAsync(
          this.Bind<ArticleInput>().Section
        );
        return Response.AsJson(dtos.Select(MapToVm));
      });
    }

    private ArticleView MapToVm(ArticleDto dto) 
      => new ArticleView {
      Heading = dto.Title,
      Updated = dto.UpdatedDateTime,
      Link = dto.Url
    };

    private ArticleGroupByDateView MapToVm(ArticleGroupByDateDto dto) => new ArticleGroupByDateView {
      Total = dto.Total,
      Date = dto.UpdatedDate.ToString("yyyy-MM-dd")
    };
  }
}
