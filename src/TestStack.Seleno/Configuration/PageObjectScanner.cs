using System;
using System.Linq;
using System.Reflection;
using TestStack.Seleno.PageObjects;

namespace TestStack.Seleno.Configuration
{
    internal class PageObjectScanner
    {
        private readonly Assembly[] _assembliesToScan;

        public PageObjectScanner(Assembly[] assembliesToScan = null)
        {
            _assembliesToScan = assembliesToScan;
        }

        public Type[] Scan()
        {
            Assembly[] assembliesToScan = _assembliesToScan ?? new[] { Assembly.GetCallingAssembly() };
            return assembliesToScan
                .SelectMany(a => a.GetExportedTypes())
                .Where(IsPageObject)
                .OrderBy(t => t.FullName)
                .ToArray();
        }

        private bool IsPageObject(Type type)
        {
            return type.IsClass && type.IsAbstract == false && typeof(UiComponent).IsAssignableFrom(type);
        }

    }
}