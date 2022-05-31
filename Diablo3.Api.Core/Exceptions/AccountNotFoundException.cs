namespace Diablo3.Api.Core.Exceptions;

public class AccountNotFoundException : Exception
{
    public AccountNotFoundException(string battleTag) : base(
        $"BattleNet API error: No account found for battleTag: {battleTag}")
    {
    }
}