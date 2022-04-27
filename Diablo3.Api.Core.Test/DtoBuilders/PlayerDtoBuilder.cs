using System.Collections.Generic;
using Diablo3.Api.Core.Models.DTOs;

namespace Diablo3.Api.Core.Test.DtoBuilders
{
    public class PlayerDtoBuilder
    {
        private readonly List<Data> data;
        private static string name;
        private static int paragon;

        private PlayerDtoBuilder(List<Data> data)
        {
            this.data = data;
        }
    
        public static PlayerDtoBuilder WithDefaultValues()
        {
            name = "testBarb";
            paragon = 100;
            var battleTag = "#1234";
        
            var dataDtos = new List<Data>
            {
                new Data
                {
                    id = "",
                    String = name + battleTag,
                    number = 0,
                    timestamp = 0
                },
                new Data(),
                new Data
                {
                    id = "",
                    String = "barbarian",
                    number = 0,
                    timestamp = 0
                }, 
                new Data(),
                new Data(),
                new Data
                {
                    id = "",
                    String = "",
                    number = paragon,
                    timestamp = 0
                }
            };
        
            return new PlayerDtoBuilder(dataDtos);
        }

        public PlayerDtoBuilder WithPlayerName(string name)
        {
            var battleTag = "#1234";
            var dataDtos = new List<Data>
            {
                new Data
                {
                    id = "",
                    String = name + battleTag,
                    number = 0,
                    timestamp = 0
                },
                new Data(),
                new Data
                {
                    id = "",
                    String = "barbarian",
                    number = 0,
                    timestamp = 0
                }, 
                new Data(),
                new Data(),
                new Data
                {
                    id = "",
                    String = "",
                    number = paragon,
                    timestamp = 0
                }
            };
        
            return new PlayerDtoBuilder(dataDtos);
        }
    
        public PlayerDtoBuilder WithParagon(int paragon)
        {
            var battleTag = "#1234";
            var dataDtos = new List<Data>
            {
                new Data
                {
                    id = "",
                    String = name + battleTag,
                    number = 0,
                    timestamp = 0
                },
                new Data(),
                new Data
                {
                    id = "",
                    String = "barbarian",
                    number = 0,
                    timestamp = 0
                }, 
                new Data(),
                new Data(),
                new Data
                {
                    id = "",
                    String = "",
                    number = paragon,
                    timestamp = 0
                }
            };
        
            return new PlayerDtoBuilder(dataDtos);
        }

        public PlayerDto Build() => new PlayerDto { data = data };
    }
}