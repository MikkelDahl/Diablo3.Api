using Diablo3.Api.Core.Models;

namespace Diablo3.Api.Core.Services;

public interface IItemCache
{
    Task<Item> GetAsync(string name);
    Task<ICollection<Item>> GetAllAsync();
}