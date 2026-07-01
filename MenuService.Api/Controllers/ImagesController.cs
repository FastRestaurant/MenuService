using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using MenuService.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MenuService.Api.Controllers;

[ApiController]
[Route("api/v1/images")]
[Authorize(Roles = "Admin")]
public class ImagesController : ControllerBase
{
    private const long MaxFileSize = 5 * 1024 * 1024;
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;

    public ImagesController(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
    }

    [HttpPost]
    [RequestSizeLimit(MaxFileSize)]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file is null || file.Length == 0)
            return Error(StatusCodes.Status400BadRequest, "Debe seleccionar una imagen.");

        if (file.Length > MaxFileSize)
            return Error(StatusCodes.Status400BadRequest, "La imagen no puede superar los 5 MB.");

        if (string.IsNullOrWhiteSpace(file.ContentType) || !file.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
            return Error(StatusCodes.Status400BadRequest, "El archivo debe ser una imagen.");

        var cloudName = _configuration["Cloudinary:CloudName"];
        var apiKey = _configuration["Cloudinary:ApiKey"];
        var apiSecret = _configuration["Cloudinary:ApiSecret"];

        if (string.IsNullOrWhiteSpace(cloudName) || string.IsNullOrWhiteSpace(apiKey) || string.IsNullOrWhiteSpace(apiSecret))
            return Error(StatusCodes.Status500InternalServerError, "Falta configurar Cloudinary.");

        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        var folder = _configuration["Cloudinary:Folder"] ?? "fastrestaurant";
        var signature = BuildSignature(folder, timestamp, apiSecret);

        using var content = new MultipartFormDataContent();

        await using var stream = file.OpenReadStream();
        using var memory = new MemoryStream();
        await stream.CopyToAsync(memory);
        using var fileContent = new ByteArrayContent(memory.ToArray());
        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
        content.Add(fileContent, "file", file.FileName);

        var client = _httpClientFactory.CreateClient();
        var response = await client.PostAsync(BuildUploadUrl(cloudName, apiKey, timestamp, folder, signature), content);
        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            return Error((int)response.StatusCode, BuildCloudinaryErrorMessage(responseBody));

        using var json = JsonDocument.Parse(responseBody);
        var url = json.RootElement.GetProperty("secure_url").GetString();

        return Ok(new { url });
    }

    private static string BuildSignature(string folder, string timestamp, string apiSecret)
    {
        var value = $"folder={folder}&timestamp={timestamp}{apiSecret}";
        var bytes = SHA1.HashData(Encoding.UTF8.GetBytes(value));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }

    private static string BuildUploadUrl(string cloudName, string apiKey, string timestamp, string folder, string signature)
    {
        return $"https://api.cloudinary.com/v1_1/{Uri.EscapeDataString(cloudName)}/image/upload" +
            $"?api_key={Uri.EscapeDataString(apiKey)}" +
            $"&timestamp={Uri.EscapeDataString(timestamp)}" +
            $"&folder={Uri.EscapeDataString(folder)}" +
            $"&signature={Uri.EscapeDataString(signature)}";
    }

    private static string BuildCloudinaryErrorMessage(string responseBody)
    {
        const string fallbackMessage = "Cloudinary rechazó la imagen.";

        if (string.IsNullOrWhiteSpace(responseBody))
            return fallbackMessage;

        try
        {
            using var json = JsonDocument.Parse(responseBody);
            if (json.RootElement.TryGetProperty("error", out var error) &&
                error.TryGetProperty("message", out var message) &&
                !string.IsNullOrWhiteSpace(message.GetString()))
            {
                return $"{fallbackMessage} {message.GetString()}";
            }
        }
        catch (JsonException)
        {
            return fallbackMessage;
        }

        return fallbackMessage;
    }

    private static ObjectResult Error(int statusCode, string message)
    {
        return new ObjectResult(new ErrorResponseDto
        {
            Message = message,
            StatusCode = statusCode,
            Timestamp = DateTime.UtcNow
        })
        {
            StatusCode = statusCode
        };
    }
}
