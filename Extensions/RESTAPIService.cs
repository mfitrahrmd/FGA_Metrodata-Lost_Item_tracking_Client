using System.Net.Http.Headers;

namespace Client.Extensions;

public class RESTAPIService
{
    public HttpClient HttpClient { get; }

    public RESTAPIService(HttpClient httpClient)
    {
        var httpClientHandler = new HttpClientHandler(){ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true};
        httpClient = new HttpClient(httpClientHandler);
        
        httpClient.BaseAddress = new Uri("https://localhost:7055/api/");
        
        HttpClient = httpClient;
    }

    public async Task<TResponse?> Get<TResponse>(string endpoint, string accessToken)
    {
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var response = await HttpClient.GetAsync(endpoint);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<TResponse>();
    }
}