namespace Diablo3.Api.Core.Models.DTOs
{
    internal class ItemDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public ItemAttributes Attributes { get; set; }

        public Item ToItem() => new Item(Name, $"http://media.blizzard.com/d3/icons/items/large/{Icon}.png", Attributes.secondary.Last().text);

    }
}