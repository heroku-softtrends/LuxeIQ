using LuxeIQ.Data;
using LuxeIQ.Models;
using Microsoft.EntityFrameworkCore;

namespace LuxeIQ.Repositories
{
    public class ManufacturersTerritoryRepository : IManufacturersTerritoryRepository, IDisposable
    {
        private LuxeIQContext _context;
        public ManufacturersTerritoryRepository(LuxeIQContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Method: Add
        /// Description: It is used to add new token to Manufacturers table
        /// </summary>
        /// <param name="item"></param>
        public async Task<ManufacturerTerritories> Add(ManufacturerTerritories item)
        {
            //Get AuthTokens by auth_id
            var entity = await Find(item.territoryId);
            if (entity == null)
            {
                _context.ManufacturerTerritories.Add(item);
                _context.SaveChanges();
            }
            else
            {
                entity.manufacturerId = item.manufacturerId;
                entity.repCode = item.repCode;
                entity.salesAgency = item.salesAgency;
                entity.salesRegion = item.salesRegion;

                _context.Entry(entity).State = EntityState.Modified;
                _context.SaveChanges();
            }

            return item;
        }

        /// <summary>
        /// Method: Get
        /// Description: It is used to get Manufactures by id
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<List<ManufacturerTerritories>> GetAll()
        {
            return _context.ManufacturerTerritories.OrderBy(p => p.repCode).ToList();
        }

        /// <summary>
        /// Method: Get
        /// Description: It is used to get Manufactures by id
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<List<ManufacturerTerritories>> GetAllByManufacturerId(Int64 id)
        {
            return await _context.ManufacturerTerritories.Where(x => x.manufacturerId == id).OrderBy(p => p.repCode).ToListAsync();
        }

        /// <summary>
        /// Method: Find
        /// Description: It is used to get Manufactures by id
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<ManufacturerTerritories> Find(Int64 key)
        {
            try
            {
                var entity = _context.ManufacturerTerritories.FirstOrDefault(p => p.territoryId == key);
                return entity;
            }
            catch (Exception ex)
            {
                return null;
            }

            return null;
        }

        /// <summary>
        /// Method: Reload
        /// Description: It is used to reload Manufacturers entity
        /// </summary>
        /// <param name="entity"></param>
        public async Task Reload(ManufacturerTerritories entity)
        {
            if (entity != null)
            {
                await _context.Entry(entity).ReloadAsync();
            }
        }

        /// <summary>
        /// Method: Remove
        /// Description: It is used to delete Manufacturers by manufactureid from Manufacturers table
        /// </summary>
        /// <param name="id"></param>
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
        public async Task Update(ManufacturerTerritories item)
        {
            //Get AuthTokens by auth_id
            var entity = await Find(item.territoryId);
            if (entity != null)
            {

                entity.manufacturerId = item.manufacturerId;
                entity.repCode = item.repCode;
                entity.salesAgency = item.salesAgency;
                entity.salesRegion = item.salesRegion;
                entity.salesTerritory = item.salesTerritory;

                _context.Entry(entity).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }
        public void Dispose()
        {

        }
    }
}