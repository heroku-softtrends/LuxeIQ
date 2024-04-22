using LuxeIQ.Models;

namespace LuxeIQ.Repositories
{
    public interface ISalesRepAgencyRepository
    {
        Task<List<SalesRepAgency>> GetAll();
        Task Reload(SalesRepAgency entity);
        Task<SalesRepAgency> Add(SalesRepAgency item);
        Task<SalesRepAgency> Find(long key);
        void Remove(long id);
        Task Update(SalesRepAgency item);
        Task<List<SalesRepAgency>> GetAllSalesrepAgencyByManufactuerId(Int64 manufacturerId);
    }
}
