using System.ComponentModel;
using System.Web.Mvc;

namespace OsosOracle.MvcUI.Infrastructure
{
    public class NoValidationDefaultModelBinder : System.Web.Mvc.DefaultModelBinder
    {
        protected override void OnPropertyValidated(ControllerContext controllerContext, System.Web.Mvc.ModelBindingContext bindingContext,
            PropertyDescriptor propertyDescriptor, object value)
        {
            bindingContext.ModelState.Clear();
        }
    }
}