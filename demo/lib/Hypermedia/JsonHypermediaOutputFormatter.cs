using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Hypermedia;

public sealed class JsonHypermediaOutputFormatter : TextOutputFormatter
{
    public JsonHypermediaOutputFormatter(JsonSerializerOptions jsonSerializerOptions)
    {
        SupportedEncodings.Add(Encoding.UTF8);
        SupportedMediaTypes.Add(HypermediaConstants.ContentType);
    }

    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
    {
        var httpContext = context.HttpContext;
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var hypermedia = context.HttpContext.RequestServices.GetService(typeof(HypermediaConverter)) as HypermediaConverter;
        if (hypermedia is null)
        {
            throw new InvalidOperationException("Unable to resolve HypermediaConverter");
        }

        var result = hypermedia.Convert(context.Object);
        HypermediaLinkHelper.FullfillLinkUrls(result, httpContext);

        var responseStream = httpContext.Response.Body;
        await JsonSerializer.SerializeAsync(responseStream, result, result.GetType(), jsonSerializerOptions, httpContext.RequestAborted);
        await responseStream.FlushAsync(httpContext.RequestAborted);
    }
}
