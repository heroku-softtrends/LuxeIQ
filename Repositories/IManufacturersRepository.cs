using System.Collections.Generic;
using LuxeIQ.Models;
using LuxeIQ.ViewModels;
using System.Threading.Tasks;

namespace LuxeIQ.Repositories
{
    public interface IManufacturersRepository
    {
        Task<Manufacturers> UpdateProductAttributes(Manufacturers item);
        Task<List<Manufacturers>> GetAll();
        Task Reload(Manufacturers entity);
        Task<Manufacturers> Add(Manufacturers item);
        Task<Manufacturers> Find(Int64 key);
        void Remove(Int64 id);
        Task Update(Manufacturers item);
    }
}
