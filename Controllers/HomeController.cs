using System.Diagnostics;
using Client.Extensions;
using Microsoft.AspNetCore.Mvc;
using Client.Models;

namespace Client.Controllers;


public class HomeController : Controller
{
    private readonly RESTAPIService _restapiService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(RESTAPIService restapiService, ILogger<HomeController> logger)
    {
        _restapiService = restapiService;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [Route("/FoundItems")]
    public async Task<IActionResult> FoundItems()
    {
        var response = await _restapiService.Get<Response<IEnumerable<FoundItem>>>("items/found", null);

        if (User.Identity.IsAuthenticated)
        {
            var secResponse = await _restapiService.Get<Response<IEnumerable<MyRequestClaimItem>>>("items/my/request-claim", HttpContext.Session.GetString("AccessToken"));

            var model = from fi in response.Data select new UserFoundItem
            {
                Id = fi.Id,
                Name = fi.Name,
                Description = fi.Description,
                ImagePath = fi.ImagePath,
                IsRequested = secResponse.Data.Select(mrci => mrci.Item.Id).Contains(fi.Id),
                FoundBy = fi.FoundBy,
                FoundAt = fi.FoundAt
            };

            return View("FoundAuth", model);
        }
        
        return View("Found", response.Data);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}