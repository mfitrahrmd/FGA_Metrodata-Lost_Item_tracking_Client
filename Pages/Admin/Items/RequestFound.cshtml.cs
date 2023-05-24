using Client.Extensions;
using Client.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace Client.Pages.Admin.Items;

public class RequestFound : PageModel
{
    private readonly RESTAPIService _restapiService;
    public IEnumerable<Models.RequestFound> Data { get; private set; }

    public RequestFound(RESTAPIService restapiService)
    {
        _restapiService = restapiService;
    }

    public async Task OnGet()
    {
        var endpoint = "items/request-found";

        var response = await _restapiService.Get<Response<IEnumerable<Models.RequestFound>>>(QueryHelpers.AddQueryString(endpoint, Request.Query), HttpContext.Session.GetString("AccessToken"));

        Data = response.Data;
    }

    public async Task<IActionResult> OnPostApprove()
    {
        Console.WriteLine("approved");

        return RedirectToAction(nameof(OnGet));
    }

    public async Task OnPostReject()
    {
        Console.WriteLine("rejected");
    }
}