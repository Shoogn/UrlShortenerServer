using Microsoft.EntityFrameworkCore;
using UrlShortener.EntityFramework;

namespace UrlShortener.AspNetCore.Custom.Models
{
    public class BaseDbContext : UrlShortenerDbContext<CustomShortUrlEntity>
    {
        public BaseDbContext(DbContextOptions<BaseDbContext> options) : base(options)
        {

        }
    }
}
