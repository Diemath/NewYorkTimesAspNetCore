using Nancy;
using Nancy.Configuration;
using Nancy.ModelBinding;
using Nancy.Responses.Negotiation;
using Nancy.Validation;
using NancyApi.Models;
using Services.Abstractions;
using Services.Abstractions.Dto;
using Services.Abstractions.Enums;
using System;
using System.Globalization;
using System.Linq;

namespace NancyApi.NancyModules
{
  public class ArticlesModule : NancyModule
  {
    private readonly IArticleService _articleService;

    private const string _validDateFormat = "yyyy-MM-dd";

    public ArticlesModule(IArticleService articleService, INancyEnvironment environment)
    {
      _articleService = articleService;

      Get("/", _ =>
      {
        return string.Empty;
      });

      Get("/list/{section}", async _ =>
      {
        var @params = this.Bind<ArticlesBySectionQueryParams>();
        var validationResult = this.Validate(@params);

        if (!validationResult.IsValid)
        {
          return GetErrorResult(this, validationResult);
        }

        var dtos = await _articleService.FilterArticlesAsync(
          ParseSection(@params.Section)
        );

        return Response.AsJson(dtos.Select(MapToVm));
      });

      Get("/list/{section}/first", async _ =>
      {
        var @params = this.Bind<ArticlesBySectionQueryParams>();
        var validationResult = this.Validate(@params);

        if (!validationResult.IsValid)
        {
          return GetErrorResult(this, validationResult);
        }

        var dto = await _articleService.GetArticleAsync(
          ParseSection(@params.Section)
        );

        if (dto is null)
        {
          return GetNotFoundResult(this);
        }

        return Response.AsJson(MapToVm(dto));
      });

      Get("/list/{section}/{updatedDate}", async _ =>
      {
        var @params = this.Bind<ArticlesBySectionAndDateQueryParams>();
        var validationResult = this.Validate(@params);

        if (!validationResult.IsValid)
        {
          return GetErrorResult(this, validationResult);
        }

        var dtos = await _articleService.FilterArticlesAsync(
          ParseSection(@params.Section),
          ParseDate(@params.UpdatedDate)
        );

        return Response.AsJson(dtos.Select(MapToVm));
      });

      Get("/article/{shortUrl}", async _ =>
      {
        var @params = this.Bind<ArticlesByShortUrlQueryParams>();
        var validationResult = this.Validate(@params);

        if (!validationResult.IsValid)
        {
          return GetErrorResult(this, validationResult);
        }

        var dto = await _articleService.GetArticleAsync(
          @params.ShortUrl
        );

        if (dto is null)
        {
          return GetNotFoundResult(this);
        }

        return Response.AsJson(MapToVm(dto));
      });

      Get("/group/{section}", async _ =>
      {
        var @params = this.Bind<ArticlesBySectionQueryParams>();
        var validationResult = this.Validate(@params);

        if (!validationResult.IsValid)
        {
          return GetErrorResult(this, validationResult);
        }

        var dtos = await _articleService.GetGroupsAsync(
          ParseSection(@params.Section)
        );

        return Response.AsJson(dtos.Select(MapToVm));
      });
    }

    private ArticleView MapToVm(ArticleDto dto)
    {
      return new ArticleView
      {
        Heading = dto.Title,
        Updated = dto.UpdatedDateTime,
        Link = dto.Url
      };
    }

    private ArticleGroupByDateView MapToVm(ArticleGroupByDateDto dto)
    {
      return new ArticleGroupByDateView
      {
        Total = dto.Total,
        Date = dto.UpdatedDate.ToString(_validDateFormat)
      };
    }

    private Response GetErrorResult(ArticlesModule articlesModule, ModelValidationResult modelValidationResult)
      => articlesModule.Response.AsJson(modelValidationResult, HttpStatusCode.BadRequest);

    private Response GetNotFoundResult(ArticlesModule articlesModule)
      => articlesModule.Response.AsJson(new { status = nameof(HttpStatusCode.NotFound) }, HttpStatusCode.NotFound);

    private Section ParseSection(string section)
      => (Section)Enum.Parse(typeof(Section), section, true);

    private DateTime ParseDate(string date)
      => DateTime.ParseExact(date, _validDateFormat, CultureInfo.InvariantCulture);
  }
}
