namespace Diablo3.Api.Core.Models.DTOs;

public class PlayerDto
{
    public List<Data> data { get; set; }
    
    public Player ToPlayer()
    {
        Enum.TryParse<PlayerClass>(data[2].String, out var playerClass);
        var playerName = data[0].String[..^5];
        var battleTag =  data[0].String.Substring(data[0].String.Length - 5, 5);
        return new Player(playerName, battleTag, playerClass , data[5].number);
    }
}