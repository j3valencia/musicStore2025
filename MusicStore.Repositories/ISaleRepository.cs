using System.Collections;
using System.Linq.Expressions;
using MusicStore.Entities;

namespace MusicStore.Repositories;

public interface ISaleRepository : IRepositoryBase<Sale>
{
    Task<int> CreateSaleAsync(Sale entity);

    new Task<(ICollection<TInfo> Collection, int Total)> ListAsync<TInfo, TKey>(
        Expression<Func<Sale, bool>> predicate,
        Expression<Func<Sale, TInfo>> selector,
        Expression<Func<Sale, TKey>> orderBy,
        int page, int rows
    );

}