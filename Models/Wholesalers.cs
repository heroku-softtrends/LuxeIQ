using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LuxeIQ.Models
{
    public class Wholesalers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 wholesalerId { get; set; }
        public Int64 manufacturerId { get; set; }
        public string? businessName { get; set; }
        public string? address1 { get; set; }
        public string? address2 { get; set; }
        public string? city { get; set; }
        public string? state { get; set; }
        public string? zipcode { get; set; }
        public string? country { get; set; }
        public string? purchasingMultiplier { get; set; }

        [ForeignKey("manufacturerId")]
        public virtual Manufacturers? Manufacturers { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
