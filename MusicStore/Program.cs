using Microsoft.EntityFrameworkCore;
using MusicStore.DataAccess;
using MusicStore.Dto.Request;
using MusicStore.Repositories;
using MusicStore.Entities;
using MusicStore.Service.Implementations;
using MusicStore.Service.Interfaces;
using MusicStore.Service.Profiles;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AppSettings>(builder.Configuration);

// Add services to the container.
builder.Services.AddDbContext<MusicStoreDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("Database"));
    
    if(builder.Environment.IsDevelopment())
        options.EnableSensitiveDataLogging();  //permite ver los parametros q le pasamos en las consultas en el terminal
    
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<GenreProfile>();
    config.AddProfile<ConcertProfile>();
    config.AddProfile<SaleProfile>();

});

builder.Services.AddTransient<IGenreRepository, GenreRepository>();
builder.Services.AddTransient<IGenreService, GenreService>();

builder.Services.AddTransient<IConcertRepositorio, ConcertRepositorio>();
builder.Services.AddTransient<IConcertService, ConcertService>();

builder.Services.AddTransient<IFileUploader, FileUploader>();

builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();


builder.Services.AddTransient<ISaleRepository, SaleRepository>();
builder.Services.AddTransient<ISaleService, SaleService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("api/Genres", async (IGenreService service )=> await service.ListAsync());

app.MapGet("api/Genres/{id:int}", async (IGenreService service, int id) =>
{
    var response = await service.FindByIdAsync(id);
    
    return response.Success ? Results.Ok(response) : Results.NotFound(response);

});

app.MapPost("api/Genres", async (IGenreService service, GenreDtoRequest request)=>
{
    var response = await service.AddAsync(request);
    if (response.Success)
    {
        return Results.Ok(response);
    }
    else
    {
        return Results.BadRequest(response);
    }
});

app.MapPut("api/Genres/{id:int}", async (IGenreService service, int id, GenreDtoRequest request) =>
{
    var response = await service.UpdateAsync(id, request);
    return response.Success ? Results.Ok(response) : Results.BadRequest(response);
});

app.MapDelete("api/Genres/{id:int}", async (IGenreService service, int id) =>
{
    var response = await service.DeleteAsync(id);
    return response.Success ? Results.Ok(response) : Results.NotFound(response);
});

// app.MapGet("api/Concerts", async (IConcertService service, string? filter, int page, int rows) =>
// {
//     var response = await service.ListAsync(filter, page, rows);
//     return response.Success ? Results.Ok(response) : Results.NotFound(response);
// });

// app.MapPost("api/Concerts", async (IConcertService service, ConcertDtoRequest request) =>
// {
//     var response = await service.AddAsync(request);
//     return response.Success ? Results.Ok(response) : Results.BadRequest(response);
// });

app.Run();

//TODO: USO DE TUPLAS