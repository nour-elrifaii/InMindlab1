using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;


namespace WebApplication2.Filters;

public class ValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        foreach (var kvp in context.ActionArguments)
        {
            var name = kvp.Key;
            var value = kvp.Value;
            if (value == null)
            {
                continue; //there is nothing to validate
            }
            
            var validatorType= typeof(IValidator<>).MakeGenericType(value.GetType());
            var validator=context.HttpContext.RequestServices.GetService(validatorType) as IValidator;
            if (validator == null)
            {
                continue; //no registered validator
            }
            
            ValidationResult result = await validator.ValidateAsync(new ValidationContext<object>(value));
            if (!result.IsValid)
            {
                foreach (var failure in result.Errors)
                {
                    context.ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                }
            }
        }
        //in case it contains errors
        if (!context.ModelState.IsValid)
        {
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Title = "Validation error occurred",
                Status = StatusCodes.Status400BadRequest,
                Instance = context.HttpContext.Request.Path
            };
            
            context.Result = new BadRequestObjectResult(problemDetails)
            {
                ContentTypes= { "Application/json" }
            }
            ;
            return;
        }
        await next();
        
    }
}