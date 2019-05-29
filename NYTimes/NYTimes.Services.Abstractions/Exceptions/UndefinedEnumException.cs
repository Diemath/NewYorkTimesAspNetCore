using System;

namespace NYTimes.Services.Abstractions.Exceptions
{
    public class UndefinedEnumException : Exception
    {
        public UndefinedEnumException(string message) : base(message)
        {
        }
    }
}
