namespace Simple;

public abstract record Endpoint(EndpointVerb Verb, string Path)
{
    public abstract void Configure(RouteHandlerBuilder builder);
}

public abstract record GetEndpoint(string path) : Endpoint(EndpointVerb.Get, path)
{
}

public abstract record PostEndpoint(string path) : Endpoint(EndpointVerb.Post, path)
{
}

public abstract record PutEndpoint(string path) : Endpoint(EndpointVerb.Put, path)
{
}

public abstract record DeleteEndpoint(string path) : Endpoint(EndpointVerb.Delete, path)
{
}

public abstract record PatchEndpoint(string path) : Endpoint(EndpointVerb.Patch, path)
{
}

public abstract record HeadEndpoint(string path) : Endpoint(EndpointVerb.Head, path)
{
}

public abstract record OptionsEndpoint(string path) : Endpoint(EndpointVerb.Options, path)
{
}

public abstract record TraceEndpoint(string path) : Endpoint(EndpointVerb.Trace, path)
{
}
