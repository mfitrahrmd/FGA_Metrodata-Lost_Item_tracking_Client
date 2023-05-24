using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Client.Models;
using Client.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Authorization;

namespace Client.Controllers;

[Route("[controller]")]
[Authorize]
public class UserController : Controller
{
    private readonly RESTAPIService _restapiService;
    
    public UserController(RESTAPIService restapiService)
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
}