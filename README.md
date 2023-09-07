# Url Shortener Server
This is url shortener server tht help you to generate a Base62 short url in your .NET application that target .NET 6.0 or .NET 7.0.

## Get Started

### 1- The Basic Installtion:
Create a new Asp.Net Core Empty project that target .NET 6.0 or .NET 7.0.
Second things you have to do is to install the required nuget package.
For any .NET application higher than .Net 6 install the library by using NuGet package command
```
dotnet add package UrlShortenerServer.EntityFramework --version 1.0.0
```
Or by using the nuget package manager from Visual Studio like below

![nuget](https://github.com/Shoogn/UrlShortenerServer/assets/18530495/aa175b58-f32b-47a6-b59c-2ae1d1b971c4)

---
Create a new folder named Models and inside this foler create a new class named BaseDBContext, the signature of this class is like following:

```C#
using Microsoft.EntityFrameworkCore;
using UrlShortener.EntityFramework;

namespace MyUrlShortenerServer.Models
{
    public class BaseDBContext : UrlShortenerDbContext
    {
        public BaseDBContext(DbContextOptions<BaseDBContext> options) : base(options) { }
    }
}
```
As you can see from the previous code our BaseDBContext object inherit from UrlShortenerDbContext object, and this is the main object that will help us to generate our main Table in the back store and this Table by default named {ShortUrls}.
Open the Program.cs class and paste the following code:

```C#
using Microsoft.EntityFrameworkCore;
using MyUrlShortenerServer.Models;
using UrlShortener.Core;
using UrlShortener.EntityFramework;
using UrlShortener.EntityFramework.Store;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<BaseDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("UrlShorterDBConnection"));
});

// Here I'm going to register the url shortener server
builder.Services.AddUrlShortener<ShortUrlEntity>()
    .AddUrlShortenerEntityFrameworkStores<BaseDBContext>();

var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();

app.MapDefaultControllerRoute();
app.Run();
```

The most important part in the previous code is the following section

```C#
builder.Services.AddUrlShortener<ShortUrlEntity>()
    .AddUrlShortenerEntityFrameworkStores<BaseDBContext>();
```
And this is the default configuration to register the UrlShortenerServer to your project. So let us add the migration to the project, make sure to install the

```
Microsoft.EntityFrameworkCore.Tools 7.0.10
```

to be able to add a new migration file, then open the Package Manager Console and run this script:
```
PM> Add-Migration -Name init
```
And by doing so you will get a new crated file in the Solution Explorer named Migrations and you will find a new file name {uniquenumber_init.cs} see the next picture:

![image](https://github.com/Shoogn/UrlShortenerServer/assets/18530495/56dcf55b-0590-4dcb-8cb3-3d0b6802bc7f)

and as you can see from the migration code in the previous code the default schema name is "dbo" and the default table name is "ShortUrls" and there are three Columns comming from the ShortUrlEntity object:
- Id ( the type of this column is bigint, long in C#)
- LongUrl ( the type of this column is nvarchar(450), string in C#)
- ShortUrl ( the type of this column is nvarchar(450), string in C#)
Don't worry I will show you in the second part of this explaination how to custom the UrlShortenerServer.
So, let us run our migration file to create the database by opening the Package Manager Console again and run the following script:
```
PM> Update-Database
```
And by pressing Enter key in your keyboard EF Core will create a new database with the given name from the ConnectionStrings in the appsettings.json file and here is the described picture:

![image](https://github.com/Shoogn/UrlShortenerServer/assets/18530495/481dba1b-8761-4d56-8d53-294db8358a28)

Ok, untile now we prepare our application to work with UrlShortenerServer, but how can we create a new short url?
In the Controllers folder create a new file named HomeController.cs file, we need to inject a new class named: UrlShortenerManager and this is a parameter type class, and accept any object of type ShortUrlEntity, the signature of HomeController class is like following:

```C#
 public class HomeController : Controller
 {
     public readonly UrlShortenerManager<ShortUrlEntity> _manager;
     public HomeController(UrlShortenerManager<ShortUrlEntity> manager)
     {
         _manager = manager;
     }

     public async Task<IActionResult> Index()
     {
         string longUrl = "https://localhost:7031/Home/Data";
         var res = await _manager.CreateAsync(longUrl);
         return View();
     }


     public IActionResult Data()
     {
         return View();
     }
 }
```

From the above code we have method named CreateAsync from the UrlShortenerManager object, and this method take one parameter of type string and this parameter should be your long url
and the return type of calling this method is ShortUrlEntity object. which conatins the Id, ShortUrl, and LongUrl. And from that example our long url is "https://localhost:7031/Home/Data" so we need to create a short url from it.
Later on I will show you how to add your custom properties if your busniess needs that.
Make sure to create two Views one for the Index ActionMethod and the other one for the Data ActionMethod. And then run the app, after that open the Database and open the ShortUrls table data to see the result of calling the CreateAsync method, see the below picture:

![image](https://github.com/Shoogn/UrlShortenerServer/assets/18530495/56361181-07dc-47b5-baf7-149ad11fd2bf)

Wow, that is great.
### How to redirect user to the Long Url?
You can do that by impementing your custom logic when you receving a short url, and  you can call the Redirect method from the controller, but you don't need to do that, thankfully to our UrlShortenerServer for providing us an ready Midlleware.
Open the Program.cs class and add the following one line of code:

```C#
app.UseUrlShortenerRedirection<ShortUrlEntity>();
```

Make sure to add this Middlware after app.UseRouting() method here is the completer code:

```C#
var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.UseUrlShortenerRedirection<ShortUrlEntity>();
app.MapDefaultControllerRoute();
app.Run();
```

Copy the ShortUrl from the ShortUrls table and run the app again and open a new tab in your browser and paste the SHortUrl that you're copied and hit the Enter key in your keyboard:

![image](https://github.com/Shoogn/UrlShortenerServer/assets/18530495/d9b2625a-ee33-47e2-881e-2d8df54103af)

After that your browser will redirect you to the corect Long Url:

![image](https://github.com/Shoogn/UrlShortenerServer/assets/18530495/e4bccb50-9007-4f8b-8e67-6752cbe67011)








