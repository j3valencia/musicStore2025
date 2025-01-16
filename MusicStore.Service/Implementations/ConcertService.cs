using Microsoft.Extensions.Logging;
using MusicStore.Dto.Request;
using MusicStore.Dto.Response;
using MusicStore.Entities;
using MusicStore.Repositories;
using MusicStore.Service.Interfaces;

namespace MusicStore.Service.Implementations;

public class ConcertService : IConcertService
{
    private readonly IConcertRepositorio _repositorio;
    private readonly ILogger<ConcertService> _logger;
    private readonly IFileUploader _fileUploader;

    public ConcertService(IConcertRepositorio repositorio, ILogger<ConcertService> logger, IFileUploader fileUploader)
    {
        _repositorio = repositorio;
        _logger = logger;
        _fileUploader = fileUploader;
    }
    
    public async Task<BaseResponseGeneric<ICollection<ConcertDtoResponse>>> ListAsync(string? filter, int page, int row)
    {
        var response = new BaseResponseGeneric<ICollection<ConcertDtoResponse>>();
        try
        {
            var concerts = await _repositorio.ListAsync(filter, page, row);
            response.Data = concerts.Select(c => new ConcertDtoResponse
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                DateEvent = c.DateEvent,
                TimeEvent = c.TimeEvent,
                Place = c.Place,
                UnitPrice = c.UnitPrice,
                Genre = c.Genre,
                ImageUrl = c.ImageUrl,
                TicketsQuantity = c.TicketsQuantity,
                Status = c.Status
                    
            }).ToList();
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Error in Concertservice.ListAsync {message}", ex.Message);
            response.Success = false;
            response.ErrorMessage = "Error al listar los conciertos";
        }
        return response;
    }

    public async Task<BaseResponseGeneric<ConcertSingleDtoRequest>> FindByIdAsync(int id)
    {
        var response = new BaseResponseGeneric<ConcertSingleDtoRequest>();
        try
        {
            var concert = await _repositorio.FindByIdAsync(id);
            if (concert is null)
            {
                response.Success = false;
                response.ErrorMessage = "Error al obtener el conciero para el ID";
                return response;
            }

            response.Data = new ConcertSingleDtoRequest
            {
                Id = concert.Id,
                Title = concert.Title,
                Description = concert.Description,
                DateEvent = concert.DateEvent.ToString("yyyy-MM-dd"),
                TimeEvent = concert.DateEvent.ToString("HH:mm"),
                ImageUrl = concert.ImageUrl,
                Place = concert.Place,
                TicketsQuantity = concert.TicketsQuantity,
                UnitPrice = concert.UnitPrice,
                Status = concert.Status ? "Activo" : "Inactivo",
                GenreDtoResponse = new GenreDtoResponse
                {
                    Id = concert.Genre.Id,
                    Name = concert.Genre.Name,
                    Status = concert.Genre.Status
                }
                
            };
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Error in Concertservice.FindByIdAsync {message}", ex.Message);
            response.Success = false;
            response.ErrorMessage = "Error al obtener los conciertos";
        }
        return response;
    }

    public async Task<BaseResponseGeneric<int>> AddAsync(ConcertDtoRequest request)
    {
        var response = new BaseResponseGeneric<int>();
        try
        {
            var concert = new Concert
            {
                Title = request.Title,
                Description = request.Description,
                DateEvent = Convert.ToDateTime($"{request.DateEvent} {request.TimeEvent}"),
                GenreId = request.IdGenre,
                UnitPrice = request.UnitPrice,
                TicketsQuantity = request.TicketsQuantity,
                Place = request.Place,
            };

            concert.ImageUrl = await _fileUploader.UploadFileAsync(request.Base64Image, request.FileName);
            
            
            await _repositorio.AddAsync(concert);
            response.Data = concert.Id;
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Error in ConcertService.AddAsync{message}", ex.Message);
            response.Success = false;
            response.ErrorMessage="No se pudo agregar el concierto";
        }

        return response;
    }

    public async Task<BaseResponse> UpdateAsync(int id, ConcertDtoRequest request)
    {
        var response = new BaseResponse();
        try
        {
            var entity = await _repositorio.FindByIdAsync(id); //este select utiliza el changetracker
            if (entity is null)
            {
                response.Success = false;
                response.ErrorMessage = "No se pudo encontrar el concierto";
                return response;
            }
            entity.Title = request.Title;
            entity.Description = request.Description;
            entity.DateEvent = Convert.ToDateTime($"{request.DateEvent} {request.TimeEvent}");
            entity.UnitPrice = request.UnitPrice;
            entity.TicketsQuantity = request.TicketsQuantity;
            entity.Place = request.Place;

            if (!string.IsNullOrEmpty(request.FileName)) 
                entity.ImageUrl = await _fileUploader.UploadFileAsync(request.Base64Image, request.FileName);
            
            await _repositorio.UpdateAsync();
            response.Success = true;
            
        }
        catch (Exception ex)
        {
                _logger.LogCritical(ex, "Error en ConcertService.UpdateAsync {message}", ex.Message);
                response.Success = false;
                response.ErrorMessage = "No se pudo actualizar el concierto";
        }
        return response;
    }

    public async Task<BaseResponse> DeleteAsync(int id)
    {
        var response = new BaseResponse();
        try
        {
            var entity = await _repositorio.FindByIdAsync(id);
            if (entity is null)
            {
                response.Success = false;
                response.ErrorMessage = "No se pudo el concierto";
                return response;
            }

            await _repositorio.DeleteAsync(entity.Id);
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Error in ConcertService.DeleteAsync {message}",ex.Message);
            response.Success = false;
            response.ErrorMessage = "Error al eliminar el concierto";
        }
        return response;
    }

    public async Task<BaseResponse> FinalizeAsync(int id)
    {
        var response = new BaseResponse();
        try
        {
            var entity = await _repositorio.FindByIdAsync(id);
            if (entity is null)
            {
                response.Success = false;
                response.ErrorMessage = "No se pudo el concierto";
            }
            await _repositorio.FinalizeAsync(id);
            response.Success = true;
            
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Error in ConcertService.FinalizeAsync {message}",ex.Message);
            response.Success = false;
            response.ErrorMessage = "Error al eliminar el concierto";
        }
        return response;
    }
}