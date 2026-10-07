namespace DigitaltDodsbo.Core.Models;

public class Provider
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Category { get; set; } = "";
    public bool RequiresDeathCertificate { get; set; }
    public bool RequiresEstateInventory { get; set; }
    public string ContactMethod { get; set; } = "";
    public string ContactDetails { get; set; } = "";
    public string ProcessNotes { get; set; } = "";
}
