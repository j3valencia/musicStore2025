
using Microsoft.AspNetCore.Mvc;
using MusicStore.Dto.Request;
using MusicStore.Dto.Response;
using MusicStore.Service.Interfaces;

namespace MusicStore.Controllers;


[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly ISaleService _saleService;
    private readonly ILogger<SalesController> _logger;

    public SalesController(ISaleService saleService, ILogger<SalesController> logger)
    {
        _saleService = saleService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSaleAsync(string email, [FromBody] SaleDtoRequest request)
    {
        var response = await _saleService.CreateSaleAsync(email, request);

        if (response.Success)
        {
            return Ok(response);
        }
        return BadRequest(response);;
        
    }

    [HttpGet("ListSales")]
    public async Task<IActionResult> GetListSaleAsync(string email, string? filter, int page = 1, int rows = 10)
    {
        var response = await _saleService.ListAsync(email, filter, page, rows);
        
        return response.Success ? Ok(response) : NotFound(response);
    }


    [HttpGet("ListSalesByDate")]
    public async Task<IActionResult> GetListSaleDateAsync(string dateStart, string dateEnd, int page = 1, int rows = 10)
    {
        try
        {
            var response = await _saleService.ListAsync(DateTime.Parse(dateStart), DateTime.Parse(dateEnd), page, rows);

            return response.Success ? Ok(response) : NotFound(response);
        }
        catch (FormatException ex)
        {
            _logger.LogWarning(ex, "Error en conversion de formato de fecha {message}", ex.Message);
            return BadRequest(new BaseResponse {ErrorMessage = "Error en conversion de formato de fecha"});
        }
    }
}
    
