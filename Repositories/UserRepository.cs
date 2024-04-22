using LuxeIQ.Common;
using LuxeIQ.Data;
using LuxeIQ.Models;
using LuxeIQ.ViewModels;
using Microsoft.EntityFrameworkCore;
using Mono.TextTemplating;
using NuGet.Packaging.Signing;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq.Expressions;
using System.Net;
using System.Reflection.Emit;
using System.Xml.Linq;

namespace LuxeIQ.Repositories
{
    public class UserRepository : IUsersRepository, IDisposable
    {
        private LuxeIQContext _context;
        public UserRepository(LuxeIQContext context)
        {
            _context = context;
        }

        public async Task<List<UserViewModel>> GetAllSalesReps(Int64? ManufacturerId)
        {

            var uvm = from user in _context.Users
                      join agency in _context.SalesRepAgency on user.salesRepAgencyId equals agency.salesRepAgencyId
                      where user.userType == "SA" && user.ManufacturerId== ManufacturerId
                      select new UserViewModel
                      {
                          userId = user.userId,
                          name = user.name,
                          userType = user.userType,
                          address = user.address,
                          city = user.city,
                          state = user.state,
                          zipCode = user.zipCode,
                          country = user.country,
                          phone = user.phone,
                          email = user.email,
                          whatsappMobile = user.mobile,
                          activationStatus = user.activated,
                          password = user.password,
                          ManufacturerId = user.ManufacturerId,
                          salesRepAgencyId = user.salesRepAgencyId,
                          SalesRepAgencyName = agency != null ? agency.salesRepAgencyName : ""
                      };

            return uvm.ToList();
        }
        public async Task<List<UserViewModel>> GetAllManufacturingAdmins()
        {

            var uvm = from user in _context.Users
                      join manufacturer in _context.Manufacturers on user.ManufacturerId equals manufacturer.manufacturerId
                      where user.userType == "M"
                      select new UserViewModel
                      {
                          userId = user.userId,
                          name = user.name,
                          userType = user.userType,
                          address = user.address,
                          city = user.city,
                          state = user.state,
                          zipCode = user.zipCode,
                          country = user.country,
                          phone = user.phone,
                          email = user.email,
                          whatsappMobile = user.mobile,
                          activationStatus = user.activated,
                          password = user.password,
                          ManufacturerId = user.ManufacturerId,
                          salesRepAgencyId = user.salesRepAgencyId,
                          ManufacturerName = manufacturer != null ? manufacturer.businessName : ""
                      };

            return uvm.ToList();
        }
        public async Task<Users> UpdatePassword(Users item)
        {
            //Get AuthTokens by auth_id
            var entity = await Find(item.userId);
            if (entity != null)
            {
                entity.password = item.password;
                entity.mobile = item.mobile;
                entity.phone = item.phone;
                entity.activated = "YES";
                _context.Entry(entity).State = EntityState.Modified;
                _context.SaveChanges();
            }
            return null;
        }

        public async Task<Users> Add(Users item)
        {
            //Get AuthTokens by auth_id
            var entity = await Find(item.userId);
            if (entity == null)
            {
                _context.Users.Add(item);
                _context.SaveChanges();
            }
            else
            {
                if (item.userType == "M")
                {
                    //item.WholesalerId = null;
                    //item.MTerritoryId = null;
                    //item.WholesalerShowroomId = null;
                }
                else if (item.userType == "W")
                {
                    item.ManufacturerId = null;
                    //item.MTerritoryId = null;
                    //item.WholesalerShowroomId = null;
                }
                else if (item.userType == "SA")
                {
                    //item.WholesalerId = null;
                    //item.WholesalerShowroomId = null;
                }
                else if (item.userType == "SH")
                {
                    item.ManufacturerId = null;
                    // item.MTerritoryId = null;
                }
                entity.name = item.name;
                entity.address = item.address;
                entity.userType = item.userType;
                entity.city = item.city;
                entity.state = item.state;
                entity.zipCode = item.zipCode;
                entity.country = item.country;
                entity.phone = item.phone;
                entity.mobile = item.mobile;
                entity.email = item.email;
                entity.ManufacturerId = item.ManufacturerId;
                entity.salesRepAgencyId = item.salesRepAgencyId;
                entity.activated = item.activated;
                _context.Entry(entity).State = EntityState.Modified;
                _context.SaveChanges();
            }

            return null;
        }


        public async Task<Users> Find(Int64 key)
        {
            try
            {
                var entity = _context.Users.FirstOrDefault(p => p.userId == key);
                //get latest database value
                return entity;
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }
        public Users FindByEmailId(string key)
        {
            try
            {
                if (_context.Users != null)
                {
                    Users user = _context.Users.FirstOrDefault(p => p.email == key);
                    if (user != null)
                    {
                        return user;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }
        public Users Login(string username, string password)
        {
            return _context.Users.Where(p => p.email == username && p.password == password).FirstOrDefault();
        }
        public async Task<List<Users>> GetAll()
        {
            return _context.Users.OrderBy(p => p.name).ToList();
        }
        public async Task<List<UserViewModel>> GetAllWithDetails()
        {

            var uvm = from user in _context.Users
                      join manu in _context.Manufacturers on user.ManufacturerId equals manu.manufacturerId
                      select new
                      {
                          userId = user.userId,
                          name = user.name,
                          userType = user.userType,
                          address = user.address,
                          city = user.city,
                          state = user.state,
                          zipCode = user.zipCode,
                          country = user.country,
                          phone = user.phone,
                          email = user.email,
                          password = user.password,
                          ManufacturerId = user.ManufacturerId,
                          salesRepAgencyId = user.salesRepAgencyId,
                          ManufacturerName = manu.businessName
                      };
            List<UserViewModel> userviewmodel = new List<UserViewModel>();
            foreach (var user in uvm)
            {
                UserViewModel _uvm = new UserViewModel();
                _uvm.userId = user.userId;
                _uvm.name = user.name;
                _uvm.userType = user.userType;
                _uvm.address = user.address;
                _uvm.city = user.city;
                _uvm.state = user.state;
                _uvm.zipCode = user.zipCode;
                _uvm.country = user.country;
                _uvm.phone = user.phone;
                _uvm.email = user.email;
                _uvm.password = user.password;
                _uvm.ManufacturerId = user.ManufacturerId;
                _uvm.salesRepAgencyId = user.salesRepAgencyId;
                _uvm.ManufacturerName = user.ManufacturerName;
                userviewmodel.Add(_uvm);
            }

            return userviewmodel;

        }

        public async Task Reload(Users entity)
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

        public async Task Update(Users item)
        {
            //Get AuthTokens by auth_id
            var entity = await Find(item.userId);
            if (entity != null)
            {
                entity.name = item.name;
                entity.address = item.address;
                entity.userType = item.userType;
                entity.city = item.city;
                entity.state = item.state;
                entity.zipCode = item.zipCode;
                entity.country = item.country;
                entity.phone = item.phone;
                entity.email = item.email;
                _context.Entry(entity).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }
        public void Dispose()
        {

        }

    }
}
