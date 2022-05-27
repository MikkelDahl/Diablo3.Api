using Diablo3.Api.Core.Models;

namespace Diablo3.Api.Core.Services
{
    public interface IItemFetcher
    {
        Task<ICollection<Item>> GetAllAsync();
    }
}