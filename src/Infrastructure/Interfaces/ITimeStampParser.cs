using System;

namespace Infrastructure.Interfaces
{
    public interface ITimeStampParser
    {
        DateTimeOffset Parse(string content);
    }
}