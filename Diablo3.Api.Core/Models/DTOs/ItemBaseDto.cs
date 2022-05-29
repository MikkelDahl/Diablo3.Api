namespace Diablo3.Api.Core.Models.DTOs
{
    internal class ItemBaseDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Path { get; set; }

        public ItemBase ToItemBase() => new ItemBase(Name, $"http://media.blizzard.com/d3/icons/items/large/{Icon}.png", Path);
    }
}