using CatalogoApp.Application.Services;
using CatalogoApp.Domain.Interfaces;
using CatalogoApp.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Ruta del JSON de Items
var jsonPath = Path.Combine(
    builder.Environment.ContentRootPath,
    "data",
    "items.json"
);

// Ruta del JSON de Reviews
var reviewsPath = Path.Combine(
    builder.Environment.ContentRootPath,
    "data",
    "Reviews.json"
);

// Registrar repositorio de Items
builder.Services.AddSingleton<IItemRepository>(
    new JsonItemRepository(jsonPath)
);

// Registrar repositorio de Reviews
builder.Services.AddSingleton<IReviewRepository>(
    new JsonReviewRepository(reviewsPath)
);

// Registrar servicios
builder.Services.AddScoped<ItemService>();
builder.Services.AddScoped<IReviewService, ReviewService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();