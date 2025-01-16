using MusicStore.Dto.Request;
using MusicStore.Dto.Response;

namespace MusicStore.Service.Interfaces;

public interface IConcertService
{
    Task<BaseResponseGeneric<ICollection<ConcertDtoResponse>>> ListAsync(string? filter, int page, int row);
    Task<BaseResponseGeneric<ConcertSingleDtoRequest>> FindByIdAsync(int id);
    Task<BaseResponseGeneric<int>> AddAsync(ConcertDtoRequest request);
    Task<BaseResponse> UpdateAsync(int id, ConcertDtoRequest request);
    Task<BaseResponse> DeleteAsync(int id);
    Task<BaseResponse> FinalizeAsync(int id);
}