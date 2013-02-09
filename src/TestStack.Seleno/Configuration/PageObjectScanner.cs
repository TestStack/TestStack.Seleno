using System;
using System.Collections.Generic;
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
            Assembly[] assembliesToScan = _assembliesToScan ?? GetAssembliesToScan();
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

        private Assembly[] GetAssembliesToScan()
        {
            var excludedAssemblies = new List<string>(new[]
            {
                // note comma after TestStack.Seleno so as to still include TestStack.Seleno.*
                "System", "mscorlib", "TestStack.Seleno,", "TestStack.BDDfy", "WebDriver", "TestDriven", "JetBrains.ReSharper",
                "nunit", "xunit", "DynamicProxy", "NSubstitute", "Autofac"
            });

            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => !excludedAssemblies.Any(ex => assembly.GetName().FullName.StartsWith(ex)))
                .ToArray();
        }
    }
}