using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ChameleonForms.Attributes;

namespace TestStack.Seleno.AcceptanceTests.Web.ViewModels
{
    public class Form1ViewModel
    {
        public Form1ViewModel()
        {
            List = new List<ListItem> { new ListItem { Id = 1, Name = "A" }, new ListItem { Id = 2, Name = "B" }, new ListItem { Id = 3, Name = "C" } };
        }

        [Required]
        public string RequiredString { get; set; }

        public int RequiredInt { get; set; }
        public int? OptionalInt { get; set; }

        public bool RequiredBool { get; set; }
        [Required]
        public bool? RequiredNullableBool { get; set; }
        public bool? OptionalBool { get; set; }
        public bool? OptionalBool2 { get; set; }
        public bool? OptionalBool3 { get; set; }

        public SomeEnum RequiredEnum { get; set; }
        [Required]
        public SomeEnum? RequiredNullableEnum { get; set; }
        public SomeEnum? OptionalEnum { get; set; }

        /*[Required]
        public IEnumerable<SomeEnum> RequiredEnums { get; set; }
        [Required]
        public IEnumerable<SomeEnum?> RequiredNullableEnums { get; set; }
        public IEnumerable<SomeEnum> OptionalEnums { get; set; }
        public IEnumerable<SomeEnum?> OptionalNullableEnums { get; set; }*/

        [ReadOnly(true)]
        public List<ListItem> List { get; set; }
        [ExistsIn("List", "Id", "Name")]
        public int RequiredListId { get; set; }
    }

    public class ListItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public enum SomeEnum
    {
        Value1,
        Value2,
        Value3
    }
}