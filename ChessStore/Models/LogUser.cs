#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace ChessStore.Models;
public class LoginUser
{
    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    [Display(Name = "Email")]
    public string LEmail { get; set; }
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Password is required")]
    [Display(Name = "Password")]
    public string LPassword { get; set; }
}