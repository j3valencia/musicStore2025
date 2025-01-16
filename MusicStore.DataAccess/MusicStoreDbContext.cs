using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicStore.Entities;

namespace MusicStore.DataAccess
{
    public class MusicStoreDbContext :DbContext
    {

        public MusicStoreDbContext(DbContextOptions<MusicStoreDbContext> options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); //scanea todas las clases para no ir creando las tablas una por una 
        }
        
        
        




    }
}