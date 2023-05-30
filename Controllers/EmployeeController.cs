using Client.Extensions;
using Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers;

[Route("[controller]")]
[Authorize]
public class EmployeeController : Controller
{
    private readonly RESTAPIService _restapiService;

    public EmployeeController(RESTAPIService restapiService)
    {
        _restapiService = restapiService;
    }
    
    [HttpPost]
    public async Task<IActionResult> UpdateProfile([FromForm] Employee request)
    {
        Console.WriteLine("id " + request.Id);
        Console.WriteLine("nik " + request.Nik);
        await _restapiService.Put<Employee, Response<object>>($"employees/{request.Id}", request, HttpContext.Session.GetString("AccessToken"));

        if (User.IsInRole("Admin"))
        {
            return RedirectToAction("Profile", "Admin");
        }

        return RedirectToAction("Profile", "User");
    }
}