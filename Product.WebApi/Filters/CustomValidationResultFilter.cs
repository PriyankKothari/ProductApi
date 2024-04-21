using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ProductApi.WebApi.Interceptors
{
    internal sealed class CustomValidationResultFilter : IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {

        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var validationErrorMessages = new Dictionary<string, string[]>(context.ModelState.Count);
                
                foreach (var keyModelStatePair in context.ModelState)
                {
                    var key = keyModelStatePair.Key;
                    var validationErrors = keyModelStatePair.Value.Errors;
                    
                    if (validationErrors is not null && validationErrors.Any())
                    {
                        validationErrorMessages.Add(key, validationErrors.Select(error => error.ErrorMessage).ToArray());
                    }
                }

                context.Result = new BadRequestObjectResult(new { ValidationErrorMessages = validationErrorMessages });
            }
        }
    }
}