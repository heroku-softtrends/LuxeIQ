using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace LuxeIQ.Models
{
    public class ManufacturerTerritories
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public Int64 territoryId { get; set; }
        public Int64 manufacturerId { get; set; }
        public Int64? repCode { get; set; } 
        public string? salesRegion { get; set; }
        public string? salesAgency { get; set; }
        public string? salesTerritory { get; set; }

        [ForeignKey("manufacturerId")]
        public virtual Manufacturers? Manufacturers { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
