namespace Diablo3.Api.Core.Models.DTOs
{
    public class LeaderBoardEntryObject
    {
        public LeaderBoardEntryObject()
        {
        }
        public List<PlayerDataObject> player { get; set; }
        public List<Data> data { get; set; }
    }
}