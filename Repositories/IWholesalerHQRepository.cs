using LuxeIQ.Models;

namespace LuxeIQ.Repositories
{
    public interface IWholesalerHQRepository
    {
        Task<List<WholesalerHQ>> GetAll();
        Task Reload(WholesalerHQ entity);
        Task<WholesalerHQ> Add(WholesalerHQ item);
        Task<WholesalerHQ> Find(Int64 key);
        Task<List<WholesalerHQ>> FindbyWholesaler(Int64 key);
        void Remove(Int64 id);
        Task Update(WholesalerHQ item);
        Task<List<WholesalerHQ>> GetAllWholesalerHQsByManufactuerId(Int64 manufacturerId);
    }
}
