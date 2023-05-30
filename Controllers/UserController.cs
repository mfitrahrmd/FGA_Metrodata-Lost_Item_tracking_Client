using System.Collections;
using System.Diagnostics;
using System.Dynamic;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using Client.Controllers.common;
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
        var response = await _restapiService.Get<Response<IEnumerable<FoundItem>>>("items/found", HttpContext.Session.GetString("AccessToken"));
        
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

        return View("Items/Found", model);
    }

    [Route("Items/My/RequestFound")]
    public async Task<IActionResult> MyRequestFoundItems()
    {
        var response = await _restapiService.Get<Response<IEnumerable<MyRequestFoundItem>>>("items/my/request-found", HttpContext.Session.GetString("AccessToken"));
        
        return View("Items/RequestFound", response.Data);
    }
    
    [Route("Items/My/RequestClaim")]
    public async Task<IActionResult> MyRequestClaimItems()
    {
        var response = await _restapiService.Get<Response<IEnumerable<MyRequestClaimItem>>>("items/my/request-claim", HttpContext.Session.GetString("AccessToken"));

        return View("Items/RequestClaim", response.Data);
    }

    [HttpPost]
    [Route("Items/RequestFound")]
    public async Task<IActionResult> ProcessRequestFound([FromForm] RequestFound request)
    {
        var response = await _restapiService.PostForm<RequestFound, Response<Item>>("items/request-found", request, HttpContext.Session.GetString("AccessToken"));

        return RedirectToAction("MyRequestFoundItems");
    }
    
    [HttpPost]
    [Route("Items/RequestClaim")]
    public async Task<IActionResult> ProcessRequestClaim([FromForm] requestClaim request)
    {
        var response = await _restapiService.Post<Response<Item>>($"items/found/{request.ItemId}/request-claim", HttpContext.Session.GetString("AccessToken"));
        
        return RedirectToAction("MyRequestClaimItems");
    }
    
    [Route("Profile")]
    public async Task<IActionResult> Profile()
    {
        var employeeData = await _restapiService.Get<Response<Employee>>($"employees/{User.FindFirstValue(ClaimTypes.Sid)}", HttpContext.Session.GetString("AccessToken"));
        var departmentsData = await _restapiService.Get<Response<IEnumerable<Department>>>($"departments", HttpContext.Session.GetString("AccessToken"));
        var genderForms = new List<GenderForm>
        {
            new GenderForm("Female", "F"),
            new GenderForm("Male", "M")
        };
        
        return View(new Tuple<Employee, IEnumerable<Department>, IEnumerable<GenderForm>>(employeeData.Data, departmentsData.Data, genderForms));
    }

    public record requestClaim(string ItemId);
}