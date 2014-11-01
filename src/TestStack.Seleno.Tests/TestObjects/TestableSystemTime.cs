using System;
using TestStack.Seleno.Extensions;

namespace TestStack.Seleno.Tests.TestObjects
{
    public class TestableSystemTime : IDisposable
    {
        public TestableSystemTime(DateTime dateTime)
        {
            SystemTime.Now = () => dateTime;
        }

        public TestableSystemTime(Func<DateTime> dateTimeFactory)
        {
            SystemTime.Now = dateTimeFactory;
        }

        public void Dispose()
        {
            SystemTime.Reset();
        }
    }
}
