namespace MusicStore.Dto.Response;

public class BaseResponse
{
    public bool Success { get; set; }//pude eliminar un registro si o no
    public string? ErrorMessage { get; set; } 
}