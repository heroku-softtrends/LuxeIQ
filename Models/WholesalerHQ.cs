using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LuxeIQ.Models
{
    public class WholesalerHQ
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 wholesalerHQId { get; set; }
        public Int64 wholesalerId { get; set; }
        public int? accountNo { get; set; }
        public int? salesRegion { get; set; }
        public int? salesTerritory { get; set; }
        public string? customer { get; set; }
        public string? address { get; set; }
        public string? city { get; set; }
        public string? state { get; set; }
        public string? zipcode { get; set; }
        public string? country { get; set; }
        public string? phone { get; set; }
        public string? fax { get; set; }

        [ForeignKey("wholesalerId")]
        public virtual Wholesalers? Wholesalers { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
