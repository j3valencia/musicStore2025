using AutoMapper;
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
    private readonly IMapper _mapper;

    public ConcertService(IConcertRepositorio repositorio, ILogger<ConcertService> logger, IFileUploader fileUploader, IMapper mapper)
    {
        _repositorio = repositorio;
        _logger = logger;
        _fileUploader = fileUploader;
        _mapper = mapper;
    }
    
    public async Task<BaseResponsePagination<ConcertDtoResponse>> ListAsync(string? filter, int page, int row)
    {
        var response = new BaseResponsePagination<ConcertDtoResponse>();
        try
        {
            var tuple = await _repositorio
                .ListAsync(c => c.Title.Contains(filter ?? string.Empty),
                    c => _mapper.Map<ConcertDtoResponse>(c), //de genre a genredtoresponse
                    x => x.DateEvent, page, row);

            response.Collection = tuple.Collection;
            response.TotalPages = tuple.Total / row;
            if(tuple.Total % row > 0)//si el residuo es mayor a 0 la pagina aumenta en 1
                response.TotalPages++;
            
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

            response.Data = _mapper.Map<ConcertSingleDtoRequest>(concert); //mapeamos de concert a concertSingleDtorequest
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
            var concert = _mapper.Map<Concert>(request);

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
            var entity = await _repositorio.FindByIdAsync(id); //este select utiliza el changetracker ssadas
            if (entity is null)
            {
                response.Success = false;
                response.ErrorMessage = "No se pudo encontrar el concierto";
                return response;
            }
            // entity.Title = request.Title;
            // entity.Description = request.Description;
            // entity.DateEvent = Convert.ToDateTime($"{request.DateEvent} {request.TimeEvent}");
            // entity.UnitPrice = request.UnitPrice;
            // entity.TicketsQuantity = request.TicketsQuantity;
            // entity.Place = request.Place;
            
            _mapper.Map(request, entity); //los request o los parametros a actualizar  lo metemos o remplazamos en el entity 
            
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