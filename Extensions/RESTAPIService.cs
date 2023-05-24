using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Client.Models;

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

    public async Task<TResponse?> Post<TRequest, TResponse>(string endpoint, TRequest data, string accessToken)
    {
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var response = await HttpClient.PostAsJsonAsync(endpoint, data);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<TResponse>();
    }

    public async Task<TResponse?> Patch<TRequest, TResponse>(string endpoint, TRequest data, string accessToken)
    {
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var response = await HttpClient.PatchAsync(endpoint, new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json"));

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<TResponse>();
    }

    public async Task<Response<LoginResponse>?> Login(LoginRequest request)
    {
        var response = await HttpClient.PostAsJsonAsync("accounts/login", request);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<Response<LoginResponse>>();
    }
}