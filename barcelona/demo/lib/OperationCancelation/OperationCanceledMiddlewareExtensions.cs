using Microsoft.AspNetCore.Builder;

namespace OperationCancelation;

public static class OperationCanceledMiddlewareExtensions
{
    public static IApplicationBuilder CatchOperationCanceled(this IApplicationBuilder app)
    {
        app.UseMiddleware<OperationCanceledMiddleware>();
        return app;
    }
}