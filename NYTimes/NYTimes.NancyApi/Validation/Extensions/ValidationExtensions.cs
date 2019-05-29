using FluentValidation;
using System;
using System.Globalization;
using System.Linq;

namespace NYTimes.NancyApi.Validators.Extensions
{
    public static class ValidationExtensions
    {
        public static IRuleBuilderOptions<T, string> MustBeValidDate<T>(this IRuleBuilder<T, string> ruleBuilder, string format)
          => ruleBuilder.Must(p => DateTime.TryParseExact(p, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime res))
                        .WithMessage($"Valid format for updated date is {format}.");

        public static IRuleBuilderOptions<T, string> IsInEnum<T>(this IRuleBuilder<T, string> ruleBuilder, Type enumType, string overrideProertyName = null)
        {
            var validElements = Enum.GetNames(enumType);
            return ruleBuilder.Must(p => validElements.Any(v => v.Equals(p, StringComparison.InvariantCultureIgnoreCase)))
                       .WithMessage("'{PropertyName}' must be one of: '" + string.Join(", ", validElements) + "'.")
                       .OverridePropertyName(overrideProertyName ?? enumType.Name);
        }
    }
}
