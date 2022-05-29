namespace Diablo3.Api.Core.Models
{
    public class Item
    {
        public Item(string name, string iconUri, string effect)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            IconUri = iconUri ?? throw new ArgumentNullException(nameof(iconUri));
            Effect = effect ?? throw new ArgumentNullException(nameof(effect));
        }

        public string Name { get; }
        public string IconUri { get; }
        public string Effect { get; }
    }
}