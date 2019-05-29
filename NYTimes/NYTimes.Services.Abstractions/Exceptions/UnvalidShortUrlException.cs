using System;

namespace NYTimes.Services.Abstractions.Exceptions
{
  public class UnvalidShortUrlException : Exception
  {
    public UnvalidShortUrlException(string message) : base(message)
    {
    }
  }
}
