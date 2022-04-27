namespace Diablo3.Api.Core.Models.DTOs
{
    public class LeaderBoardEntryObject
    {
        public LeaderBoardEntryObject()
        {
        }

        public LeaderBoardEntryObject(List<PlayerDto> players, List<Data> riftData)
        {
            player = players;
            data = riftData;
        }

        public List<PlayerDto> player { get; set; }
        public List<Data> data { get; set; }
    }
}