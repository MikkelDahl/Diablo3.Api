using System;
using Diablo3.Api.Core.Models;

namespace Diablo3.Api.Core.Test.Builders
{
    public class RiftInformationBuilder
    {
        private int level;
        private TimeSpan clearTime;
        private DateTime clearDate;
        private ItemSet itemSet;
    
        private RiftInformationBuilder(int Level, TimeSpan ClearTime, DateTime ClearDate, ItemSet ItemSet)
        {
            level = Level;
            clearTime = ClearTime;
            clearDate = ClearDate;
            itemSet = ItemSet;
        }

        public static RiftInformationBuilder WithDefaultValues() => new RiftInformationBuilder(100, TimeSpan.FromMinutes(10), DateTime.UtcNow, ItemSet.Raekor);
        public RiftInformationBuilder WithLevel(int level) => new RiftInformationBuilder(level, clearTime, clearDate, itemSet);
        public RiftInformationBuilder WithItemSet(ItemSet itemSet) => new RiftInformationBuilder(level, clearTime, clearDate, itemSet);

        public RiftInformation Build() => new RiftInformation(level, clearTime, clearDate, itemSet);
    }
}