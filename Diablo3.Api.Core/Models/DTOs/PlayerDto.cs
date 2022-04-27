namespace Diablo3.Api.Core.Models.DTOs
{
    public class PlayerDto
    {
        public List<Data> data { get; set; }
    
        public Player ToPlayer()
        {
            var upperCaseStringRepresentation = char.ToUpper(data[2].String[0]) + data[2].String.Substring(1, data[2].String.Length - 1);
            if (upperCaseStringRepresentation.Contains("hunter") || upperCaseStringRepresentation.Contains("doctor"))
                upperCaseStringRepresentation = upperCaseStringRepresentation[0] 
                                                + upperCaseStringRepresentation.Substring(1, 4)
                                                + char.ToUpper(upperCaseStringRepresentation[6])
                                                + upperCaseStringRepresentation.Substring(7, upperCaseStringRepresentation.Length - 7);

            Enum.TryParse<PlayerClass>(upperCaseStringRepresentation, out var playerClass);
            var playerName = data[0].String[..^5];
            var battleTag =  data[0].String.Substring(data[0].String.Length - 5, 5);
            return new Player(playerName, battleTag, playerClass , data[5].number);
        }
    }
}