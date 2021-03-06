namespace Diablo3.Api.Core.Models.DTOs;

internal class SeasonObject
{
    public int SeasonId { get; set; }
    public int ParagonLevel { get; set; }
    public int ParagonLevelHardcore { get; set; }
    public TimePlayedDataObject TimePlayed { get; set; }
}