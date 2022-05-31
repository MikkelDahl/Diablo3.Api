using Newtonsoft.Json;

namespace Diablo3.Api.Core.Models.DTOs;

public class TimePlayedDataObject
{
    [JsonProperty("demon-hunter")]
    public double DemonHunter { get; set; }
    public double barbarian { get; set; }

    [JsonProperty("witch-doctor")]
    public double WitchDoctor { get; set; }
    public double necromancer { get; set; }
    public double wizard { get; set; }
    public double monk { get; set; }
    public double crusader { get; set; }
}