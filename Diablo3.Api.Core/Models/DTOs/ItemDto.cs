namespace Diablo3.Api.Core.Models.DTOs;

public class ItemDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Icon { get; set; }

    public Item ToItem() => new Item(Name, $"http://media.blizzard.com/d3/icons/items/large/{Icon}.png");
}