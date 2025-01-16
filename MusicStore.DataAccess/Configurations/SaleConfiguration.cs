using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicStore.Entities;

namespace MusicStore.DataAccess.Configurations;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.Property(s => s.OperationNumber)
            .IsUnicode(false)
            .HasMaxLength(20);
        builder.Property(s => s.Total)
            .HasPrecision(11, 2);
    }
}