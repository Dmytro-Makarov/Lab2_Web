using System.ComponentModel.DataAnnotations;

namespace Lab2_Web.Models;

public class Block
{
    public Block()
    {
        Countries = new List<Country>();
    }

    public int Id { get; set; }

    [Required]
    [Display(Name = "Воєнний Блок")]
    public string Name { get; set; }

    public ICollection<Country?>? Countries { get; set; }
}