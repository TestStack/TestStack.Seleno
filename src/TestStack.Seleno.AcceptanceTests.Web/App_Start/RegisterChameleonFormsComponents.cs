using ChameleonForms.Attributes;
using ChameleonForms.ModelBinders;
using System;
using System.Linq;
using System.Web.Mvc;

[assembly: WebActivator.PreApplicationStartMethod(typeof(TestStack.Seleno.AcceptanceTests.Web.RegisterChameleonFormsComponents), "Start")]
 
namespace TestStack.Seleno.AcceptanceTests.Web
{
    public static class RegisterChameleonFormsComponents
    {
        public static void Start()
        {
            ModelBinders.Binders.Add(typeof(DateTime), new DateTimeModelBinder());
            ModelBinders.Binders.Add(typeof(DateTime?), new DateTimeModelBinder());
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(RequiredFlagsEnumAttribute), typeof(RequiredAttributeAdapter));
            typeof(RegisterChameleonFormsComponents).Assembly.GetTypes().Where(t => t.IsEnum && t.GetCustomAttributes(typeof(FlagsAttribute), false).Any())
                .ToList().ForEach(t =>
                {
                    ModelBinders.Binders.Add(t, new FlagsEnumModelBinder());
                    ModelBinders.Binders.Add(typeof(Nullable<>).MakeGenericType(t), new FlagsEnumModelBinder());
                });
        }
    }
}
