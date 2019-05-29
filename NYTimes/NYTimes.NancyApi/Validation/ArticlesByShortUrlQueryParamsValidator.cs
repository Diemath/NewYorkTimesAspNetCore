using FluentValidation;
using NYTimes.NancyApi.Models;

namespace NYTimes.NancyApi.Validation
{
    public class ArticlesByShortUrlQueryParamsValidator : AbstractValidator<ArticlesByShortUrlQueryParams>
    {
        public ArticlesByShortUrlQueryParamsValidator()
        {
            RuleFor(x => x.ShortUrl).Length(7).WithMessage("Valid format for short url is XXXXXXX.");
        }
    }
}
