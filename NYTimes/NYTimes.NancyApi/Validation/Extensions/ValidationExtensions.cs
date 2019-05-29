using FluentValidation;
using NYTimes.Services.Abstractions.Enums;
using System;
using System.Globalization;
using System.Linq;

namespace NYTimes.NancyApi.Validators.Extensions
{
    public static class ValidationExtensions
    {
        public static IRuleBuilderOptions<T, string> MustBeValidDate<T>(this IRuleBuilder<T, string> ruleBuilder)
          => ruleBuilder.Must(m => m.BeValidDate()).WithMessage("Valid format for updated date is yyyy-MM-dd.");

        public static IRuleBuilderOptions<T, string> IsInEnum<T>(this IRuleBuilder<T, string> ruleBuilder, Type enumType, string overrideProertyName = null)
        {
            var validElements = Enum.GetNames(enumType);
            return ruleBuilder.Must(p => validElements.Any(v => v.Equals(p, StringComparison.InvariantCultureIgnoreCase)))
                       .WithMessage("'{PropertyName}' must be one of: '" + string.Join(", ", validElements) + "'.")
                       .OverridePropertyName(overrideProertyName ?? enumType.Name);
        }
        
        public static IRuleBuilderOptions<T, string> MustHaveExactLength<T>(this IRuleBuilder<T, string> ruleBuilder)
          => ruleBuilder.Length(7).WithMessage("Valid format for short url is XXXXXXX.");

        private static bool BeValidDate(this string date)
          => DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime res);
    }
}
