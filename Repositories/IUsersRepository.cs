using LuxeIQ.Models;
using LuxeIQ.ViewModels;

namespace LuxeIQ.Repositories
{
    public interface IUsersRepository
    {
        Task<List<UserViewModel>> GetAllManufacturingAdmins();
        Task<List<UserViewModel>> GetAllSalesReps(Int64? ManufacturerId);
        Task<List<Users>> GetAll();
        Task<List<UserViewModel>> GetAllWithDetails();
        Task Reload(Users entity);
        Task<Users> Add(Users item);
        Task<Users> Find(Int64 key);
        Users Login(string username,string password);
        void Remove(Int64 id);
        Task Update(Users item);
        Users FindByEmailId(string key);
        Task<Users> UpdatePassword(Users item);
    }
}
