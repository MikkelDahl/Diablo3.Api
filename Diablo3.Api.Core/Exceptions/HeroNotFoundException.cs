namespace Diablo3.Api.Core.Exceptions;

public class HeroNotFoundException : Exception
{
    public HeroNotFoundException(int id, string battleTag) : base(
        $"BattleNet API error: No hero found. Request params: ID: {id}, BattleTag: {battleTag}")
    {
    }
}