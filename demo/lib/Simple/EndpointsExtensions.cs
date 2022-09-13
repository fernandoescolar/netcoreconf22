using System.Linq.Expressions;
using System.Reflection;

namespace Simple;

public static class EndpointsExtensions
{
    public static RouteGroupBuilder MapEndpoints(this IEndpointRouteBuilder endpoints, Assembly assembly = null)
    {
        var group = endpoints.MapGroup("/");
        foreach (var t in GetEndpoints(assembly ?? Assembly.GetExecutingAssembly()))
        {
            group.MapEndpoint(t);
        }

        return group;
    }

    private static RouteHandlerBuilder MapEndpoint(this RouteGroupBuilder builder, Type type)
    {
        if (!type.GetConstructors().Any(c => c.GetParameters().Length == 0))
        {
            throw new InvalidOperationException($"Endpoint type {type.FullName} must have a parameterless constructor.");
        }

        var endpoint = Activator.CreateInstance(type, new object[] { }) as Endpoint;
        var delegateMethod = type.GetMethods().Where(m => m.Name == "Handle" || m.Name == "HandleAsync").FirstOrDefault();
        if (delegateMethod == null)
        {
            throw new Exception($"Endpoint {type.Name} does not have a Handle or HandleAsync method.");
        }

        var @delegate = CreateDelegate(endpoint, delegateMethod);
        var routeHanderBuilder = endpoint.Verb switch
        {
            EndpointVerb.Get => builder.MapGet(endpoint.Path, @delegate),
            EndpointVerb.Post => builder.MapPost(endpoint.Path, @delegate),
            EndpointVerb.Put => builder.MapPut(endpoint.Path, @delegate),
            EndpointVerb.Delete => builder.MapDelete(endpoint.Path, @delegate),
            EndpointVerb.Patch => builder.MapPatch(endpoint.Path, @delegate),
            // EndpointVerb.Head => builder.MapHead(endpoint.Path, @delegate),
            // EndpointVerb.Options => builder.MapOptions(endpoint.Path, @delegate),
            // EndpointVerb.Trace => builder.MapTrace(endpoint.Path, @delegate),
            _ => throw new Exception($"Endpoint {type.Name} has an invalid verb."),
        };

        endpoint.Configure(routeHanderBuilder);

        return routeHanderBuilder;
    }

    private static IEnumerable<Type> GetEndpoints(Assembly assembly)
    {
        var types = assembly.GetTypes();
        var endpoints = types.Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(Endpoint)));
        return endpoints;
    }

    public static Delegate CreateDelegate(object target, MethodInfo methodInfo)
    {
        Func<Type[], Type> getType;
        var isAction = methodInfo.ReturnType.Equals((typeof(void)));
        var types = methodInfo.GetParameters().Select(p => p.ParameterType);
        if (isAction)
        {
            getType = Expression.GetActionType;
        }
        else
        {
            getType = Expression.GetFuncType;
            types = types.Concat(new[] { methodInfo.ReturnType });
        }

        if (methodInfo.IsStatic)
        {
            return Delegate.CreateDelegate(getType(types.ToArray()), methodInfo);
        }

        return Delegate.CreateDelegate(getType(types.ToArray()), target, methodInfo.Name);
    }
}
