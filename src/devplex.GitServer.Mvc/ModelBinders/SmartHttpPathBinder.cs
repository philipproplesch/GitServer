using System;
using System.Web.Mvc;

namespace devplex.GitServer.Mvc.ModelBinders
{
    public class SmartHttpPathBinder : IModelBinder
    {
        public object BindModel(
            ControllerContext controllerContext, 
            ModelBindingContext bindingContext)
        {
            var result =
                bindingContext.ValueProvider.GetValue(
                    bindingContext.ModelName);

            if (result != null)
            {
                var value = 
                    result.AttemptedValue;

                var index =
                    value.LastIndexOf(
                        ".git/",
                        StringComparison.Ordinal);
                
                return value.Substring(0, index);
            }

            throw new Exception();
        }
    }
}
