namespace Lab2_Web.Models.DTO;

public class CountryDtoWrite
{
    public string Name { get; set; }
    public string LeaderName { get; set; }
    public string[] ContinentNames { get; set; }
    public string BlockName { get; set; }
    public int MilitaryStrength { get; set; }
}