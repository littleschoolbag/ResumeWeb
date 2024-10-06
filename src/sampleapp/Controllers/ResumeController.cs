using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using sampleapp.Models;

namespace sampleapp.Controllers;

public class ResumeController : Controller
{
    private readonly ILogger<ResumeController> _logger;

    public ResumeController(ILogger<ResumeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Personal()
    {
        return View();
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
