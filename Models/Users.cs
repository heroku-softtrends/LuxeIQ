using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LuxeIQ.Models
{
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 userId { get; set; }
        public string? userType { get; set; }
        public Int64? ManufacturerId { get; set; } 
        public Int64? salesRepAgencyId { get; set; }
        public string? password { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }

        public string? name { get; set; }
        public string? email { get; set; }
        public string? phone { get; set; }
        public string? mobile { get; set; }
        public string? activated { get; set; }
        public string? address { get; set; }
        public string? city { get; set; }
        public string? state { get; set; }
        public string? zipCode { get; set; }
        public string? country { get; set; }
       
    }
}
