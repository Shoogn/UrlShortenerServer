using Microsoft.EntityFrameworkCore;
using UrlShortener.AspNetCore.Custom.Models;
using UrlShortener.Core;
using UrlShortener.EntityFramework;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BaseDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("BaseDBConnection"));
});
builder.Services.AddUrlShortener<CustomShortUrlEntity>(options =>
{
    options.ShortUrlPrefix = "/cu";
}).AddUrlShortenerEntityFrameworkStores<BaseDbContext>(options =>
{
    options.ShortUrlMaxLength = 100;
    options.LongUrlMaxLength = 100;
    options.ShemaName = "common";
    options.TableName = "CustomShortUrls";
});

var app = builder.Build();

app.UseUrlShortenerRedirection<CustomShortUrlEntity>();
app.MapGet("/", async (UrlShortenerManager<CustomShortUrlEntity> manager) =>
{
    string longUrl = "https://localhost:7045/Home/Data";
    var shortUrl = new CustomShortUrlEntity
    {
        AddedDate = DateTime.Now
    };
    var result = await manager.CreateAsync(shortUrl, longUrl);
});

app.MapGet("/Home/Data", () =>
{
   return "This is the redirect url result.";
});

app.Run();
