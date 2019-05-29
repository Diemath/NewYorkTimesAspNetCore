using FluentValidation;
using NYTimes.NancyApi.Models;
using NYTimes.NancyApi.Validators.Extensions;
using NYTimes.Services.Abstractions.Enums;

namespace NYTimes.NancyApi.Validators
{
    public class ArticlesBySectionAndDateQueryParamsValidator : AbstractValidator<ArticlesBySectionAndDateQueryParams>
    {
        public ArticlesBySectionAndDateQueryParamsValidator()
        {
            RuleFor(x => x.UpdatedDate).MustBeValidDate("yyyy-MM-dd");
            RuleFor(x => x.Section).IsInEnum(typeof(Section));
        }
    }
}
