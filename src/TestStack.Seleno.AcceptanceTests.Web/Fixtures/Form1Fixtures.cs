using System;
using TestStack.Seleno.AcceptanceTests.Web.ViewModels;

namespace TestStack.Seleno.AcceptanceTests.Web.Fixtures
{
    public static class Form1Fixtures
    {
        public static Form1ViewModel A
        {
            get
            {
                return new Form1ViewModel
                {
                    RequiredString = "RequiredString",
                    RequiredInt = 1,
                    OptionalInt = null,
                    RequiredBool = true,
                    RequiredNullableBool = false,
                    OptionalBool = true,
                    OptionalBool2 = false,
                    OptionalBool3 = null,
                    RequiredEnum = SomeEnum.Value2,
                    RequiredNullableEnum = SomeEnum.Value1,
                    OptionalEnum = null,
                    TextAreaField = string.Format("Some text in line 1.{0}More Text in another line 2.{0}Some more text in line 3.{0}And finally last line 4.", Environment.NewLine),
                    /*RequiredEnums = new List<SomeEnum> { SomeEnum.Value2, SomeEnum.Value3 },
                    RequiredNullableEnums = new List<SomeEnum?> { SomeEnum.Value1 },
                    OptionalEnums = null,
                    OptionalNullableEnums = new List<SomeEnum?>(),*/
                    RequiredListId = 3
                };
            }
        }
    }
}