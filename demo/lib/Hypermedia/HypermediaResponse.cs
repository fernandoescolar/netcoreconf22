namespace Hypermedia;

public record HypermediaResponse(IEnumerable<HypermediaLink> Links);
