using Diablo3.Api.Core.Models.DTOs;

namespace Diablo3.Api.Core.Test.Builders;

internal class LeaderBoardDtoBuilder
{
    private LeaderBoardDtoBuilder()
    {
        
    }

    public static LeaderBoardDtoBuilder WithDefaultValues() => new LeaderBoardDtoBuilder();

    public LeaderBoardDataObject Build() => new LeaderBoardDataObject();
}