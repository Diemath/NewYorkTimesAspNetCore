using System;

namespace Services.Abstractions.Exceptions
{
  public class UnvalidShortUrlException : Exception
  {
    public UnvalidShortUrlException(string message) : base(message)
    {
    }
  }
}
