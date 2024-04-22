using LuxeIQ.Models;
using LuxeIQ.ViewModels;

namespace LuxeIQ.Extensions
{
    public static class WholesalerShowroomExtensions
    {
        public static WholesalerShowrooms ToModel(this WholesalerShowroomImport showrooms)
        {
            if (showrooms == null)
                return default(WholesalerShowrooms);

            return new WholesalerShowrooms
            {
                businessName = showrooms.businessName,
                //wholesalerAccountNo = showrooms.wholesalerAccountNo,
                address1 = showrooms.address1,
                address2 = showrooms.address2,
                city = showrooms.city,
                state = showrooms.state,
                zipcode = showrooms.zipcode,
                country = showrooms.country,
                phoneNumber=showrooms.phoneNumber,
                contactName = showrooms.contactName,
                contactMail=showrooms.contactMail,
                branchNumber=showrooms.branchNumber,
                manufacturerAccountNo=showrooms.manufacturerAccountNo,
                buyingMultiplier=showrooms.buyingMultiplier,
                territoryName=showrooms.territoryName,
                territoryNumber=showrooms.territoryNumber,
                salesAgency=showrooms.salesAgency,
                salesRep=showrooms.salesRep
            };
        }
    }
}
