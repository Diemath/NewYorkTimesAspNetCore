using FluentValidation;
using NancyApi.Models;
using Services.Abstractions.Enums;

namespace NancyApi.Validators
{
  public class ArticlesBySectionAndDateQueryParamsValidator : AbstractValidator<ArticlesBySectionAndDateQueryParams>
  {
    public ArticlesBySectionAndDateQueryParamsValidator()
    {
      RuleFor(x => x.UpdatedDate).Must(x => x.BeValidDate()).WithMessage("Valid format for updated date is yyyy-MM-dd.");
      RuleFor(x => x.Section).Must(x => x.BeFromSectionEnum()).WithMessage($"The possible section value are: {typeof(Section).JoinValues<Section>()}.");
    }
  }

  public class ArticlesBySectionQueryParamsValidator : AbstractValidator<ArticlesBySectionQueryParams>
  {
    public ArticlesBySectionQueryParamsValidator()
    {
      RuleFor(x => x.Section).Must(x => x.BeFromSectionEnum()).WithMessage($"The possible section value are: {typeof(Section).JoinValues<Section>()}.");
    }
  }

  public class ArticlesByShortUrlQueryParamsValidator : AbstractValidator<ArticlesByShortUrlQueryParams>
  {
    public ArticlesByShortUrlQueryParamsValidator()
    {
      RuleFor(x => x.ShortUrl).Length(7).WithMessage("Valid format for short url is XXXXXXX.");
    }
  }
}
