namespace TestStack.Seleno.PageObjects.Controls
{
    public interface IHTMLControl
    {
        string Id { get; }

        string Name { get; }
        
        TReturn AttributeValueAs<TReturn>(string attributeName);
    }
}