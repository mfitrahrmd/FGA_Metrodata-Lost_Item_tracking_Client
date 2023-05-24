using Client.Extensions;
using Client.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client.Pages.Admin.Items;

public class Found : PageModel
{
    private readonly RESTAPIService _restapiService;
    public IEnumerable<Models.Item> Data { get; private set; }

    public Found(RESTAPIService restapiService)
    {
        _restapiService = restapiService;
    }
    
    public async Task OnGet()
    {
        var response = await _restapiService.Get<Response<IEnumerable<Item>>>("items/found", HttpContext.Session.GetString("AccessToken"));

        Data = response.Data;
    }
}