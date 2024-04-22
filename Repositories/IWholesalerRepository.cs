using LuxeIQ.Models;

namespace LuxeIQ.Repositories
{
    public interface IWholesalerRepository
    {
        Task<List<Wholesalers>> GetAll();
        Task Reload(Wholesalers entity);
        Task<Wholesalers> Add(Wholesalers item);
        Task<Wholesalers> Find(Int64 key);
        void Remove(Int64 id);
        Task Update(Wholesalers item);
        Task<List<Wholesalers>> GetAllbyManufacturer(Int64 manufacturerId);
    }
}
