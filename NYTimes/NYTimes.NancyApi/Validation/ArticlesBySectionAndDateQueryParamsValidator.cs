using FluentValidation;
using NYTimes.NancyApi.Models;
using NYTimes.NancyApi.Validators.Extensions;

namespace NYTimes.NancyApi.Validators
{
  public class ArticlesBySectionAndDateQueryParamsValidator : AbstractValidator<ArticlesBySectionAndDateQueryParams>
  {
    public ArticlesBySectionAndDateQueryParamsValidator()
    {
      RuleFor(x => x.UpdatedDate).MustBeValidDate();
      RuleFor(x => x.Section).MustBeFromSectionEnum();
    }
  }
}
