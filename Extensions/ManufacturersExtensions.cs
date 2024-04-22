using LuxeIQ.Models;
using LuxeIQ.ViewModels;
using Mono.TextTemplating;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace LuxeIQ.Extensions
{
    public static class ManufacturersExtensions
    {
        public static Manufacturers ToModel(this ManufacturerImport manufacturer)
        {
            if (manufacturer == null)
                return default(Manufacturers);

            return new Manufacturers
            {
                businessName = manufacturer.businessName,
                address1 = manufacturer.address1,
                address2 = manufacturer.address2,
                city = manufacturer.city,
                state = manufacturer.state,
                zipcode = manufacturer.zipcode,
                country = manufacturer.country,
                phone = manufacturer.phone,
                contactName = manufacturer.contactName,
                contactEmail = manufacturer.contactEmail,
                corporateAdmin = manufacturer.corporateAdmin,
                corporateAdminEmail = manufacturer.corporateAdminEmail,
                salesAdmin = manufacturer.salesAdmin,
                salesAdminEmail = manufacturer.salesAdminEmail,
                otherAdmin = manufacturer.otherAdmin,
                otherAdminEmail = manufacturer.otherAdminEmail
            };
        }
        public static List<Manufacturers> ToListModel(this List<ManufacturerImport> manufacturer)
        {
            List<Manufacturers> manufacturerList = new List<Manufacturers>();
            if (manufacturer == null)
                return default(List<Manufacturers>);

            foreach (ManufacturerImport mi in manufacturer)
            {
                Manufacturers man = new Manufacturers();
                man.businessName = mi.businessName;
                man.address1 = mi.address1;
                man.address2 = mi.address2;
                man.city = mi.city;
                man.state = mi.state;
                man.zipcode = mi.zipcode;
                man.country = mi.country;
                man.phone = mi.phone;
                man.contactName = mi.contactName;
                man.contactEmail = mi.contactEmail;
                man.corporateAdmin = mi.corporateAdmin;
                man.corporateAdminEmail = mi.corporateAdminEmail;
                man.salesAdmin = mi.salesAdmin;
                man.salesAdminEmail = mi.salesAdminEmail;
                man.otherAdmin = mi.otherAdmin;
                man.otherAdminEmail = mi.otherAdminEmail;
                manufacturerList.Add(man);
            }
            return manufacturerList;
        }
    }
}
