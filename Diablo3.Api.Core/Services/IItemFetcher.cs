using Diablo3.Api.Core.Models;

namespace Diablo3.Api.Core.Services
{
    public interface IItemFetcher
    {
        Task<Item> GetAsync(string name);
        Task<ICollection<Item>> GetAsync(params string[] names);
    }
}