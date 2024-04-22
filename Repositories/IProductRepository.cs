using LuxeIQ.Models;

namespace LuxeIQ.Repositories
{
    public interface IProductRepository
    {
        Task UpdateProductAttribute(string productAttribute, Int64 productId);
        Task<Products> FindByManufacturingId(Int64 key);
        Task<List<Products>> GetAll();
        Task Reload(Products entity);
        Task<Products> Add(Products item);
        Task<Products> Find(Int64 key);
        void Remove(Int64 id);
        Task Update(Products item);
    }
}
