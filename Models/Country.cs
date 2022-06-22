using System.ComponentModel.DataAnnotations;

namespace Lab2_Web.Models;

public class Country
{
    public Country()
    {
        ContinentsCountries = new List<ContinentCountry>();
    }
    public int Id { get; set; }
    
    [Required]
    [Display(Name = "Країна")]
    public string Name { get; set; }
    
    [Required]
    [Display(Name = "Лідер")]
    public int LeaderId { get; set; }
    
    [Required]
    [Display(Name = "Блок")]
    public int BlockId { get; set; }
    
    [Required]
    public int MilitaryStrength { get; set; }
    
    public Leader Leader { get; set; }
    
    public Block Block { get; set; }
    public ICollection<ContinentCountry> ContinentsCountries { get; set; }

}