using FluentValidation;
using NYTimes.NancyApi.Models;
using NYTimes.NancyApi.Validators.Extensions;

namespace NYTimes.NancyApi.Validation
{
    public class ArticlesByShortUrlQueryParamsValidator : AbstractValidator<ArticlesByShortUrlQueryParams>
    {
        public ArticlesByShortUrlQueryParamsValidator()
        {
            RuleFor(x => x.ShortUrl).MustHaveExactLength();
        }
    }
}
