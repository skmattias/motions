using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace CsAspnet.Models.Tools
{
    public static class ViewTools
    {
        public static PartialViewResult GetPartialView(string viewName, object model = null)
        {
            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(),
                    new ModelStateDictionary())
                { Model = model };

            var result = new PartialViewResult
            {
                ViewName = viewName,
                ViewData = viewData
            };

            return result;
        }
    }
}