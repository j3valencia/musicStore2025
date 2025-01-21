
using Microsoft.AspNetCore.Mvc;
using MusicStore.Dto.Request;
using MusicStore.Service.Interfaces;

namespace MusicStore.Controllers;


[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly ISaleService _saleService;

    public SalesController(ISaleService saleService)
    {
        _saleService = saleService;
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
    
}