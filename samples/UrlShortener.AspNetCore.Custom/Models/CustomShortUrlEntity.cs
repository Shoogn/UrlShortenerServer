using UrlShortener.EntityFramework.Store;

namespace UrlShortener.AspNetCore.Custom.Models
{
    /// <summary>
    /// Define a new properties and added them to an exiting ones inside <see cref="ShortUrlEntity"/>;
    /// </summary>
    public class CustomShortUrlEntity : ShortUrlEntity
    {
        /// <summary>
        /// Get or set the created date of <see cref="ShortUrlEntity"/> object
        /// and to be honest there is no need to add at which time the <see cref="ShortUrlEntity"/> object is created,
        /// because internaly the UrlShortenerServer use SnowflakeId algorithm to create a unique Id for any object,
        /// and by default you can get the created time from this Id, but I added this column here for demo purpose,
        /// for more details about SnowflakeId <see cref="https://github.com/Shoogn/SnowflakeId"/> repo.
        /// </summary>
        public DateTime? AddedDate { get; set; }
    }
}
