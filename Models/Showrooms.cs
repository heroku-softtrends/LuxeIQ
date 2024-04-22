using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LuxeIQ.Models
{
    public class Showrooms
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string? showroomId { get; set; }
        public string? wholesalerId { get; set; } 
        public string? wholesalerTerritoryId { get; set; }
        public string? accountNumber { get; set; }
        public string? branchNumber { get; set; }
        public string? customer { get; set; }
        public string? address { get; set; }
        public string? city { get; set; }
        public string? state { get; set; }
        public string? zipcode { get; set; }
        public string? country { get; set; }
        public string? uniqueCustomerName { get; set; }
        public string? displays { get; set; }
        public string? region { get; set; }
        public string? territoryName { get; set; }
        public string? territory { get; set; }
        public string? salesAgency { get; set; }
        public string? rep { get; set; }
        
    }
}
