using Nancy;
using Nancy.Configuration;
using Nancy.ModelBinding;
using Nancy.Validation;
using NYTimes.NancyApi.Models;
using NYTimes.Services.Abstractions;
using NYTimes.Services.Abstractions.Dto;
using NYTimes.Services.Abstractions.Enums;
using System;
using System.Globalization;
using System.Linq;

namespace NYTimes.NancyApi.NancyModules
{
    public class ArticlesModule : NancyModuleBase
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

                var dtos = await _articleService.GetArticlesAsync(
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

                var dtos = await _articleService.GetArticlesAsync(
                    ParseSection(@params.Section)
                );
                var dto = dtos.FirstOrDefault();

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

                var dtos = await _articleService.GetArticlesAsync(
                    ParseSection(@params.Section),
                    DateTime.ParseExact(@params.UpdatedDate, _validDateFormat, CultureInfo.InvariantCulture)
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

        private Section ParseSection(string section)
         => (Section)Enum.Parse(typeof(Section), section, true);
    }
}
