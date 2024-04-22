using LuxeIQ.Models;
using LuxeIQ.ViewModels;

namespace LuxeIQ.Extensions
{
    public static class WholesalerHQExtension
    {
        public static WholesalerHQ ToModel(this WholesalerHQImport wholesaler)
        {
            if (wholesaler == null)
                return default(WholesalerHQ);

            return new WholesalerHQ
            {
                salesRegion = wholesaler.salesRegion,
                salesTerritory = wholesaler.salesTerritory,
                accountNo = wholesaler.accountNo,
                customer = wholesaler.customer,
                address = wholesaler.address,
                city = wholesaler.city,
                state = wholesaler.state,
                zipcode = wholesaler.zipcode,
                country = wholesaler.country,
                phone = wholesaler.phone,
                fax = wholesaler.fax
            };
        }
    }
}
