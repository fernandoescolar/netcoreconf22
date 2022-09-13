namespace MinimalApi.Api.BeerStyles.List;

public class BeerStyleDetailsHypermediaProvider : HypermediaProvider<IEnumerable<BeerStyleDetail>>
{
    protected override IEnumerable<HypermediaLink> GetLinksFor(IEnumerable<BeerStyleDetail> @object)
    {
        yield return new HypermediaLink("self", "/styles", "GET");
    }
}
