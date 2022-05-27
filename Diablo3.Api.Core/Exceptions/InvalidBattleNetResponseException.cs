namespace Diablo3.Api.Core.Exceptions;

public class InvalidBattleNetResponseException : Exception
{
    public InvalidBattleNetResponseException()
    {
    }

    public InvalidBattleNetResponseException(Exception e, string request) : base(
        $"BattleNet API error: {e.Message}. Failed request: {request}")
    {
    }

    public InvalidBattleNetResponseException(string request) : base(
        $"BattleNet did not return a valid response. Failed request: {request}")
    {
    }
}