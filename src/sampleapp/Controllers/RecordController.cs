using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using sampleapp.Models;

namespace sampleapp.Controllers;

public class RecordController : Controller
{
    private readonly UserRepository _userRepository;
    private readonly ILogger<RecordController> _logger;

    public RecordController(IConfiguration configuration)
    {
         string connectionString = configuration.GetConnectionString("DefaultConnection");
        _userRepository = new UserRepository(connectionString);
    }
    [HttpGet]
    public IActionResult Experience()
    {
        return View();
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpPost]
    public IActionResult CreateTabTable(string tablename)
    {
        try{
            bool isCreatTab = _userRepository.CreateTabTable(tablename);
            if(isCreatTab == true){
                return Json(new { success = true , message = "创建成功" });
            }
            return Json(new { success = false , message = "创建失败" });
        }
        catch{
            return Json(new { success = false , message = "未知失败" });
        }
    }
    [HttpPost]
    public IActionResult InsertTabData([FromBody] ExperienceViewModel model)
    {
        try{
            (bool result, int id) = _userRepository.InsertTabData(model.tabName, model.dateValue, model.imageUrl, model.description);
            if(result == true){
                return Json(new { success = true , message = "插入成功", id = id });
            }
            return Json(new { success = false , message = "插入失败" });
        }
        catch{
            return Json(new { success = false , message = "未知失败" });
        }
    }


    
    [HttpDelete]
    public IActionResult DeleteTabTable(string tablename)
    {
        try{
            bool isDeleteTab = _userRepository.DeleteTabTable(tablename);
            if(isDeleteTab == true){
                return Json(new { success = true , message = "删除成功" });
            }
            return Json(new { success = false , message = "删除失败" });
        }
        catch{
            return Json(new { success = false , message = "未知失败" });
        }
    }
    
    [HttpDelete]
    public IActionResult DeleteTabData([FromBody] ExperienceViewModel model)
    {
        try{
            bool isDeleteData = _userRepository.DeleteTabData(model.tabName, model.id);
            if(isDeleteData == true){
                return Json(new { success = true , message = "删除成功" });
            }
            return Json(new { success = false , message = "删除失败" });
        }
        catch{
            return Json(new { success = false , message = "未知失败" });
        }
    }
    
    [HttpGet]
    public IActionResult GetTabList()
    {
        return Json(_userRepository.getTabList());
    }

}
