namespace Lab2_Web.Models;

public class ContinentCountry
{
    public int Id { get; set; }
    
    public int ContinentId { get; set; }
    
    public int CountryId { get; set; }
    
    
    public Continent Continent { get; set; }
    
    public Country Country { get; set; }
}