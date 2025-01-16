namespace MusicStore.Entities;

public class Concert : EntityBase
{
    public Genre Genre { get; set; } = default!;  // un Genero puede tener muchos conciertos y un concierto puede tener un solo genero
    public int GenreId { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime DateEvent { get; set; }
    public string? ImageUrl { get; set; }
    public string Place { get; set; } = default!;
    public int TicketsQuantity { get; set; }
    public decimal UnitPrice { get; set; }
    public bool Finalized { get; set; }
    
}