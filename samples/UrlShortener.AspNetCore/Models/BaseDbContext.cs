using Microsoft.EntityFrameworkCore;
using UrlShortener.EntityFramework;

namespace UrlShortener.AspNetCore.Models
{
    public class BaseDbContext : UrlShortenerDbContext
    {
        public BaseDbContext(DbContextOptions<BaseDbContext> options) : base(options) { }

    }
}
