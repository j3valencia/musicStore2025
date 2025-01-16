using Microsoft.AspNetCore.Mvc;
using MusicStore.Dto.Request;
using MusicStore.Dto.Response;
using MusicStore.Service.Interfaces;

namespace MusicStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConcertsController : ControllerBase
{
    private readonly IConcertService _service;

    public ConcertsController(IConcertService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> ListAsync(string? filter, int page = 1, int rows=10)
    {
        var response = await _service.ListAsync(filter, page, rows);
        return response.Success ? Ok(response) : NotFound(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> FindByIdAsync(int id)
    {
        var response = await _service.FindByIdAsync(id);
        return response.Success ? Ok(response) : NotFound(response);
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody]ConcertDtoRequest request)
    {
        var response = await _service.AddAsync(request);
        return response.Success ? Ok(response) : BadRequest(response);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync([FromBody] ConcertDtoRequest request, int id)
    {
        var response = await _service.UpdateAsync(id, request);
        return response.Success ? Ok(response) : BadRequest(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var response = await _service.DeleteAsync(id);
        return response.Success ? Ok(response) : NotFound(response);
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> FinalizeAsync(int id)
    {
        var response = await _service.FinalizeAsync(id);
        return response.Success ? Ok(response) : NotFound(response);
    }
    
    
}