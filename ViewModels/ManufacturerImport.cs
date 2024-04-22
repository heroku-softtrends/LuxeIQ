using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LuxeIQ.ViewModels
{
    public class ManufacturerImport
    {
        public string businessName { get; set; }
        public string? address1 { get; set; }
        public string? address2 { get; set; }
        public string? city { get; set; }
        public string? state { get; set; }
        public string? zipcode { get; set; }
        public string? country { get; set; }
        public string? phone { get; set; }
        public string? contactName { get; set; }
        public string? contactEmail { get; set; }
        public string? corporateAdmin { get; set; }
        public string? corporateAdminEmail { get; set; }
        public string? salesAdmin { get; set; }
        public string? salesAdminEmail { get; set; }
        public string? otherAdmin { get; set; }
        public string? otherAdminEmail { get; set; }
    }
}
