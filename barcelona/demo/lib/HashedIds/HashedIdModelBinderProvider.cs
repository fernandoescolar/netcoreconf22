namespace HashedIds;

public class HashedIdModelBinderProvider : IModelBinderProvider
{
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (context.Metadata.ModelType != typeof(HashedId))
        {
            return null;
        }

        return new HashedIdModelBinder();
    }
}
