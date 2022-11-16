var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBeerDbContext();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.MapType<HashedId>(() => new OpenApiSchema { Type = "string" }));
builder.Services.AddHashedIds(op => op.Passphrase = "my secret passphrase");
builder.Services.AddHypermedia();

var app = builder.Build();

app.SeedBeerDbData();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.CatchOperationCanceled();

app.MapGroup("/")
   .MapBeersApi()
   .WithHypermedia()
   .WithTags("Beers");

app.MapGroup("/")
   .MapBeerStylesApi()
   .WithHypermedia()
   .WithTags("Styles");

app.MapGroup("/")
   .MapBreweriesApi()
   .WithHypermedia()
   .WithTags("Breweries");

app.Run();
