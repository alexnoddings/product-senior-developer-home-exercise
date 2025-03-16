using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace UKParliament.CodeTest.Web;

// Note that while FluentValidation provides a package for auto-validating ASP.NET Core's MVC,
// it is no longer supported, and they recommend using against it:
// https://github.com/FluentValidation/FluentValidation/issues/1959
public class ValidateAttribute<T> : ActionFilterAttribute where T : class
{
    private readonly string _argumentName;

    public ValidateAttribute(string argumentName)
    {
        _argumentName = argumentName;
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var parameter = context.ActionDescriptor.Parameters.FirstOrDefault(p => p.Name == _argumentName);
        if (parameter is null)
            throw new InvalidOperationException($"Parameter {_argumentName} not found on action '{context.ActionDescriptor.DisplayName}'.");
        
        if (!parameter.ParameterType.IsAssignableTo(typeof(T)))
            throw new InvalidOperationException($"Parameter '{_argumentName}' must of type '{typeof(T)}' on action '{context.ActionDescriptor.DisplayName}'.");
        
        var valueObject = context.ActionArguments[parameter.Name];
        if (valueObject is null)
        {
            var validationProblemDetails = new ValidationProblemDetails { Detail = $"No '{_argumentName}' provided." };
            context.Result = new BadRequestObjectResult(validationProblemDetails);
            return;
        }

        // This check should never fail, the IsAssignableTo check above should prevent it
        if (valueObject is not T value)
            throw new InvalidOperationException($"Argument '{_argumentName}' was not a valid '{typeof(T)}'.");

        var isValid = await TryValidateAsync(context, value);
        if (isValid)
            await next();
    }

    private static async Task<bool> TryValidateAsync(ActionExecutingContext context, T value)
    {
        var validator = context.HttpContext.RequestServices.GetRequiredService<IValidator<T>>();
        var validationResult = await validator.ValidateAsync(value);
        if (validationResult.IsValid)
            return true;

        var errors = validationResult
            .Errors
            .GroupBy(static e => e.PropertyName)
            .ToDictionary(
                static e => e.Key,
                static e => e.Select(static i => i.ErrorMessage).ToArray()
            );
        
        var validationProblemDetails = new ValidationProblemDetails(errors);
        context.Result = new BadRequestObjectResult(validationProblemDetails);
        return false;
    }
}
