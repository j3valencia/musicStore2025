using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicStore.Entities;

namespace MusicStore.DataAccess.Configurations;

public class ConcertConfiguration : IEntityTypeConfiguration<Concert>
{
    public void Configure(EntityTypeBuilder<Concert> builder)
    {
        builder.Property(c => c.Title)
            .HasMaxLength(100);
        builder.Property(c => c.Description)
            .HasMaxLength(500);
        builder.Property(c => c.Place)
            .HasMaxLength(100);
        builder.Property(c => c.ImageUrl)
            .IsUnicode(false)
            .HasMaxLength(1000);
        builder.Property(c => c.UnitPrice)
            .HasPrecision(11, 2);
        
    }
}