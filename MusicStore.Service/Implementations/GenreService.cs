using AutoMapper;
using Microsoft.Extensions.Logging;
using MusicStore.Dto.Request;
using MusicStore.Dto.Response;
using MusicStore.Entities;
using MusicStore.Repositories;
using MusicStore.Service.Interfaces;

namespace MusicStore.Service.Implementations;

public class GenreService : IGenreService
{
    private readonly IGenreRepository _repository;
    private readonly ILogger<GenreService> _logger;
    private readonly IMapper _mapper;

    public GenreService(IGenreRepository repository, ILogger<GenreService> logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }
    public async Task<BaseResponseGeneric<IEnumerable<GenreDtoResponse>>> ListAsync()
    {
        var response = new BaseResponseGeneric<IEnumerable<GenreDtoResponse>>();
        try
        {
            var collecion = await _repository.ListAsync();

            // response.Data = collecion.Select(x => new GenreDtoResponse
            // {
            //     Id = x.Id,
            //     Name = x.Name,
            //     Status = x.Status
            // });
            // response.Success = true;
            
            response.Data = _mapper.Map<IEnumerable<GenreDtoResponse>>(collecion); //mapea de genre a genredtoresponse  
            response.Success = true;
            
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Error in GenreService.ListAsync {message}", ex.Message);
            response.Success = false;
            response.ErrorMessage = "Ocurrio un error al listar Generos";
        }

        return response;
    }

    public async Task<BaseResponseGeneric<GenreDtoResponse>> FindByIdAsync(int id)
    {
        var response = new BaseResponseGeneric<GenreDtoResponse>();

        try
        {
            var entity = await _repository.FindByIdAsync(id);
            if (entity is null)
            {
                response.Success = false;
                response.ErrorMessage = "No se encontro el genero";
                return response;
            }

            // response.Data = new GenreDtoResponse
            // {
            //     Id = entity.Id,
            //     Name = entity.Name,
            //     Status = entity.Status
            // };
            // response.Success = true;

            response.Data = _mapper.Map<GenreDtoResponse>(entity);
            response.Success = true;

        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Error in GenreService.FindByIdAsync {message}", ex.Message);
            response.Success = false;
            response.ErrorMessage = "Ocurrio un error al buscar el Genero";
        }
        return response;
    }

    public async Task<BaseResponseGeneric<int>> AddAsync(GenreDtoRequest request)
    {
        var response = new BaseResponseGeneric<int>();
        try
        {
            // var entity = new Genre
            // {
            //     Name = request.Name,
            //     Status = request.Status
            // };

            var entity = _mapper.Map<Genre>(request); //de genredtorequest a genre

            var id = await _repository.AddAsync(entity);
            response.Data = id;
            response.Success = true;
            
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Error in GenreService.AddAsync {message}", ex.Message);
            response.Success = false;
            response.ErrorMessage = "Ocurrio un error al crear el Genero";
        }
        return response;
    }

    public async Task<BaseResponse> UpdateAsync(int id, GenreDtoRequest request)
    {
        var response = new BaseResponse();

        try
        {
            var entity = await _repository.FindByIdAsync(id);
            if (entity is null)
            {
                response.Success = false;
                response.ErrorMessage = "No se encontro el genero";
                return response;
            }
            // entity.Name = request.Name;
            // entity.Status = request.Status;
            
            //cuando hay una propiedad que ya existe como este caso entity ya lo buscamos por medio de findByIdAsync, hacemos la sobrecarga de mapper
            
            _mapper.Map(request, entity);  //los request o los parametros a actualizar  lo metemos o remplazamos en el entity 
            await _repository.UpdateAsync();
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex,"Error in GenreService.UpdateAsync {message}", ex.Message);
            response.Success = false;
            response.ErrorMessage = "Ocurrio un error al actualizar el Genero";
        }
        
        return response;
    }

    public async Task<BaseResponse> DeleteAsync(int id)
    {
        var response = new BaseResponse();
        try
        {
            var entity = await _repository.FindByIdAsync(id);
            if (entity is null)
            {
                response.Success = false;
                response.ErrorMessage = "No se encontro el genero";
                return response;    
            }

            await _repository.DeleteAsync(entity.Id);
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex,"Error in GenreService.DeleteAsync {message}", ex.Message);
            response.Success = false;
            response.ErrorMessage = "Ocurrio un error al eliminar el Genero";
        }
        return response;
    }
}