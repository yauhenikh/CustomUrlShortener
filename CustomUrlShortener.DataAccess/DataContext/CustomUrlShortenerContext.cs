using CustomUrlShortener.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CustomUrlShortener.DataAccess.DataContext
{
    public class CustomUrlShortenerContext : DbContext
    {
        public CustomUrlShortenerContext(DbContextOptions<CustomUrlShortenerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<UrlInfo> UrlInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
