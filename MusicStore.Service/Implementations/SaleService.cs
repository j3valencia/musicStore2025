using Microsoft.Extensions.Logging;
using MusicStore.Dto.Request;
using MusicStore.Dto.Response;
using MusicStore.Entities;
using MusicStore.Repositories;
using MusicStore.Service.Interfaces;

namespace MusicStore.Service.Implementations;

public class SaleService : ISaleService
{
    private readonly ISaleRepository _saleRepository;
    private readonly IConcertRepositorio _concertRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger<SaleService> _logger;

    public SaleService(ISaleRepository saleRepository, IConcertRepositorio concertRepository, ICustomerRepository customerRepository, ILogger<SaleService> logger)
    {
        _saleRepository = saleRepository;
        _concertRepository = concertRepository;
        _customerRepository = customerRepository;
        _logger = logger;
    }
    
    public async Task<BaseResponseGeneric<int>> CreateSaleAsync(string email, SaleDtoRequest request)
    {
        var response = new BaseResponseGeneric<int>();

        try
        {
            var concert = await _concertRepository.FindByIdAsync(request.ConcertId); //traigo el concert de la venta porque debo verificar q el concert exista

            if (concert is null)
            {
                throw new Exception("No se encontro el concierto");
            }

            if (concert.Finalized)
            {
                throw new Exception("El concierto ya fue finalizado");
            }

            if (concert.DateEvent <= DateTime.Now)
                throw new Exception("El concierto ya termindo");

            var customer = await _customerRepository.GetByEmailAsync(email);
            if (customer is null)
            {
                //codigo de prueba
                customer = new Customer
                {
                    Email = email,
                    FullName = "Test",
                };
                customer.Id = await _customerRepository.AddAsync(customer);
            }

            var sale = new Sale
            {
                ConcertId = concert.Id,
                CustomerId = customer.Id,
                Quantity = request.TicketsQuantity,
                Total = concert.UnitPrice * request.TicketsQuantity,
            };

            response.Data = await _saleRepository.CreateSaleAsync(sale);
            response.Success = true;

        }
        
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Error in SaleService.CreateSaleAsync {message}", ex.Message);
            response.Success = false;
            response.ErrorMessage = "Ocurrio un error al crear la venta";
        }
        
        return response;
    }
}