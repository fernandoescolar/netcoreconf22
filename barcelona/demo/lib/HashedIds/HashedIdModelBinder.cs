namespace HashedIds;

public class HashedIdModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        ArgumentNullException.ThrowIfNull(bindingContext);

        var modelName = bindingContext.ModelName;
        var providedValue = bindingContext.ValueProvider.GetValue(modelName);
        if (providedValue == ValueProviderResult.None)
        {
            return Task.CompletedTask;
        }

        bindingContext.ModelState.SetModelValue(modelName, providedValue);

        var value = providedValue.FirstValue;
        if (string.IsNullOrEmpty(value))
        {
            return Task.CompletedTask;
        }

        if (!HashedId.TryParse(value, out var result))
        {
            bindingContext.ModelState.TryAddModelError(modelName, "Invalid HashedId value.");
            return Task.CompletedTask;
        }

        bindingContext.Result = ModelBindingResult.Success(result);
        return Task.CompletedTask;
    }
}
