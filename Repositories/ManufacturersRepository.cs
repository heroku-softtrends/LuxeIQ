using System;
using System.Collections.Generic;
using System.Linq;
using LuxeIQ.Models;
using LuxeIQ.Data;
using Microsoft.EntityFrameworkCore;
using LuxeIQ.ViewModels;
using System.Threading.Tasks;
using System.Threading;
using System.Text;

namespace LuxeIQ.Repositories
{
    public class ManufacturersRepository : IManufacturersRepository, IDisposable
    {
        private LuxeIQContext _context;
        public ManufacturersRepository(LuxeIQContext context)
        {
            _context = context;
        }

        public async Task<Manufacturers> UpdateProductAttributes(Manufacturers item)
        {
            //Get AuthTokens by auth_id
            var entity = await Find(item.manufacturerId);
            if (entity != null)
            {
                entity.product_attributes = item.product_attributes;
                _context.Entry(entity).State = EntityState.Modified;
                _context.SaveChanges();
            }

            return item;
        }

        /// <summary>
        /// Method: Add
        /// Description: It is used to add new token to Manufacturers table
        /// </summary>
        /// <param name="item"></param>
        public async Task<Manufacturers> Add(Manufacturers item)
        {
            //Get AuthTokens by auth_id
            var entity = await Find(item.manufacturerId);
            if (entity == null)
            {
                _context.Manufacturers.Add(item);
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
                entity.phone = item.phone;
                entity.contactName = item.contactName;
                entity.contactEmail = item.contactEmail;
                entity.corporateAdmin = item.corporateAdmin;
                entity.corporateAdminEmail = item.corporateAdminEmail;
                entity.salesAdmin = item.salesAdmin;
                entity.salesAdminEmail = item.salesAdminEmail;
                entity.otherAdmin = item.otherAdmin;
                entity.otherAdminEmail = item.otherAdminEmail;
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
        public async Task<List<Manufacturers>> GetAll()
        {
            return _context.Manufacturers.OrderBy(p => p.businessName).ToList();
        }


        /// <summary>
        /// Method: Find
        /// Description: It is used to get Manufactures by id
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<Manufacturers> Find(Int64 key)
        {
            try
            {
                var entity = _context.Manufacturers.FirstOrDefault(p => p.manufacturerId == key);
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
        public async Task Reload(Manufacturers entity)
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

        public async Task Update(Manufacturers item)
        {
            //Get AuthTokens by auth_id
            //var entity = await Find(item.manufacturerId);
            //if (entity != null)
            //{
            //    entity.businessName = item.businessName;
            //    entity.address1 = item.address1;
            //    entity.address2 = item.address2;
            //    entity.city = item.city;
            //    entity.state = item.state;
            //    entity.zipcode = item.zipcode;
            //    entity.country = item.country;
            //    entity.phone = item.phone;
            //    entity.contactName = item.contactName;
            //    entity.contactEmail = item.contactEmail;
            //    entity.corporateAdmin = item.corporateAdmin;
            //    entity.corporateAdminEmail = item.corporateAdminEmail;
            //    entity.salesAdmin = item.salesAdmin;
            //    entity.salesAdminEmail = item.salesAdminEmail;
            //    entity.otherAdmin = item.otherAdmin;
            //    entity.otherAdminEmail = item.otherAdminEmail;
            //    _context.Entry(entity).State = EntityState.Modified;
            //    _context.SaveChanges();
            //}
        }


        public void Dispose()
        {

        }
    }
}