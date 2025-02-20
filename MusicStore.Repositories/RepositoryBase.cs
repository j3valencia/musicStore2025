using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MusicStore.Entities;

namespace MusicStore.Repositories;

public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : EntityBase
{
    protected readonly DbContext Context; //podriamos crear la variable tambien de tipo MusicStoreDbContext
    
    public RepositoryBase(DbContext context)
    {
        Context = context;
    }
    
    public async Task<ICollection<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Context.Set<TEntity>()
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync();
    }

    //funcion con tupla
    public virtual async Task<(ICollection<TInfo> Collection, int Total)> ListAsync<TInfo, TKey>(
        Expression<Func<TEntity, bool>> predicate, 
        Expression<Func<TEntity, TInfo>> selector, 
        Expression<Func<TEntity, TKey>> orderBy, int page, int rows  )
    
    {
        var collection = await Context.Set<TEntity>()
            .Where(predicate)
            .OrderBy(orderBy)
            .Skip((page - 1) * rows)
            .Take(rows)
            .AsNoTracking()
            .Select(selector)
            .ToListAsync();
        
        var total = await Context.Set<TEntity>()
            .Where(predicate)
            .CountAsync();
        
        return (collection, total);
    }

    public async Task<ICollection<TInfo>> ListAsync<TInfo>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TInfo>> selector)
    {
        return await Context.Set<TEntity>()
            .Where(predicate)
            .AsNoTracking()
            .Select(selector)
            .ToListAsync();
    }

    public async Task<int> AddAsync(TEntity entity)
    {
        await Context.Set<TEntity>().AddAsync(entity);
        await Context.SaveChangesAsync();
        return entity.Id;
    }

    public virtual async Task<TEntity?> FindByIdAsync(int id)
    {
        return await Context.Set<TEntity>().FindAsync(id); //el FindAsync devuelve un null en caso de q no encuentre 
    }

    public async Task UpdateAsync()
    {
        await Context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        Context.Update(entity);
        await Context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await Context.Set<TEntity>().FindAsync(id);
        if (entity is not null)
        {
            entity.Status = false;
            await UpdateAsync();
            
        }
        else
        {
            throw new InvalidOperationException($"No se encontro el registro con el id {id}"); //
        }
        
    }
}