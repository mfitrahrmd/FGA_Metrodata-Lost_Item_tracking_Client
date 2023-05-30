using System.Collections;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;
using Client.Models;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

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
    
    public async Task<TResponse?> Post<TResponse>(string endpoint, string accessToken)
    {
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var response = await HttpClient.PostAsync(endpoint, null);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<TResponse>();
    }

    public async Task<TResponse?> PostForm<TRequest, TResponse>(string endpoint, TRequest data, string accessToken)
    {
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        using (var multipartFormDataContent = new MultipartFormDataContent())
        {
            foreach (var prop in data.GetType().GetProperties())
            {
                var value = prop.GetValue(data);

                if (value is FormFile)
                {
                    var file = value as FormFile;
                    multipartFormDataContent.Add(new StreamContent(file.OpenReadStream()), prop.Name, file.FileName);
                    multipartFormDataContent.Headers.ContentDisposition =
                        new ContentDispositionHeaderValue("form-data") { Name = prop.Name, FileName = file.FileName};
                }
                else
                {
                    multipartFormDataContent.Add(new StringContent(value.ToString()), prop.Name);
                }
            }

            var response = await HttpClient.PostAsync(endpoint, multipartFormDataContent);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<TResponse>();
        }
    }

    public async Task<TResponse?> Patch<TRequest, TResponse>(string endpoint, TRequest data, string accessToken)
    {
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var response = await HttpClient.PatchAsync(endpoint, new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json"));

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<TResponse>();
    }
    public async Task<TResponse?> Put<TRequest, TResponse>(string endpoint, TRequest data, string accessToken)
    {
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var response = await HttpClient.PutAsJsonAsync(endpoint, data);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<TResponse>();
    }

    public async Task<Response<LoginResponse>?> Login(LoginRequest request)
    {
        var response = await HttpClient.PostAsJsonAsync("accounts/login", request);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<Response<LoginResponse>>();
    }

    public async Task<Response<RegisterResponse>?> Register(RegisterRequest request)
    {
        var response = await HttpClient.PostAsJsonAsync("accounts/register", request);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<Response<RegisterResponse>>();
    }
}