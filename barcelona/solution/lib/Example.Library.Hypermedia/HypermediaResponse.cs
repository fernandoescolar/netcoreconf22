namespace Example.Library.Hypermedia;

public record HypermediaResponse(IEnumerable<HypermediaLink> Links);
