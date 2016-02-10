using System.Web.Mvc;

namespace TestStack.Seleno.Configuration.Fakes
{
    internal class FakeViewDataContainer : IViewDataContainer
    {
        public ViewDataDictionary ViewData { get; set; } = new ViewDataDictionary();
    }
}
