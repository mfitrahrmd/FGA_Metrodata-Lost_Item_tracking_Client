using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Client.Models;
using Client.Extensions;

namespace Client.Controllers;

public class AuthController : Controller
{
    private readonly RESTAPIService _restapiService;

    public AuthController(RESTAPIService restapiService)
    {
        _restapiService = restapiService;
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ProcessLogin([FromForm] LoginRequest request)
    {
        System.Console.WriteLine("received");
        var response = await _restapiService.Login(request);

        HttpContext.Session.SetString("AccessToken", response.Data.AccessToken);

        return RedirectToAction("Index", "Home");
    }
}