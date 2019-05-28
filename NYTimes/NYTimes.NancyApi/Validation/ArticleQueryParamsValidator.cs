using FluentValidation;
using NancyApi.Models;
using NancyApi.Validators.Extensions;
using Services.Abstractions.Enums;

namespace NancyApi.Validators
{
  public class ArticlesBySectionAndDateQueryParamsValidator : AbstractValidator<ArticlesBySectionAndDateQueryParams>
  {
    public ArticlesBySectionAndDateQueryParamsValidator()
    {
      RuleFor(x => x.UpdatedDate).MustBeValidDate();
      RuleFor(x => x.Section).MustBeFromSectionEnum();
    }
  }

  public class ArticlesBySectionQueryParamsValidator : AbstractValidator<ArticlesBySectionQueryParams>
  {
    public ArticlesBySectionQueryParamsValidator()
    {
      RuleFor(x => x.Section).MustBeFromSectionEnum();
    }
  }

  public class ArticlesByShortUrlQueryParamsValidator : AbstractValidator<ArticlesByShortUrlQueryParams>
  {
    public ArticlesByShortUrlQueryParamsValidator()
    {
      RuleFor(x => x.ShortUrl).MustHaveExactLength();
    }
  }
}
