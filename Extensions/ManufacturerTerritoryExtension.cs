using LuxeIQ.Models;
using LuxeIQ.ViewModels;

namespace LuxeIQ.Extensions
{
    public static class ManufacturerTerritoryExtension
    {
        public static ManufacturerTerritories ToModel(this ManufacturerTerritoryImport mTerritory)
        {
            if (mTerritory == null)
                return default(ManufacturerTerritories);

            return new ManufacturerTerritories
            {
                repCode = mTerritory.repCode, 
                salesAgency = mTerritory.salesAgency,
                salesRegion = mTerritory.salesRegion,
                salesTerritory = mTerritory.salesTerritory
            };
        }
    }
}

