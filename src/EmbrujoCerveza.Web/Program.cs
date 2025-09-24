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
                Ibu = 55,
                Stock = 120
            },
            new BeerStyle
            {
                Name = "Stout de avena",
                Description = "Cerveza oscura y cremosa con sabores a café y chocolate.",
                Abv = 5.8m,
                Ibu = 35,
                Stock = 80
            });

        context.SaveChanges();
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
