using LuxeIQ.Data;
using LuxeIQ.Models;
using Microsoft.EntityFrameworkCore;

namespace LuxeIQ.Repositories
{
    public class WholesalerHQRepository : IWholesalerHQRepository, IDisposable
    {
        private LuxeIQContext _context;
        public WholesalerHQRepository(LuxeIQContext context)
        {
            _context = context;
        }
        public async Task<WholesalerHQ> Add(WholesalerHQ item)
        {
            //Get AuthTokens by auth_id
            var entity = await Find(item.wholesalerHQId);
            if (entity == null)
            {
                _context.WholesalerHQ.Add(item);
                _context.SaveChanges();
            }
            else
            {
                entity.salesRegion = item.salesRegion;
                entity.salesTerritory = item.salesTerritory;
                entity.accountNo = item.accountNo;
                entity.customer = item.customer;
                entity.address = item.address;
                entity.city = item.city;
                entity.state = item.state;
                entity.zipcode = item.zipcode;
                entity.country = item.country;
                entity.phone = item.phone;
                entity.fax = item.fax;

                _context.Entry(entity).State = EntityState.Modified;
                _context.SaveChanges();
            }

            return null;
        }

        public void Dispose()
        {

        }

        public async Task<WholesalerHQ> Find(Int64 key)
        {
            try
            {
                var entity =  _context.WholesalerHQ.FirstOrDefault(p => p.wholesalerHQId == key);
                return entity;
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }

        public async Task<List<WholesalerHQ>> FindbyWholesaler(Int64 key)
        {
            if (_context.WholesalerHQ != null && await _context.WholesalerHQ.Where(p => p.wholesalerId == key).CountAsync() > 0)
            {
                var entity = _context.WholesalerHQ.Where(p => p.wholesalerId == key).ToList();
                //get latest database value
                return entity;
            }
            return null;
        }
        public async Task<List<WholesalerHQ>> GetAllWholesalerHQsByManufactuerId(Int64 manufacturerId)
        {

            var lwholesalerHQs = from showroom in _context.WholesalerHQ
                                 join wholesaler in _context.Wholesalers on showroom.wholesalerId equals wholesaler.wholesalerId
                                 where wholesaler.manufacturerId == manufacturerId && showroom.wholesalerId == wholesaler.wholesalerId
                                 select showroom;

            return lwholesalerHQs.ToList();
        }
        public async Task<List<WholesalerHQ>> GetAll()
        {
            return _context.WholesalerHQ.OrderBy(p => p.customer).ToList();
        }

        public async Task Reload(WholesalerHQ entity)
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

        public async Task Update(WholesalerHQ item)
        {
            //Get AuthTokens by auth_id
            var entity = await Find(item.wholesalerHQId);
            if (entity != null)
            {
                entity.salesRegion = item.salesRegion;
                entity.salesTerritory = item.salesTerritory;
                entity.accountNo = item.accountNo;
                entity.customer = item.customer;
                entity.address = item.address;
                entity.city = item.city;
                entity.state = item.state;
                entity.zipcode = item.zipcode;
                entity.country = item.country;
                entity.phone = item.phone;
                entity.fax = item.fax;
                _context.Entry(entity).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }
    }
}
