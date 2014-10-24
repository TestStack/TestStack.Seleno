using TestStack.Seleno.AcceptanceTests.Web.Fixtures;
using TestStack.Seleno.PageObjects;

namespace TestStack.Seleno.AcceptanceTests.PageObjects
{
    public class DetailsPage : Page<StudentViewModel>
    {
        public StudentViewModel ReadModel()
        {
            return Read.ModelFromPage();
        }
    }
}