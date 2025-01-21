using System.Linq.Expressions;
using MusicStore.Entities;

namespace MusicStore.Repositories;

public interface IRepositoryBase<TEntity> where TEntity : EntityBase //interfaz generica en donde TEntity es un entitybase osea q solo pueden ser las entidades, ya q todas las entidades heredan de entitybase
{
        //Listar todos los registros
        Task<ICollection<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate); //una funcion que devuelva un Tentity y q sea booleana

        //funcion que devuelve una lista, tiene como parametros un predicado un selector q es la projeccion, tambien va recibir el dato para ordenar y para la paginacion
        Task<(ICollection<TInfo> Collection, int Total)> ListAsync<TInfo, TKey>(  
                Expression<Func<TEntity, bool>> predicate, //recibe una entidad y devuelve un booleado, todos los predicados devuelven booleanos
                Expression<Func<TEntity, TInfo>> selector, //recibe la entidad y la transforma a TInfo que puede ser cualquier cosa
                Expression<Func<TEntity, TKey>> orderBy, //
                int page, int rows
        );

        Task<ICollection<TInfo>> ListAsync<TInfo>(
                Expression<Func<TEntity, bool>> predicate,
                Expression<Func<TEntity, TInfo>> selector
        );
        Task<int> AddAsync(TEntity entity);
        Task<TEntity?> FindByIdAsync(int id);
        Task UpdateAsync(); //el modelo conectado
        Task UpdateAsync(TEntity entity); //modelo desconectado
        Task DeleteAsync(int id);

}