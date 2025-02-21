namespace MusicStore.Entities.Infos;

public class ConcertInfo
{
    public int Id { get; set; }
    public string Genre { get; set; } = default!;//para sacar la descripcion del genero
    public string Title { get; set; } = default!; 
    public string Description { get; set; } = default!;
    public string Place { get; set; }= default!;
    public string DateEvent { get; set; } = default!;
    public string TimeEvent { get; set; } = default!;
    public string? ImageUrl { get; set; }
    public int TicketsQuantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string Status { get; set; } = default!;
}