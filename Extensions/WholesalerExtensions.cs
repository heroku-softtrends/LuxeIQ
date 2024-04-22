using LuxeIQ.Models;
using LuxeIQ.ViewModels;

namespace LuxeIQ.Extensions
{
    public static class WholesalerExtensions
    {
        public static Wholesalers ToModel(this WholesalerImport wholesaler)
        {
            if (wholesaler == null)
                return default(Wholesalers);

            return new Wholesalers
            {
                businessName = wholesaler.businessName,
                address1 = wholesaler.address1,
                address2 = wholesaler.address2,
                city = wholesaler.city,
                state = wholesaler.state,
                zipcode = wholesaler.zipcode,
                country = wholesaler.country,
                purchasingMultiplier = wholesaler.purchasingMultiplier
               
            };
        }
    }
}
