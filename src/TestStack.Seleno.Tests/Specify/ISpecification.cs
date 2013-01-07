using System;

namespace TestStack.Seleno.Tests.Specify
{
    public interface ISpecification
    {
        Type Story { get; }
        string Title { get; set; }
    }
}