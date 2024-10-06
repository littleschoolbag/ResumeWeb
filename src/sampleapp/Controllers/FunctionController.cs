using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using sampleapp.Models;

namespace sampleapp.Controllers;

public class FunctionController : Controller
{
    private readonly ILogger<FunctionController> _logger;

    public FunctionController(ILogger<FunctionController> logger)
    {
        _logger = logger;
    }

    public IActionResult Nowadays()
    {
        return View();
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
