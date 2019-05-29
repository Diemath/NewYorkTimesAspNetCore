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

    public static IRuleBuilderOptions<T, string> MustBeFromSectionEnum<T>(this IRuleBuilder<T, string> ruleBuilder)
      => ruleBuilder.Must(x => x.BeFromSectionEnum()).WithMessage($"The possible section value are: {typeof(Section).JoinValues<Section>()}.");

    public static IRuleBuilderOptions<T, string> MustHaveExactLength<T>(this IRuleBuilder<T, string> ruleBuilder)
      => ruleBuilder.Length(7).WithMessage("Valid format for short url is XXXXXXX.");

    private static bool BeValidDate(this string date)
      => DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime res);

    private static bool BeFromSectionEnum(this string section)
      => Enum.TryParse(section, true, out Section res) && Enum.IsDefined(typeof(Section), res);

    private static string JoinValues<TEnum>(this Type @enum)
      => string.Join(", ", Enum.GetValues(@enum).Cast<TEnum>().Select(e => e.ToString().ToLower()));
  }
}
