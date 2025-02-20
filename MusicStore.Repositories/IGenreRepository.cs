using MusicStore.Entities;

namespace MusicStore.Repositories;

public interface IGenreRepository : IRepositoryBase<Genre>
{
    Task<List<Genre>> ListAsync();
    
}