using LuxeIQ.Models;

namespace LuxeIQ.Repositories
{
    public interface IWholesalerShowroomRepository
    {
        Task<List<WholesalerShowrooms>> GetAll();
        Task Reload(WholesalerShowrooms entity);
        Task<WholesalerShowrooms> Add(WholesalerShowrooms item);
        Task<WholesalerShowrooms> Find(Int64 key);
        void Remove(Int64 id);
        Task Update(WholesalerShowrooms item);
        Task<List<WholesalerShowrooms>> GetAllByWholesalerId(Int64 wholesalerId);
        Task<List<WholesalerShowrooms>> GetAllShowroomByManufactuerId(Int64 manufacturerId);
    }
}
