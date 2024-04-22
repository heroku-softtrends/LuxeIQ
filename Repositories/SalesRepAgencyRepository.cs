using LuxeIQ.Data;
using LuxeIQ.Models;
using Microsoft.EntityFrameworkCore;

namespace LuxeIQ.Repositories
{
    public class SalesRepAgencyRepository : ISalesRepAgencyRepository, IDisposable
    {
        private LuxeIQContext _context;
        public SalesRepAgencyRepository(LuxeIQContext context)
        {
            _context = context;
        }
        public async Task<SalesRepAgency> Add(SalesRepAgency item)
        {
            //Get AuthTokens by auth_id
            var entity = await Find(item.salesRepAgencyId);
            if (entity == null)
            {
                _context.SalesRepAgency.Add(item);
                _context.SaveChanges();
            }
            else
            {
                entity.salesRepAgencyName = item.salesRepAgencyName;
                entity.address1 = item.address1;
                entity.address2 = item.address2;
                entity.city = item.city;
                entity.state = item.state;
                entity.zipcode = item.zipcode;
                entity.country = item.country;
                entity.administrator = item.administrator;
                entity.administratorMail = item.administratorMail;
                entity.territoryName = item.territoryName;
                entity.territoryNumber = item.territoryNumber;
                _context.Entry(entity).State = EntityState.Modified;
                _context.SaveChanges();
            }

            return null;
        }

        public void Dispose()
        {

        }

        public async Task<SalesRepAgency> Find(long key)
        {
            try
            {
                var entity =   _context.SalesRepAgency.FirstOrDefault(p => p.salesRepAgencyId == key);
                return entity;
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }

        public async Task<List<SalesRepAgency>> GetAllSalesrepAgencyByManufactuerId(Int64 manufacturerId)
        {

            var lsalesRepAgency = from showroom in _context.SalesRepAgency
                                  join territory in _context.ManufacturerTerritories on showroom.territoryNumber equals territory.repCode
                                  where territory.manufacturerId == manufacturerId && showroom.territoryNumber == territory.repCode
                                  select showroom;

            return lsalesRepAgency.ToList();
        }

        public async Task<List<SalesRepAgency>> GetAll()
        {
            return _context.SalesRepAgency.OrderBy(p => p.salesRepAgencyName).ToList();
        }

        public async Task Reload(SalesRepAgency entity)
        {
            if (entity != null)
            {
                await _context.Entry(entity).ReloadAsync();
            }
        }

        public void Remove(long id)
        {
            //Get Manufacturers by auth_id
            var entity = Find(id).Result;
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Deleted;
                _context.SaveChanges();
            }
        }


        public async Task Update(SalesRepAgency item)
        {
            //Get AuthTokens by auth_id
            var entity = await Find(item.salesRepAgencyId);
            if (entity != null)
            {
                entity.salesRepAgencyName = item.salesRepAgencyName;
                entity.address1 = item.address1;
                entity.address2 = item.address2;
                entity.city = item.city;
                entity.state = item.state;
                entity.zipcode = item.zipcode;
                entity.country = item.country;
                entity.administrator = item.administrator;
                entity.administratorMail = item.administratorMail;
                entity.territoryName = item.territoryName;
                entity.territoryNumber = item.territoryNumber;
                _context.Entry(entity).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }
    }
}