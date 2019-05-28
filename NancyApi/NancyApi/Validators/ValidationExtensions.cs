﻿using Services.Abstractions.Enums;
using System;
using System.Globalization;
using System.Linq;

namespace NancyApi.Validators
{
  public static class ValidationExtensions
  {
    public static bool BeValidDate(this string date)
    {
      return DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime res);
    }

    public static bool BeFromSectionEnum(this string section)
    {
      return Enum.TryParse(section, true, out Section res) && Enum.IsDefined(typeof(Section), res);
    }

    public static string JoinValues<TEnum>(this Type @enum)
    {
      return string.Join(", ", Enum.GetValues(@enum).Cast<TEnum>().Select(e => e.ToString().ToLower()));
    }
  }
}