using System.ComponentModel.DataAnnotations;

namespace Lab2_Web.Models;

public class Leader
{
    public int Id { get; set; }
    
    [Required]
    [Display(Name = "Лідер")]
    public string Name { get; set; }
 
    [Required(ErrorMessage = "Немає лідеру без країни")]
    public Country Country { get; set; }
}