using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Client.Models;
using Client.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Authorization;

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
        var response = await _restapiService.Get<Response<IEnumerable<RequestFound>>>(QueryHelpers.AddQueryString("items/request-found", Request.Query), HttpContext.Session.GetString("AccessToken"));

        return View("Items/RequestFound", response.Data);
    }

    [Route("Items/RequestClaim")]
    public async Task<IActionResult> RequestClaimItems()
    {
        var response = await _restapiService.Get<Response<IEnumerable<RequestClaim>>>(QueryHelpers.AddQueryString("items/request-claim", Request.Query), HttpContext.Session.GetString("AccessToken"));

        return View("Items/RequestClaim", response.Data);
    }

    [HttpPost]
    [Route("Items/RequestFound/Approve")]
    public async Task<IActionResult> ProcessApproveRequestFound([FromForm] updateRequest request)
    {
        var response = await _restapiService.Patch<UpdateRequest, Response<Status>>($"items/request-found/{request.RequestId}/approve", new UpdateRequest{Message = request.Message}, HttpContext.Session.GetString("AccessToken"));

        return RedirectToAction(nameof(RequestFoundItems));
    }
    
    [HttpPost]
    [Route("Items/RequestFound/Reject")]
    public async Task<IActionResult> ProcessRejectRequestFound([FromForm] updateRequest request)
    {
        var response = await _restapiService.Patch<UpdateRequest, Response<Status>>($"items/request-found/{request.RequestId}/reject", new UpdateRequest{Message = request.Message}, HttpContext.Session.GetString("AccessToken"));

        return RedirectToAction(nameof(RequestFoundItems));
    }
    
    [HttpPost]
    [Route("Items/RequestClaim/Approve")]
    public async Task<IActionResult> ProcessApproveRequestClaim([FromForm] updateRequest request)
    {
        var response = await _restapiService.Patch<UpdateRequest, Response<Status>>($"items/request-claim/{request.RequestId}/approve", new UpdateRequest{Message = request.Message}, HttpContext.Session.GetString("AccessToken"));

        return RedirectToAction(nameof(RequestFoundItems));
    }
    
    [HttpPost]
    [Route("Items/RequestClaim/Reject")]
    public async Task<IActionResult> ProcessRejectRequestClaim([FromForm] updateRequest request)
    {
        var response = await _restapiService.Patch<UpdateRequest, Response<Status>>($"items/request-claim/{request.RequestId}/reject", new UpdateRequest{Message = request.Message}, HttpContext.Session.GetString("AccessToken"));

        return RedirectToAction(nameof(RequestFoundItems));
    }

    public record updateRequest(Guid RequestId, string Message);
}