namespace Diablo3.Api.Core.Models.DTOs;

public class AccountHeroDataObject
{
    private string Id { get; set; }
    private string Name { get; set; }
    private string Class { get; set; }
    private int ParagonLevel { get; set; }
    private bool Hardcore { get; set; }
    private bool Seasonal { get; set; }
}