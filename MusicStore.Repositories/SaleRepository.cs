using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MusicStore.DataAccess;
using MusicStore.Entities;

namespace MusicStore.Repositories;

public class SaleRepository : RepositoryBase<Sale>, ISaleRepository
{
    public SaleRepository(MusicStoreDbContext context) : base(context)
    {
        
    }

    public async Task<int> CreateSaleAsync(Sale entity)
    {
        entity.SaleDate = DateTime.Now;
        var lastNumber = await Context.Set<Sale>().CountAsync() + 1;
        entity.OperationNumber = $"{lastNumber:000000}"; //000001

        await Context.Set<Sale>().AddAsync(entity);
        await Context.SaveChangesAsync();

        return entity.Id;
    }

    public async override Task<(ICollection<TInfo> Collection, int Total)> ListAsync<TInfo, TKey>(
        Expression<Func<Sale, bool>> predicate, 
        Expression<Func<Sale, TInfo>> selector, 
        Expression<Func<Sale, TKey>> orderBy, 
        int page, int rows)
    {
        var collection = await Context.Set<Sale>()
            .Include(x => x.Customer)
            .Include(x => x.Concert)
           .ThenInclude(x => x.Genre)
            .Where(predicate)
            .OrderBy(orderBy)
            .Skip((page-1)*rows)
            .Take(rows)
            .AsNoTracking()
            .Select(selector)
            .ToListAsync();
        
        var total = await Context.Set<Sale>()
            .Where(predicate)
            .CountAsync();
        
        return  (collection, total);
    }
}