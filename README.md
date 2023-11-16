# Url Shortener Server
This is url shortener server that help you to generate a Base62 short url in your .NET application that target .NET 6.0 or .NET 7.0.

## Get Started

### 1- The Basic Installtion:
Create a new Asp.Net Core Empty project that target .NET 6.0 or .NET 7.0.
Second things you have to do is to install the required nuget package, install the library by using NuGet package command
```
dotnet add package UrlShortenerServer.Core --version 1.2.0
```
Or by using the nuget package manager from Visual Studi:

![nuget](https://github.com/Shoogn/UrlShortenerServer/assets/18530495/aa175b58-f32b-47a6-b59c-2ae1d1b971c4)

---
Create a new folder named Models and inside this foler create a new class named BaseDBContext, the signature of this class it looks like the following:

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

The most important part in the previous code from the Program.cs file is the following section:

```C#
builder.Services.AddUrlShortener<ShortUrlEntity>()
    .AddUrlShortenerEntityFrameworkStores<BaseDBContext>();
```
And this is the default configuration to register the UrlShortenerServer to your project. So let us add the migration to the project, make sure to install the required package to be able to create a new migration file:

```
Microsoft.EntityFrameworkCore.Tools 7.0.10
```

after that open the Package Manager Console and run this script:
```
PM> Add-Migration -Name init
```
And by doing so you will get a new created file in the Solution Explorer named Migrations and inside that folder you will find a new file nameed {uniquenumber_init.cs} see the next picture:

![image](https://github.com/Shoogn/UrlShortenerServer/assets/18530495/56dcf55b-0590-4dcb-8cb3-3d0b6802bc7f)

and as you can see from the migration code in the previous picture the default schema name is "dbo" and the default table name is "ShortUrls" and there are three Columns which are comming from the ShortUrlEntity object:
- Id ( the type of this column is bigint, long in C#).
- LongUrl ( the type of this column is nvarchar(450), string in C#).
- ShortUrl ( the type of this column is nvarchar(450), string in C#).

Don't worry I will show you in the second part of this explaination how to customize the UrlShortenerServer.
So, let us run our migration file to create the database by opening the Package Manager Console again and run the following script:
```
PM> Update-Database
```
And by pressing Enter key in your keyboard EF Core will create a new database with the given name that is defined in the ConnectionStrings in the appsettings.json file and here is the described picture:

![image](https://github.com/Shoogn/UrlShortenerServer/assets/18530495/481dba1b-8761-4d56-8d53-294db8358a28)

Ok, untile now we prepare our application to work with UrlShortenerServer, but how can we create a new short url?
In the Controllers folder create a new file named HomeController.cs file, we need to inject a new class named: UrlShortenerManager and this is a parameter type class, and accept any object of type ShortUrlEntity, the signature of HomeController class it looks like the following:

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

From the above code we have a method named *CreateAsync* from the UrlShortenerManager object, and this method accept one parameter of type string and this parameter should be your long url
and the return type of this method is *ShortUrlEntity* object. which conatins the Id, ShortUrl, and LongUrl properties. And from that example our long url is "https://localhost:7031/Home/Data" so we need to create a short url from it.
Later on I will show you how to add your custom properties if your busniess needs that.
Make sure to create two Views one for the Index ActionMethod and the other one for the Data ActionMethod. And then run the app, after that open the Database and open the ShortUrls table data to see the result of calling the CreateAsync method, see the below picture:

![image](https://github.com/Shoogn/UrlShortenerServer/assets/18530495/56361181-07dc-47b5-baf7-149ad11fd2bf)

Wow, that is great.

### How to redirect user to the Long Url?
---

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

Copy the ShortUrl from the ShortUrls table and run the app again and open a new tab in your browser and paste the SHortUrl that you have copied and hit the Enter key in your keyboard:

![image](https://github.com/Shoogn/UrlShortenerServer/assets/18530495/d9b2625a-ee33-47e2-881e-2d8df54103af)

After that your browser will redirect you to the corect Long Url:

![image](https://github.com/Shoogn/UrlShortenerServer/assets/18530495/e4bccb50-9007-4f8b-8e67-6752cbe67011)

### 2- Customize the UrlShortenerServer:
---
If you are looking carefully to the short url that is generated is the previous stpes you can see 'sh' letters that append next to the domain name, by default the UrlShortenerServer will append these letters to match short url to the correct long url in the back store (SQL Server for example.). In this section I'm going to show you how to customize the UrlShortenerServer.
From the Package Manager Console run the following script:

```C#
PM> Drop-Database
```
Create a new class named CustomShortUrlsEntity and make sure to let this class inherit from ShortUrlEntity see the code below:

```C#
using UrlShortener.EntityFramework.Store;

namespace MyUrlShortenerServer.Models
{
    /// <summary>
    /// A new custom object that inherent from <see cref="ShortUrlEntity"/>
    /// </summary>
    public class CustomShortUrlsEntity : ShortUrlEntity
    {
        /// <summary>
        /// This is a custom property
        /// </summary>
        public string? CustomeData { get; set; }
    }
}
```
Then open the BaseDBContext.cs class to update it as follow:

```C#
using Microsoft.EntityFrameworkCore;
using UrlShortener.EntityFramework;

namespace MyUrlShortenerServer.Models
{
    public class BaseDBContext : UrlShortenerDbContext<CustomShortUrlsEntity>
    {
        public BaseDBContext(DbContextOptions<BaseDBContext> options) : base(options) { }
    }
}
```

If you look by opening eyes you will see that the UrlShortenerDbContext object is accept a type parameter, and the type here should be (a) ShortUrlEntity object, and the CustomShortUrlsEntity object is a valid ShortUrlEntity object because it inherit from it. 

Open the Program.cs file and change the default setting of the Url Shortener to be like so:

```C#
builder.Services.AddUrlShortener<CustomShortUrlsEntity>(options =>
{
    options.ShortUrlPrefix = "/cu";
    options.UsePermanentRedirect = true;
    options.DomainName = "";
    options.UseUrlShortenerInTheSameDomain = true; // by default is true

}).AddUrlShortenerEntityFrameworkStores<BaseDBContext>(options =>
{
    options.ShortUrlMaxLength = 50;
    options.LongUrlMaxLength = 100;
    options.ShemaName = "common";
    options.TableName = "CustomShortUlrs";
});

```

From the above code we configure the UrlShortenerServer to accept our custom behaviour. The AddUrlShortener extensiom method accept a nullable Action<UrlShortenerOptions> parameter,
that let you change the Short Url Prefix, as well as the  Permanent Redirect type, (301 or 302) by default is Permanent, and there are two critical properties, the first one is DomainName and and the second one is UseUrlShortenerInTheSameDomain,
the UseUrlShortenerInTheSameDomain property is a boolean, and by default is equal to true and that means you are going to use the UrlShortenerServer in your current application and for that reason the UrlShortenerServer needs to set a ShortUrlPrefix value to differentiate between the short url and the normal url, and for the DomainName property, if it is not null or empty or whitespace (if it has a value - it should be a valid URI) so that means you are going to use the UrlShortenerServer in a **sperate** domain then, when the UrlShortenerServer generate a short url will not append the ShortUrlPrefix value for that short url instead of that it take the name of the domain that is provide in DomainName property and then append the shorturl (Base62) to the DomainName, but you have to make sure by to assign the value of the UseUrlShortenerInTheSameDomain property to false.
Now let us add a new Migration file by opening the Package Manager Console and run the following script:

```
PM> Add-Migration -Name init
```

and updating the database as well by applying the following script:

```
PM> Update-Database
```

If you looking at the generated migration file you will see somthing similar to the following:

![image](https://github.com/Shoogn/UrlShortenerServer/assets/18530495/bc8e9765-5ae4-4ad3-ba65-cf69d03a3d49)

As you can see the schema name is typical to the one that we provide in our AddUrlShortenerEntityFrameworkStores configuration also the name of the table is changed to CustomShortUlrs instead of the default one (ShortUrls) also the length of the LongUrl and ShortUrl is changed to (100/50) respectfully isntead of the default one (450) and the last things our custom CustomeData property is added to file as well.

Before running app make sure to change to prameter type of the UrlShortenerManager to our new CustomShortUrlsEntity object:

```C#
       public readonly UrlShortenerManager<CustomShortUrlsEntity> _manager;
       public HomeController(UrlShortenerManager<CustomShortUrlsEntity> manager)
       {
           _manager = manager;
       }
```
Fire the app now and open the table in your back store ( in my case is SQL Server):

![image](https://github.com/Shoogn/UrlShortenerServer/assets/18530495/823d21fa-6d0a-4e4a-95a8-d854c558041f)

Copy the Short Url and paste it in a new tab in your browser and then hit Enter key in your keyboard, then the browser will redirect you to the long url without any issues.
Thenks for reading!

