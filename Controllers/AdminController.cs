using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Client.Models;
using Client.Extensions;

namespace Client.Controllers;

[Route("[controller]")]
public class AdminController : Controller
{
    private readonly RESTAPIService _restapiService;

    public AdminController(RESTAPIService restapiService)
    {
        _restapiService = restapiService;
    }

    [Route("Dashboard")]
    public async Task<IActionResult> Dashboard()
    {
        return View();
    }

    [Route("Items/Found")]
    public async Task<IActionResult> FoundItems()
    {
        var response = await _restapiService.Get<Response<IEnumerable<Item>>>("items/found", HttpContext.Session.GetString("AccessToken"));

        return View("Items/Found", response.Data);
    }

    [Route("Items/RequestFound")]
    public async Task<IActionResult> RequestFoundItems()
    {
        var response = await _restapiService.Get<Response<IEnumerable<RequestFound>>>("items/request-found", HttpContext.Session.GetString("AccessToken"));

        return View("Items/RequestFound", response.Data);
    }
}