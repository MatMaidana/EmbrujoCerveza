using EmbrujoCerveza.Web.Data;
using EmbrujoCerveza.Web.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

var rawConnectionString = builder.Configuration["DATABASE_URL"]
    ?? builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("No se encontró la cadena de conexión a la base de datos.");

var connectionString = NormalizeConnectionString(rawConnectionString);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();

    if (!context.BeerStyles.Any())
    {
        context.BeerStyles.AddRange(
            new BeerStyle
            {
                Name = "IPA",
                Description = "India Pale Ale con notas cítricas y amargor medio-alto.",
                Abv = 6.2m,
                Ibu = 55
            },
            new BeerStyle
            {
                Name = "Stout de avena",
                Description = "Cerveza oscura y cremosa con sabores a café y chocolate.",
                Abv = 5.8m,
                Ibu = 35
            });

        context.SaveChanges();
    }

    if (!context.BottleTypes.Any())
    {
        context.BottleTypes.AddRange(
            new BottleType
            {
                Material = "Vidrio ámbar",
                CapacityMl = 330,
                Description = "Botella estándar ideal para estilos lupulados."
            },
            new BottleType
            {
                Material = "Lata de aluminio",
                CapacityMl = 473,
                Description = "Formato práctico para lotes pequeños."
            },
            new BottleType
            {
                Material = "Vidrio ámbar",
                CapacityMl = 500,
                Description = "Presentación clásica para stouts y cervezas especiales."
            });

        context.SaveChanges();
    }

    if (!context.BeerLots.Any())
    {
        var ipa = context.BeerStyles.FirstOrDefault(s => s.Name == "IPA");
        var stout = context.BeerStyles.FirstOrDefault(s => s.Name == "Stout de avena");
        var bottle330 = context.BottleTypes.FirstOrDefault(b => b.CapacityMl == 330);
        var can473 = context.BottleTypes.FirstOrDefault(b => b.CapacityMl == 473);

        if (ipa != null && stout != null && bottle330 != null && can473 != null)
        {
            context.BeerLots.AddRange(
                new BeerLot
                {
                    BeerStyleId = ipa.Id,
                    BottleTypeId = bottle330.Id,
                    BottleCount = 120,
                    BottledOn = DateTime.Today.AddDays(-10),
                    Notes = "Lote principal de temporada."
                },
                new BeerLot
                {
                    BeerStyleId = stout.Id,
                    BottleTypeId = can473.Id,
                    BottleCount = 60,
                    BottledOn = DateTime.Today.AddDays(-3),
                    Notes = "Producción limitada."
                });

            context.SaveChanges();
        }
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

app.Run();

static string NormalizeConnectionString(string connectionString)
{
    if (string.IsNullOrWhiteSpace(connectionString))
    {
        throw new InvalidOperationException("La cadena de conexión proporcionada está vacía.");
    }

    if (connectionString.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase) ||
        connectionString.StartsWith("postgresql://", StringComparison.OrdinalIgnoreCase))
    {
        var databaseUri = new Uri(connectionString);
        var userInfo = databaseUri.UserInfo.Split(':', 2);

        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = databaseUri.Host,
            Port = databaseUri.Port != -1 ? databaseUri.Port : 5432,
            Username = Uri.UnescapeDataString(userInfo[0]),
            Password = userInfo.Length > 1 ? Uri.UnescapeDataString(userInfo[1]) : string.Empty,
            Database = databaseUri.AbsolutePath.Trim('/'),
            SslMode = SslMode.Prefer,
            TrustServerCertificate = true
        };

        return builder.ConnectionString;
    }

    return connectionString;
}
