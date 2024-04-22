using System.ComponentModel.DataAnnotations;

namespace LuxeIQ.ViewModels
{
    public class UserViewModel
    {

        public Int64 userId { get; set; }
        public Int64? salesRepAgencyId { get; set; }

        [Required, Display(Name = "Name")]
        public string? name { get; set; }
        [Required, Display(Name = "Email")]
        public string? email { get; set; }
        public string? userType { get; set; }
        public string? address { get; set; }
        public string? city { get; set; }
        public string? state { get; set; }
        public string? zipCode { get; set; }
        public string? country { get; set; }
        public string? phone { get; set; }

        [Required, Display(Name = "Password")]
        public string? password { get; set; }

        [Required, Display(Name = "Confirm Password")]
        public string? confirmpassword { get; set; }
        public Int64? ManufacturerId { get; set; }
        public Int64? MTerritoryId { get; set; }
        public Int64? WholesalerId { get; set; }
        public Int64? WholesalerShowroomId { get; set; }

        public string ManufacturerName { get; set; }

        public string SalesRepAgencyName { get; set; }
        public string? whatsappMobile { get; set; }
        public string? activationStatus { get; set; }

    }
}
