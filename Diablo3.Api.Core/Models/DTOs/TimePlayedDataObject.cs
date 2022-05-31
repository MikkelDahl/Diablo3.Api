using System.Text.Json.Serialization;

namespace Diablo3.Api.Core.Models.DTOs;

public class TimePlayedDataObject
{
    [JsonPropertyName("demon-hunter")]
    public double DemonHunter { get; set; }
    public double Barbarian { get; set; }

    [JsonPropertyName("witch-doctor")]
    public double WitchDoctor { get; set; }
    public double Necromancer { get; set; }
    public double Wizard { get; set; }
    public double Monk { get; set; }
    public double Crusader { get; set; }
}