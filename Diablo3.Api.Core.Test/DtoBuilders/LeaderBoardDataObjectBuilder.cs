using System.Collections.Generic;
using Diablo3.Api.Core.Models.DTOs;

namespace Diablo3.Api.Core.Test.DtoBuilders
{
    public class LeaderBoardDataObjectBuilder
    {
        private List<LeaderBoardEntryObject> leaderBoardEntryObjects;

        private LeaderBoardDataObjectBuilder(List<LeaderBoardEntryObject> entryObjects)
        {
            leaderBoardEntryObjects = entryObjects;
        }

        public static LeaderBoardDataObjectBuilder WithDefaultValues()
        {
            var firstEntryplayerDtos = new List<PlayerDto>()
            {
                PlayerDtoBuilder.WithDefaultValues().Build(),
                PlayerDtoBuilder.WithDefaultValues().WithPlayerName("herbert1").Build(),
                PlayerDtoBuilder.WithDefaultValues().WithPlayerName("herbert2").Build(),
                PlayerDtoBuilder.WithDefaultValues().WithPlayerName("testermann").Build(),
                PlayerDtoBuilder.WithDefaultValues().WithPlayerName("test").Build(),
            };

            var riftData = new List<Data>
            {
                new Data(10, 1000),
                new Data(10, 2500),
                new Data(11, 1000),
                new Data(8, 1000),
                new Data(11, 2000),
                new Data(11, 2000)
            };
        
            var riftData2 = new List<Data>
            {
                new Data(10, 1000),
                new Data(10, 2500),
                new Data(11, 1000),
                new Data(8, 1000),
                new Data(8, 1000),
                new Data(11, 2000)
            };
        
            var riftData3 = new List<Data>
            {
                new Data(10, 1000),
                new Data(10, 2500),
                new Data(11, 1000),
                new Data(8, 1000),
                new Data(8, 1000),
                new Data(11, 2000)
            };
        
            var riftData4 = new List<Data>
            {
                new Data(10, 1000),
                new Data(10, 2500),
                new Data(11, 1000),
                new Data(8, 1000),
                new Data(8, 1000),
                new Data(11, 2000)
            };
        
            var riftData5 = new List<Data>
            {
                new Data(10, 1000),
                new Data(10, 2500),
                new Data(11, 1000),
                new Data(8, 1000),
                new Data(8, 1000),
                new Data(11, 2000)
            };

            var entries = new List<LeaderBoardEntryObject>
            {
                new LeaderBoardEntryObject(firstEntryplayerDtos, riftData)
            };
            
            return new LeaderBoardDataObjectBuilder(entries);
        }

        public LeaderBoardDataObject Build() => new() {row = leaderBoardEntryObjects};
    }
}