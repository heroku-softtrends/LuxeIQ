using LuxeIQ.Models;
using LuxeIQ.ViewModels;

namespace LuxeIQ.Extensions
{
    public static class UserExtension
    {
        public static Users ToModel(this UserViewModel user)
        {
            if (user == null)
                return default(Users);

            return new Users
            { 
                name = user.name,
                userType = user.userType,
                address = user.address,
                city = user.city,
                state = user.state,
                zipCode = user.zipCode,
                country = user.country,
                phone = user.phone,
                mobile= user.whatsappMobile,
                email = user.email,
                password = user.password,
                ManufacturerId = user.ManufacturerId,
                salesRepAgencyId = user.salesRepAgencyId
            };
        }
        public static UserViewModel ToUserViewModel(this Users user)
        {
            if (user == null)
                return default(UserViewModel);

            return new UserViewModel
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
                whatsappMobile=user.mobile,
                email = user.email,
                password = user.password,
                ManufacturerId = user.ManufacturerId,
                salesRepAgencyId=user.salesRepAgencyId
                //MTerritoryId = user.MTerritoryId,
                //WholesalerId = user.WholesalerId,
                //WholesalerShowroomId = user.WholesalerShowroomId
            };
        }
    }
}
