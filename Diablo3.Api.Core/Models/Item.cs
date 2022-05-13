namespace Diablo3.Api.Core.Models;

public class Item
{
    public Item(string name, string iconUri)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        IconUri = iconUri ?? throw new ArgumentNullException(nameof(iconUri));
    }

    public string Name { get; }
    public string IconUri { get; }
}