using LuxeIQ.Models;
using LuxeIQ.ViewModels;

namespace LuxeIQ.Extensions
{
    public static class SalesRepExtension
    {
        public static SalesRepAgency ToModel(this SalesRepAgencyImport salesRep)
        {
            if (salesRep == null)
                return default(SalesRepAgency);

            return new SalesRepAgency
            {
                salesRepAgencyName = salesRep.salesRepAgencyName,
                address1 = salesRep.address1,
                address2 = salesRep.address2,
                city = salesRep.city,
                state = salesRep.state,
                zipcode = salesRep.zipcode,
                country = salesRep.country,
                administrator=salesRep.administrator,
                administratorMail=salesRep.administratorMail,
                territoryName=salesRep.territoryName,
                territoryNumber=salesRep.territoryNumber
            };
        }
    }
}
