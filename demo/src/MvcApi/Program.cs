var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.MapType<HashedId>(() => new OpenApiSchema { Type = "string" }));

builder.Services.AddBeerDbContext();
builder.Services.AddHypermedia();
builder.Services.AddHashedIds(op => op.Passphrase = "my secret passphrase");
builder.Services.AddControllers(options => options.ModelBinderProviders.AddHashedIds());

var app = builder.Build();

app.SeedBeerDbData();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
