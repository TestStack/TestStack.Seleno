using System;

namespace Funq
{
    public interface IContainer : IDisposable
    {
        TService Resolve<TService>();
    }
}