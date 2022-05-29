namespace Diablo3.Api.Core.Models.DTOs
{
    internal class ItemDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }
        public string SetName { get; set; }
        public string SetDescription { get; set; }
        public Attributes Attributes { get; set; }

        public Item ToItem()
        {
            var effectText = Color == "green"
                ? SetName + ": " + SetDescription
                : Attributes.secondary.Last().text;
            
            return new Item(Name, $"http://media.blizzard.com/d3/icons/items/large/{Icon}.png", effectText);
        }
    }
}