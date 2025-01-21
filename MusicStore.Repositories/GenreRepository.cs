using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicStore.DataAccess;
using MusicStore.Entities;

namespace MusicStore.Repositories
{
    
    public class GenreRepository : RepositoryBase<Genre>, IGenreRepository //hereda del repositoriobase de unitwork y de la interfaz
    {
        public GenreRepository(MusicStoreDbContext context) : base(context) // le decimos q la variable de musicstoredbcontext va a pasar al contructor base
        {
        }
        public async Task<List<Genre>> ListAsync()
        {
            return await Context.Set<Genre>()
                .Where(p => p.Status)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}