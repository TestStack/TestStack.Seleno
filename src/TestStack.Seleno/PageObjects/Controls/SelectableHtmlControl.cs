using OpenQA.Selenium;
using TestStack.Seleno.Extensions;

namespace TestStack.Seleno.PageObjects.Controls
{
    public abstract class SelectableHtmlControl : HTMLControl
    {
        public bool HasSelectedElement
        {
            get { return SelectedElement != null; }
        }

        public virtual TProperty SelectedElementAs<TProperty>()
        {
            if (!HasSelectedElement)
            {
                throw new NoSuchElementException("No selected element has been found");
            }

            return SelectedElement.GetControlValueAs<TProperty>();
        }

        public abstract void SelectElement<TProperty>(TProperty value);

        public abstract IWebElement SelectedElement { get; }
    }
}