using System.ComponentModel.DataAnnotations;

namespace sampleapp.Models;

public class LoginViewModel()
{
    [Required(ErrorMessage ="请填入账号")]
    public string Username {get; set;}
    
    [Required(ErrorMessage ="请填入密码")]
    public string Password {get; set;}

}

