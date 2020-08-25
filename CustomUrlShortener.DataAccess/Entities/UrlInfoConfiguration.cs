using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomUrlShortener.DataAccess.Entities
{
    public class UrlInfoConfiguration : IEntityTypeConfiguration<UrlInfo>
    {
        public void Configure(EntityTypeBuilder<UrlInfo> builder)
        {
            builder.ToTable("url_infos");
            builder.HasKey(ui => ui.Id);
            builder.Property(ui => ui.Id).HasColumnName("id");
            builder.Property(ui => ui.LongUrl).IsRequired().HasMaxLength(1000).HasColumnName("long_url");
            builder.Property(ui => ui.Token).IsRequired().HasMaxLength(20).HasColumnName("token");
            builder.Property(ui => ui.Created).IsRequired().HasColumnName("created");
            builder.Property(ui => ui.ClickNumber).IsRequired().HasColumnName("click_number");
        }
    }
}
