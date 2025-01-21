using MusicStore.Entities;
using MusicStore.Entities.Infos;

namespace MusicStore.Repositories;

public interface IConcertRepositorio : IRepositoryBase<Concert>
{
    //Task<ICollection<ConcertInfo>> ListAsync(string? filter, int page, int rows);
    Task FinalizeAsync(int id);
    
    
    
    //TODO: Expresiones lamban con parametros
}