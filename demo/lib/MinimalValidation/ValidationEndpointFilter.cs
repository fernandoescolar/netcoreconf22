using MiniValidation;

namespace MinimalValidation;

public class ValidationEndpointFilter : IEndpointFilter
{
    public ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        foreach (var argument in context.Arguments)
        {
            if (argument is null) continue;
            if (!argument.GetType().IsClass) continue;
            if (!MiniValidator.TryValidate(argument, out var errors))
            {
                return ValueTask.FromResult<object>(Results.BadRequest(errors));
            }

            break;
        }

        return next(context);
    }
}
