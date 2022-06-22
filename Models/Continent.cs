using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace Lab2_Web.Models;

public class Continent
{
        public Continent()
        {
                ContinentCountries = new List<ContinentCountry>();
        }

        public int Id { get; set; }

        [Required]
        [Display(Name = "Континент/Регіон")]
        public string Name { get; set; }

        public ICollection<ContinentCountry> ContinentCountries { get; set; }
}