using LuxeIQ.Data;
using LuxeIQ.Models;
using Microsoft.EntityFrameworkCore;

namespace LuxeIQ.Repositories
{
    public class WholesalerRepository : IWholesalerRepository, IDisposable
    {
        private LuxeIQContext _context;
        public WholesalerRepository(LuxeIQContext context)
        {
            _context = context;
        }
        public async Task<Wholesalers> Add(Wholesalers item)
        {
            //Get AuthTokens by auth_id
            var entity = await Find(item.wholesalerId);
            if (entity == null)
            {
                _context.Wholesalers.Add(item);
                _context.SaveChanges();
            }
            else
            {
                entity.businessName = item.businessName;
                entity.address1 = item.address1;
                entity.address2 = item.address2;
                entity.city = item.city;
                entity.state = item.state;
                entity.zipcode = item.zipcode;
                entity.country = item.country;
                entity.purchasingMultiplier = item.purchasingMultiplier;
                _context.Entry(entity).State = EntityState.Modified;
                _context.SaveChanges();
            }

            return null;
        }

        public void Dispose()
        {

        }

        public async Task<Wholesalers> Find(Int64 key)
        {
            try
            {
                var entity = _context.Wholesalers.FirstOrDefault(p => p.wholesalerId == key);
                return entity;
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }

        public async Task<List<Wholesalers>> GetAllbyManufacturer(Int64 manufacturerId)
        {
            return _context.Wholesalers.Where(p => p.manufacturerId == manufacturerId).ToList();
        }
        public async Task<List<Wholesalers>> GetAll()
        {
            return _context.Wholesalers.OrderBy(p => p.businessName).ToList();
        }

        public async Task Reload(Wholesalers entity)
        {
            if (entity != null)
            {
                await _context.Entry(entity).ReloadAsync();
            }
        }

        public void Remove(Int64 id)
        {
            //Get Manufacturers by auth_id
            var entity = Find(id).Result;
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Deleted;
                _context.SaveChanges();
            }
        }

        public async Task Update(Wholesalers item)
        {
            //Get AuthTokens by auth_id
            var entity = await Find(item.wholesalerId);
            if (entity != null)
            {
                entity.businessName = item.businessName;
                entity.address1 = item.address1;
                entity.address2 = item.address2;
                entity.city = item.city;
                entity.state = item.state;
                entity.zipcode = item.zipcode;
                entity.country = item.country;
                entity.purchasingMultiplier = item.purchasingMultiplier;
                _context.Entry(entity).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }
    }
}
