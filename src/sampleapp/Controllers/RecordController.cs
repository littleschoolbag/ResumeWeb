using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using sampleapp.Models;

namespace sampleapp.Controllers;

public class RecordController : Controller
{
    private readonly ILogger<RecordController> _logger;

    public RecordController(ILogger<RecordController> logger)
    {
        _logger = logger;
    }

    public IActionResult Experience()
    {
        return View();
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
