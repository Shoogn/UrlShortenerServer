using Microsoft.EntityFrameworkCore;
using UrlShortener.AspNetCore.Models;
using UrlShortener.Core;
using UrlShortener.EntityFramework;
using UrlShortener.EntityFramework.Store;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BaseDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("BaseDBConnection"));
});
builder.Services.AddUrlShortener<ShortUrlEntity>()
    .AddUrlShortenerEntityFrameworkStores<BaseDbContext>();

var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();

app.UseUrlShortenerRedirection<ShortUrlEntity>();

app.MapDefaultControllerRoute();
app.Run();
