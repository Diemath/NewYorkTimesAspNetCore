using FluentValidation;
using NancyApi.Models;
using NancyApi.Validators.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
