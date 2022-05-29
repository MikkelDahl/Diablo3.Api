namespace Diablo3.Api.Core.Models
{
    public class ItemBase
    {
        public ItemBase(string name, string iconUri, string path)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            IconUri = iconUri ?? throw new ArgumentNullException(nameof(iconUri));
            Path = path ?? throw new ArgumentNullException(nameof(path));
        }

        public string Name { get; }
        public string IconUri { get; }
        public string Path { get; }
    }
}