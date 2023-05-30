using System.Diagnostics;
using System.Security.Claims;
using Client.Controllers.common;
using Microsoft.AspNetCore.Mvc;
using Client.Models;
using Client.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Authorization;

namespace Client.Controllers;

[Route("[controller]")]
[Authorize(Roles = "Admin")]
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
        var pendingRequestFound = await _restapiService.Get<Response<IEnumerable<RequestFoundItem>>>(QueryHelpers.AddQueryString("items/request-found", "status", "pending"), HttpContext.Session.GetString("AccessToken"));
        var pendingRequestClaim = await _restapiService.Get<Response<IEnumerable<RequestClaimItem>>>(QueryHelpers.AddQueryString("items/request-claim", "status", "pending"), HttpContext.Session.GetString("AccessToken"));
        return View(new Tuple<int, int>(pendingRequestFound.Data.Count(), pendingRequestClaim.Data.Count()));
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

    [Route("Items/Found")]
    public async Task<IActionResult> FoundItems()
    {
        var response = await _restapiService.Get<Response<IEnumerable<FoundItem>>>("items/found", HttpContext.Session.GetString("AccessToken"));

        if (Request.Headers["Accept"].Equals("application/json"))
        {
            return Ok(response.Data);
        }

        return View("Items/Found", response.Data);
    }
    
    [Route("Items/Found/All")]
    public async Task<IActionResult> FoundItemsAll()
    {
        var response = await _restapiService.Get<Response<IEnumerable<FoundItem>>>("items/found/all", HttpContext.Session.GetString("AccessToken"));

        if (Request.Headers["Accept"].Equals("application/json"))
        {
            return Ok(response.Data);
        }

        return View("Items/Found", response.Data);
    }
    
    [Route("Items/Claimed")]
    public async Task<IActionResult> ClaimedItems()
    {
        var response = await _restapiService.Get<Response<IEnumerable<ClaimedItem>>>("items/claimed", HttpContext.Session.GetString("AccessToken"));
        
        if (Request.Headers["Accept"].Equals("application/json"))
        {
            return Ok(response.Data);
        }

        return View("Items/Claimed", response.Data);
    }

    [Route("Items/RequestFound")]
    public async Task<IActionResult> RequestFoundItems()
    {
        var response = await _restapiService.Get<Response<IEnumerable<RequestFoundItem>>>(QueryHelpers.AddQueryString("items/request-found", Request.Query), HttpContext.Session.GetString("AccessToken"));

        return View("Items/RequestFound", response.Data);
    }

    [Route("Items/RequestClaim")]
    public async Task<IActionResult> RequestClaimItems()
    {
        var response = await _restapiService.Get<Response<IEnumerable<RequestClaimItem>>>(QueryHelpers.AddQueryString("items/request-claim", Request.Query), HttpContext.Session.GetString("AccessToken"));

        return View("Items/RequestClaim", response.Data);
    }

    [HttpPost]
    [Route("Items/RequestFound/Approve")]
    public async Task<IActionResult> ProcessApproveRequestFound([FromForm] updateRequest request)
    {
        var response = await _restapiService.Patch<UpdateStatusRequest, Response<Status>>($"items/request-found/{request.RequestId}/approve", new UpdateStatusRequest{Message = request.Message}, HttpContext.Session.GetString("AccessToken"));

        return RedirectToAction("RequestFoundItems");
    }
    
    [HttpPost]
    [Route("Items/RequestFound/Reject")]
    public async Task<IActionResult> ProcessRejectRequestFound([FromForm] updateRequest request)
    {
        var response = await _restapiService.Patch<UpdateStatusRequest, Response<Status>>($"items/request-found/{request.RequestId}/reject", new UpdateStatusRequest{Message = request.Message}, HttpContext.Session.GetString("AccessToken"));

        return RedirectToAction("RequestFoundItems");
    }
    
    [HttpPost]
    [Route("Items/RequestClaim/Approve")]
    public async Task<IActionResult> ProcessApproveRequestClaim([FromForm] updateRequest request)
    {
        await _restapiService.Patch<UpdateStatusRequest, Response<Status>>($"items/found/request-claim/{request.RequestId}/approve", new UpdateStatusRequest{Message = request.Message}, HttpContext.Session.GetString("AccessToken"));

        return RedirectToAction("RequestClaimItems");
    }
    
    [HttpPost]
    [Route("Items/RequestClaim/Reject")]
    public async Task<IActionResult> ProcessRejectRequestClaim([FromForm] updateRequest request)
    {
        await _restapiService.Patch<UpdateStatusRequest, Response<Status>>($"items/found/request-claim/{request.RequestId}/reject", new UpdateStatusRequest{Message = request.Message}, HttpContext.Session.GetString("AccessToken"));

        return RedirectToAction("RequestClaimItems");
    }

    [HttpPost]
    [Route("Items/ConfirmFound")]
    public async Task<IActionResult> ProcessConfirmFound([FromForm] confirmRequest request)
    {
        var response = await _restapiService.Post<Response<ItemAction>>($"items/request-found/{request.RequestId}/confirm-found", HttpContext.Session.GetString("AccessToken"));

        return RedirectToAction("RequestFoundItems");
    }
    
    [HttpPost]
    [Route("Items/ConfirmClaimed")]
    public async Task<IActionResult> ProcessConfirmClaimed([FromForm] confirmRequest request)
    {
        var response = await _restapiService.Post<Response<ItemAction>>($"items/request-claim/{request.RequestId}/confirm-claimed", HttpContext.Session.GetString("AccessToken"));

        return RedirectToAction("RequestClaimItems");
    }

    public record updateRequest(Guid RequestId, string Message);

    public record confirmRequest(Guid RequestId);
}