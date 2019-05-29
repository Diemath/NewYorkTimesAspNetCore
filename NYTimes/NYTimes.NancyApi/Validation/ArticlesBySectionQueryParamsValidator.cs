using FluentValidation;
using NYTimes.NancyApi.Models;
using NYTimes.NancyApi.Validators.Extensions;

namespace NYTimes.NancyApi.Validation
{
  public class ArticlesBySectionQueryParamsValidator : AbstractValidator<ArticlesBySectionQueryParams>
  {
    public ArticlesBySectionQueryParamsValidator()
    {
      RuleFor(x => x.Section).MustBeFromSectionEnum();
    }
  }
}
