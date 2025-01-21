using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MusicStore.DataAccess;
using MusicStore.Entities;
using MusicStore.Entities.Infos;

namespace MusicStore.Repositories;

public class ConcertRepositorio : RepositoryBase<Concert> , IConcertRepositorio
{
    
    
    public ConcertRepositorio (MusicStoreDbContext context) : base(context)
    {
    }
    // public async Task<ICollection<ConcertInfo>> ListAsync(string? filter, int page, int rows)
    // {
    //     return await _context.Set<Concert>()
    //         .Include(x => x.Genre) //eager loading, se lo utiliza cuando la tabla no tiene muchos movimientos
    //         .Where(x => x.Status
    //                     && (x.Title.Contains(filter ?? string.Empty)))
    //         .OrderByDescending(x => x.DateEvent)
    //         .Skip((page - 1) * rows)
    //         .Take(rows)
    //         .Select(x => new ConcertInfo
    //         {
    //             Id = x.Id,
    //             Title = x.Title,
    //             Description = x.Description,
    //             DateEvent = x.DateEvent.ToString("yyyy-MM-dd"),
    //             TimeEvent = x.DateEvent.ToString("HH:mm:ss"),
    //             Genre = x.Genre.Name, //lazy loading, se lo utiliza cuando la tabla tiene mucho movimiento
    //             Place = x.Place,
    //             UnitPrice = x.UnitPrice,
    //             ImageUrl = x.ImageUrl,
    //             TicketsQuantity = x.TicketsQuantity,
    //             Status = x.Finalized ? "Finalizado" : "Pendiente"
    //
    //         })
    //         .AsNoTracking() //permite traer los datos sin el changetracker de ef core 
    //         .ToListAsync();
    // }


    
    //esta funcion trae todos los conciertos agregando la clase Genre
    public  override async Task<(ICollection<TInfo> Collection, int Total)> ListAsync<TInfo, TKey>(
        Expression<Func<Concert, bool>> predicate, 
        Expression<Func<Concert, TInfo>> selector, 
        Expression<Func<Concert, TKey>> orderBy, int page, int rows)
    {
        var collection = await Context.Set<Concert>()
            .Include(p => p.Genre) //eager loading
            .Where(predicate)
            .OrderBy(orderBy)
            .Skip((page - 1) * rows)
            .Take(rows)
            .AsNoTracking()
            .Select(selector)
            .ToListAsync();

        var total = await Context.Set<Concert>()
            .Where(predicate)
            .CountAsync();
        
        return (collection, total);
    }


    public override async Task<Concert?> FindByIdAsync(int id)
    {
        return await Context.Set<Concert>()
            .Include(p => p.Genre)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task FinalizeAsync(int id)
    {
        //Modelo conectado, ef core tiene el registro en memoria
        
        var entity = await Context.Set<Concert>().SingleOrDefaultAsync(c => c.Id == id && c.Status);
        if(entity is null) throw new InvalidOperationException("No se encontro el concert");
        
        entity.Finalized = true;
        await UpdateAsync();
    }
    
   
    
}