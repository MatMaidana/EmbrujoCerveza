using EmbrujoCerveza.Web.Data;
using EmbrujoCerveza.Web.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();

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
