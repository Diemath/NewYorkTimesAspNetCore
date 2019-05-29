using FluentValidation;
using NYTimes.NancyApi.Models;
using NYTimes.NancyApi.Validators.Extensions;
using NYTimes.Services.Abstractions.Enums;

namespace NYTimes.NancyApi.Validation
{
    public class ArticlesBySectionQueryParamsValidator : AbstractValidator<ArticlesBySectionQueryParams>
    {
        public ArticlesBySectionQueryParamsValidator()
        {
            RuleFor(x => x.Section).MustBeFromSectionEnum();

            RuleFor(x => x.Section).IsInEnum(typeof(Section));//TODO: Consider this
        }
    }
}
