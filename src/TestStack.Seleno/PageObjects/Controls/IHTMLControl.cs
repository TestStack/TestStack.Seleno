namespace TestStack.Seleno.PageObjects.Controls
{
    public interface IHTMLControl
    {
        string Id { get; }
        string Name { get; }
        //string Title { get; set; }
        //bool IsReadOnly {get;set;}
        //bool IsDisabled {get;set;}

        
        
        TReturn AttributeValueAs<TReturn>(string attributeName);
    }
}