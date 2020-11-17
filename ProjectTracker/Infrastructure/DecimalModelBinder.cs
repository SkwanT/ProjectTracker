using System;
using System.Globalization;
using System.Web.Mvc;

namespace ProjectTracker.Infrastructure
{
    public class DecimalModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            object result = null;

            string modelName = bindingContext.ModelName;
            string attemptedValue = bindingContext.ValueProvider.GetValue(modelName)?.AttemptedValue;

            if (attemptedValue != null)
            {
                string wantedSeperator = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
                string alternateSeperator = (wantedSeperator == "," ? "." : ",");

                if (attemptedValue.IndexOf(wantedSeperator, StringComparison.Ordinal) == -1
                    && attemptedValue.IndexOf(alternateSeperator, StringComparison.Ordinal) != -1)
                {
                    attemptedValue = attemptedValue.Replace(alternateSeperator, wantedSeperator);
                }

                try
                {
                    if (bindingContext.ModelMetadata.IsNullableValueType && string.IsNullOrWhiteSpace(attemptedValue))
                    {
                        return null;
                    }

                    result = decimal.Parse(attemptedValue, NumberStyles.Any);
                }
                catch (FormatException e)
                {
                    bindingContext.ModelState.AddModelError(modelName, e);
                }
            }

            return result;
        }
    }
}