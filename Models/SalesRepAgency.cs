using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LuxeIQ.Models
{
    public class SalesRepAgency
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 salesRepAgencyId { get; set; }
        public Int64? territoryNumber { get; set; }
        public string? salesRepAgencyName { get; set; }
        public string? address1 { get; set; }
        public string? address2 { get; set; }
        public string? city { get; set; }
        public string? state { get; set; }
        public string? zipcode { get; set; }
        public string? country { get; set; }
        public string? administrator { get; set; }
        public string? administratorMail { get; set; } 
        public string? territoryName { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
 
    }
}
