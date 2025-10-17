namespace ClawSome.Services;

public class CatFactService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CatFactService> _logger;
    private readonly string _apiUrl;

    private record CatFactDto(string Fact, int Length);

    public CatFactService(HttpClient httpClient, IConfiguration config, ILogger<CatFactService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _httpClient.Timeout = TimeSpan.FromSeconds(
            config.GetValue("ExternalApis:TimeoutInSeconds", 5)
        );
        _apiUrl = config["ExternalApis:CatFactApiUrl"] ?? "https://catfact.ninja/fact";
    }

    public async Task<string> GetRandomCatFactAsync()
    {
        try
        {
            _logger.LogInformation("Fetching cat fact from: {Url}", _apiUrl);

            var factDto = await _httpClient.GetFromJsonAsync<CatFactDto>(_apiUrl);

            return factDto?.Fact ?? "Could not fetch a cat fact.";
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP Request error while fetching cat fact.");
            return "Cat Facts API is currently unavailable (Network Error).";
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, "Timeout while fetching cat fact.");
            return "Cat Facts API call timed out.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while fetching cat fact.");
            return "An unexpected error occurred with the Cat Facts API.";
        }
    }
}
