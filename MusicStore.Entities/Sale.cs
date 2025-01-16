namespace MusicStore.Entities;

public class Sale: EntityBase
{
    public Customer Customer { get; set; } = default!; //Un cliente tiene muchas ventas y una venta tiene un solo cliente
    public int CustomerId { get; set; }
    public Concert Concert { get; set; } = default!; //un concierto tiene una sola venta, y una venta tiene solo concierto
    public int ConcertId { get; set; }
    public DateTime SaleDate { get; set; }
    public string OperationNumber { get; set; } = default!;
    public decimal Total { get; set; }
    public short Quantity { get; set; }
    
    
    
}