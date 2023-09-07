using UrlShortener.EntityFramework.Store;

namespace UrlShortener.AspNetCore.Models
{
    public class CustomShortUrlEntity : ShortUrlEntity
    {
        public string CustomProperty { get; set; } = default!;
    }
}
