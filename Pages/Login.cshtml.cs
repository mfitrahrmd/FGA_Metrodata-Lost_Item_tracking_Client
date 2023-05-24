using Client.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client.Pages;

public class LoginRequest
{
    public string Nik { get; set; }
    public string Password { get; set; }
}

public class LoginResponse
{
    public string AccessToken { get; set; }
}

public class Login : PageModel
{
    public void OnGet()
    {
    }
    
    public async Task<IActionResult> OnPost([FromForm] LoginRequest request)
    {
        var clientHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true
        };

        var response = await new HttpClient(clientHandler).PostAsJsonAsync("https://localhost:7055/api/accounts/login", request);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadFromJsonAsync<Response<LoginResponse>>();

            HttpContext.Session.SetString("AccessToken", responseContent.Data.AccessToken);
            
            return RedirectToPage("Index");
        }

        return StatusCode((int)response.StatusCode);
    }
}