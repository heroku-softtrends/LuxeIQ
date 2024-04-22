using LuxeIQ.Data;
using LuxeIQ.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration.Utils;
using Mono.TextTemplating;
using System.Drawing;

namespace LuxeIQ.Repositories
{
    public class ProductRepository : IProductRepository, IDisposable
    {
        private LuxeIQContext _context;
        public ProductRepository(LuxeIQContext context)
        {
            _context = context;
        }
        public async Task<Products> Add(Products product)
        {
            //Get AuthTokens by auth_id
            var entity = await Find(product.productId);
            if (entity == null)
            {
                _context.Products.Add(product);
                _context.SaveChanges();
            }
            else
            {
                entity.tableName = product.tableName;
                entity.productAttributes = product.productAttributes;
                _context.Entry(entity).State = EntityState.Modified;
                _context.SaveChanges();
            }
            return null;
        }

        public void Dispose()
        {

        }
        public async Task<Products> FindByManufacturingId(Int64 key)
        {
            if (_context.Products != null && await _context.Products.Where(p => p.manufacturerId == key).CountAsync() > 0)
            {
                var entity = await _context.Products.FirstOrDefaultAsync(p => p.manufacturerId == key);
                return entity;
            }
            return null;
        }

        public async Task UpdateProductAttribute(string productAttribute,Int64 productId)
        {
            //Get AuthTokens by auth_id
            var entity = await Find(productId);
            if (entity != null)
            {

                entity.productAttributes = productAttribute;
                _context.Entry(entity).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }

        public async Task<Products> Find(Int64 key)
        {
            if (_context.Products != null && await _context.Products.Where(p => p.productId == key).CountAsync() > 0)
            {
                var entity = await _context.Products.FirstOrDefaultAsync(p => p.productId == key);
                //get latest database value
                if (entity != null)
                    await Reload(entity);
                return entity;
            }
            return null;
        }

        public async Task<List<Products>> GetAll()
        {
            return _context.Products.OrderBy(p => p.tableName).ToList();
        }

        public async Task Reload(Products entity)
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

        public async Task Update(Products product)
        {
            //Get AuthTokens by auth_id
            var entity = await Find(product.productId);
            if (entity != null)
            {

                entity.tableName = product.tableName;
                entity.productAttributes = product.productAttributes;
                _context.Entry(entity).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }
    }
}
