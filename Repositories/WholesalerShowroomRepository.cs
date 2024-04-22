using LuxeIQ.Data;
using LuxeIQ.Models;
using Microsoft.EntityFrameworkCore;

namespace LuxeIQ.Repositories
{
    public class WholesalerShowroomRepository : IWholesalerShowroomRepository, IDisposable
    {

        private LuxeIQContext _context;
        public WholesalerShowroomRepository(LuxeIQContext context)
        {
            _context = context;
        }
        public async Task<WholesalerShowrooms> Add(WholesalerShowrooms item)
        {
            //Get AuthTokens by auth_id
            var entity = await Find(item.showroomId);
            if (entity == null)
            {
                _context.WholesalerShowrooms.Add(item);
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
                entity.phoneNumber = item.phoneNumber;
                entity.wholesalerAccountNo = item.wholesalerAccountNo;
                entity.branchNumber = item.branchNumber;
                entity.contactName = item.contactName;
                entity.contactMail = item.contactMail;
                entity.buyingMultiplier = item.buyingMultiplier;
                entity.territoryName = item.territoryName;
                entity.territoryNumber = item.territoryNumber;
                entity.salesAgency = item.salesAgency;
                entity.salesRep = item.salesRep;
                _context.Entry(entity).State = EntityState.Modified;
                _context.SaveChanges();
            }

            return null;
        }

        public void Dispose()
        {

        }

        public async Task<List<WholesalerShowrooms>> GetAllShowroomByManufactuerId(Int64 manufacturerId)
        {
            var lshowroom = from showroom in _context.WholesalerShowrooms
                            join wholesaler in _context.Wholesalers on showroom.wholesalerId equals wholesaler.wholesalerId
                            where wholesaler.manufacturerId == manufacturerId && showroom.wholesalerId == wholesaler.wholesalerId
                            select showroom;

            return lshowroom.ToList();
        }
        public async Task<WholesalerShowrooms> Find(Int64 key)
        {
            try
            {
                var entity =  _context.WholesalerShowrooms.FirstOrDefault(p => p.showroomId == key);
                return entity;
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }

        public async Task<List<WholesalerShowrooms>> GetAll()
        {
            return _context.WholesalerShowrooms.OrderBy(p => p.businessName).ToList();
        }

        public async Task<List<WholesalerShowrooms>> GetAllByWholesalerId(Int64 wholesalerId)
        {
            return await _context.WholesalerShowrooms.Where(x => x.wholesalerId == wholesalerId).OrderBy(p => p.businessName).ToListAsync();
        }
        public async Task Reload(WholesalerShowrooms entity)
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

        public async Task Update(WholesalerShowrooms item)
        {
            //Get AuthTokens by auth_id
            var entity = await Find(item.showroomId);
            if (entity != null)
            {
                entity.businessName = item.businessName;
                entity.address1 = item.address1;
                entity.address2 = item.address2;
                entity.city = item.city;
                entity.state = item.state;
                entity.zipcode = item.zipcode;
                entity.country = item.country;
                entity.phoneNumber = item.phoneNumber;
                entity.wholesalerAccountNo = item.wholesalerAccountNo;
                entity.branchNumber = item.branchNumber;
                entity.contactName = item.contactName;
                entity.contactMail = item.contactMail;
                entity.buyingMultiplier = item.buyingMultiplier;
                entity.territoryName = item.territoryName;
                entity.territoryNumber = item.territoryNumber;
                entity.salesAgency = item.salesAgency;
                entity.salesRep = item.salesRep;
                _context.Entry(entity).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }
    }
}
