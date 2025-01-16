using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicStore.Entities;

namespace MusicStore.DataAccess.Configurations
{
    public class GenreConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.Property(p => p.Name)
                .HasMaxLength(100);
            
            //data seeding
            //vamos a crear 5 generos de musica iniciales
            builder.HasData(
                new Genre{Id = 1, Name="Rock"},
                new Genre{Id = 2, Name="Pop"},
                new Genre{Id = 3, Name="Jazz"},
                new Genre{Id = 4, Name="Metal"},
                new Genre{Id = 5, Name="Blues"}
            );

        }
        
        
        
    }
}