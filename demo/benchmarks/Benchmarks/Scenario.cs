namespace Benchmarks;

[MemoryDiagnoser]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class Scenario
{
    private readonly Random _rnd = new Random();
    private HttpClient _mvcClient;
    private HttpClient _minimalClient;

    private HttpClient _simpleMinimalClient;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _mvcClient = CreateClientFor(typeof(MvcApi.Model.PagedList<>).Assembly);
        _minimalClient = CreateClientFor(typeof(MinimalApi.Api.PagedList<>).Assembly);
        _simpleMinimalClient = CreateClientFor(typeof(SimpleMinimalApi.PagedList<>).Assembly);
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Get")]
    public Task Get_Mvc() => GetOperation(_mvcClient);

    [Benchmark]
    [BenchmarkCategory("Get")]
    public Task Get_Minimal() => GetOperation(_minimalClient);

    [Benchmark]
    [BenchmarkCategory("Get")]
    public Task Get_Simple() => GetOperation(_simpleMinimalClient);

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Hypermedia")]
    public Task Hypermedia_Mvc() => HypermediaOperation(_mvcClient);

    [Benchmark]
    [BenchmarkCategory("Hypermedia")]
    public Task Hypermedia_Minimal() => HypermediaOperation(_minimalClient);

    [Benchmark]
    [BenchmarkCategory("Hypermedia")]
    public Task Hypermedia_Simple() => HypermediaOperation(_simpleMinimalClient);

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Complex")]
    public Task Complex_Mvc() => ComplexOperation(_mvcClient);

    [Benchmark]
    [BenchmarkCategory("Complex")]
    public Task Complex_Minimal() => ComplexOperation(_minimalClient);

     [Benchmark]
    [BenchmarkCategory("Complex")]
    public Task Complex_Simple() => ComplexOperation(_simpleMinimalClient);

    private async Task GetOperation(HttpClient client)
    {
        client.DefaultRequestHeaders.Remove("Accept");
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        var response = await client.GetAsync("/beers?pageSize=1");
        response.EnsureSuccessStatusCode();
        await response.Content.ReadAsStringAsync();
    }

    private async Task HypermediaOperation(HttpClient client)
    {
        client.DefaultRequestHeaders.Remove("Accept");
        client.DefaultRequestHeaders.Add("Accept", "application/vnd.hypermedia+json");
        var response = await client.GetAsync("/beers?pageSize=100");
        response.EnsureSuccessStatusCode();
        await response.Content.ReadAsStringAsync();
    }

    private async Task ComplexOperation(HttpClient client)
    {
        client.DefaultRequestHeaders.Remove("Accept");
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        var response = await client.PostAsync("/breweries/", new StringContent($@"{{""name"":""Test {_rnd.NextDouble()}"",""city"":""City"",""country"":""Country""}}", Encoding.UTF8, "application/json"));
        response.EnsureSuccessStatusCode();
        await response.Content.ReadAsStringAsync();
        var hashedid = response.Headers.Location.ToString().Split('/').Last();
        response = await client.PutAsync($"/breweries/{hashedid}", new StringContent($@"{{""name"":""Test {_rnd.NextDouble()}"",""city"":""City"",""country"":""Country""}}", Encoding.UTF8, "application/json"));
        response.EnsureSuccessStatusCode();
        await response.Content.ReadAsStringAsync();
        response = await client.DeleteAsync($"/breweries/{hashedid}");
        response.EnsureSuccessStatusCode();
        await response.Content.ReadAsStringAsync();
    }

    private static HttpClient CreateClientFor(Assembly assembly)
    {
        var programType = assembly.GetTypes().Single(t => t.Name == "Program");
        var factoryType = typeof(WebApplicationFactory<>).MakeGenericType(programType);
        var factory = Activator.CreateInstance(factoryType, new object[] {});
        var createClientMethod = factoryType.GetMethods().First(m => m.Name == "CreateClient");
        return (HttpClient)createClientMethod.Invoke(factory, null);
    }
}
