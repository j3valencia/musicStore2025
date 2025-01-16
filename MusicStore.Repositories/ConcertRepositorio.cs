using Microsoft.EntityFrameworkCore;
using MusicStore.DataAccess;
using MusicStore.Entities;
using MusicStore.Entities.Infos;

namespace MusicStore.Repositories;

public class ConcertRepositorio : IConcertRepositorio
{
    private readonly MusicStoreDbContext _context;
    
    public ConcertRepositorio(MusicStoreDbContext context)
    {
        _context = context;
    }
    public async Task<ICollection<ConcertInfo>> ListAsync(string? filter, int page, int rows)
    {
        return await _context.Set<Concert>()
            .Include(x => x.Genre) //eager loading, se lo utiliza cuando la tabla no tiene muchos movimientos
            .Where(x => x.Status
                        && (x.Title.Contains(filter ?? string.Empty)))
            .OrderByDescending(x => x.DateEvent)
            .Skip((page - 1) * rows)
            .Take(rows)
            .Select(x => new ConcertInfo
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                DateEvent = x.DateEvent.ToString("yyyy-MM-dd"),
                TimeEvent = x.DateEvent.ToString("HH:mm:ss"),
                Genre = x.Genre.Name, //lazy loading, se lo utiliza cuando la tabla tiene mucho movimiento
                Place = x.Place,
                UnitPrice = x.UnitPrice,
                ImageUrl = x.ImageUrl,
                TicketsQuantity = x.TicketsQuantity,
                Status = x.Finalized ? "Finalizado" : "Pendiente"

            })
            .AsNoTracking() //permite traer los datos sin el changetracker de ef core 
            .ToListAsync();
    }

    public async Task<Concert?> FindByIdAsync(int id)
    {
        return await _context.Set<Concert>()
            .Include(x => x.Genre)
            .FirstOrDefaultAsync(p => p.Id == id);  
    }


    public async Task<int> AddAsync(Concert entity)
    {
        await _context.Set<Concert>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task UpdateAsync()
    {
      //  _context.Set<Concert>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        
        var entity = await _context.Set<Concert>().FindAsync(id);
        if (entity is not null)
        {
            entity.Status = false;
            await UpdateAsync();
        }
        else
        {
            throw new InvalidOperationException("No se encontro el concert");
        }

        
    }

    public async Task FinalizeAsync(int id)
    {
        //Modelo conectado, ef core tiene el registro en memoria
        var entity = await _context.Set<Concert>().SingleOrDefaultAsync(c => c.Id == id && c.Status);
        if(entity is null) throw new InvalidOperationException("No se encontro el concert");
        
        entity.Finalized = true;
        await UpdateAsync();
    }
    
    //todo: modelo conectado y desconectado min35
    
}