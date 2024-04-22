using LuxeIQ.Models;

namespace LuxeIQ.Repositories
{
    public interface IManufacturersTerritoryRepository
    {
        Task<List<ManufacturerTerritories>> GetAll();
        Task Reload(ManufacturerTerritories entity);
        Task<ManufacturerTerritories> Add(ManufacturerTerritories item);
        Task<ManufacturerTerritories> Find(Int64 key);
        void Remove(Int64 id);
        Task Update(ManufacturerTerritories item);
        Task<List<ManufacturerTerritories>> GetAllByManufacturerId(Int64 id);
    }
}
