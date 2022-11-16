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

app.MapEndpoints(typeof(PagedList<>).Assembly)
   .WithHypermedia();

app.Run();
