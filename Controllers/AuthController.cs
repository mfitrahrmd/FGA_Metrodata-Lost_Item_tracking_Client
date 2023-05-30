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

    public async Task<IActionResult> Register()
    {
        var departmentsData = await _restapiService.Get<Response<IEnumerable<Department>>>($"departments", HttpContext.Session.GetString("AccessToken"));

        return View(departmentsData.Data);
    }

    [HttpPost]
    public async Task<IActionResult> ProcessLogin([FromForm] LoginRequest request)
    {
        var response = await _restapiService.Login(request);

        HttpContext.Session.SetString("AccessToken", response.Data.AccessToken);

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Logout()
    {
        HttpContext.Session.Remove("AccessToken");
        
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> ProcessRegister([FromForm] RegisterRequest request)
    {
        var response = await _restapiService.Register(request);
        
        return RedirectToAction("Login");
    }
}