using MusicStore.Dto.Request;
using MusicStore.Dto.Response;

namespace MusicStore.Service.Interfaces;

public interface ISaleService
{
    Task<BaseResponseGeneric<int>> CreateSaleAsync(string email, SaleDtoRequest request);
    Task<BaseResponsePagination<SaleDtoResponse>> ListAsync(DateTime dateStart, DateTime dateEnd, int page, int rows);
    Task<BaseResponsePagination<SaleDtoResponse>> ListAsync(string email, string? filter, int page, int rows);
    
}