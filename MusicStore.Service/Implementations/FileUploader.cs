using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MusicStore.Entities;
using MusicStore.Service.Interfaces;

namespace MusicStore.Service.Implementations;

public class FileUploader : IFileUploader
{
    private readonly ILogger<FileUploader> _logger;
    private readonly AppSettings _options;

    public FileUploader(IOptions<AppSettings> options, ILogger<FileUploader> logger) //Ioption una interfaz q permite leer archivos de configuracio, nos da la instancia de appSettings
    {
        _logger = logger;
        _options = options.Value;
    }

    public async Task<string> UploadFileAsync(string? base64String, string? fileName)
    {
        if (string.IsNullOrEmpty(base64String) || string.IsNullOrEmpty(fileName))
        {
            return string.Empty;
        }

        try
        {
            var bytes = Convert.FromBase64String(base64String); // conbierte un array de bytes
            var path = Path.Combine(_options.StorageConfiguration.Path, fileName); //donde se va escribir la rura, aqui hacemos la ruta, combina el path de la carpeta y el nombre del archivo
            await using var fileStream = new FileStream(path, FileMode.Create);//subimos el archivo, 
            await fileStream.WriteAsync(bytes,0,bytes.Length);//escribe todos los bytes comienza a partir del parametro 0 

            return $"{_options.StorageConfiguration.PublicUrl}{fileName}";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al subir el archivo {filename} {message}", fileName, ex.Message);
            return string.Empty;
        }
        
    }
}