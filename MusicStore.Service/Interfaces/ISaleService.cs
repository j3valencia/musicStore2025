using MusicStore.Dto.Request;
using MusicStore.Dto.Response;

namespace MusicStore.Service.Interfaces;

public interface ISaleService
{
    Task<BaseResponseGeneric<int>> CreateSaleAsync(string email, SaleDtoRequest request);
}