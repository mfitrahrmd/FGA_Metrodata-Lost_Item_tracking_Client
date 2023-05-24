using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Client.Models;
using Client.Extensions;

namespace WebApp.Controllers;

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
}