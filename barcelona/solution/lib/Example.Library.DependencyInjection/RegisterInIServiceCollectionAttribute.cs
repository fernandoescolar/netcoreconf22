namespace Example.Library.DependencyInjection;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class RegisterInIServiceCollectionAttribute : Attribute
{
    public RegisterInIServiceCollectionAttribute()
    {
    }

    public RegisterInIServiceCollectionAttribute(Type type)
    {
        Type = type;
    }

    public Type Type { get; protected set; }

    public ServiceLifetime ServiceLifetime { get; set; } = ServiceLifetime.Scoped;
}