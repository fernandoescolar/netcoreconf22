using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Data;

public static class BeerDbExtensions
{
    public static IServiceCollection AddBeerDbContext(this IServiceCollection serviceCollection, string sqlServerConnectionString = null)
    {
        if (string.IsNullOrWhiteSpace(sqlServerConnectionString))
        {
            serviceCollection.AddDbContext<BeerDbContext>(options =>
                options.UseSqlite("Data Source=beers.db"));
        }
        else
        {
            serviceCollection.AddDbContext<BeerDbContext>(options =>
                options.UseSqlServer(sqlServerConnectionString));
        }

        return serviceCollection;
    }

    public static void SeedBeerDbData(this IHost app)
    {
        var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

        using (var scope = scopedFactory.CreateScope())
        {
            var db = scope.ServiceProvider.GetService<BeerDbContext>();
            db.Database.EnsureCreated();
            if (db.Beers.Any())
            {
                return;
            }

            db.Database.OpenConnection();
            try
            {
                if (db.Database.IsSqlServer())
                {
                    db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Styles ON;");
                }

                db.Styles.AddRange(Seed.BeerStyles);
                db.SaveChanges();

                if (db.Database.IsSqlServer())
                {
                    db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Styles OFF;");
                    db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Breweries ON;");
                }

                db.Breweries.AddRange(Seed.Breweries);
                db.SaveChanges();

                if (db.Database.IsSqlServer())
                {
                    db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Breweries OFF;");
                    db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Beers ON;");
                }

                db.Beers.AddRange(Seed.Beers);
                db.SaveChanges();

                if (db.Database.IsSqlServer())
                {
                    db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Beers OFF;");
                }
            }
            finally
            {
                db.Database.CloseConnection();
            }
        }
    }
}
