using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LuxeIQ.Models
{
    public class WholesalerShowrooms
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 showroomId { get; set; }
        public Int64 wholesalerId { get; set; } 
        public string? wholesalerAccountNo { get; set; }
        public string? businessName { get; set; }
        public string? address1 { get; set; }
        public string? address2 { get; set; }
        public string? city { get; set; }
        public string? state { get; set; }
        public string? zipcode { get; set; }
        public string? country { get; set; }
        public string? phoneNumber { get; set; }
        public string? contactName { get; set; }
        public string? contactMail { get; set; }
        public string? branchNumber { get; set; }
        public string? manufacturerAccountNo { get; set; }
        public string? buyingMultiplier { get; set; }
        public Int64? territoryNumber { get; set; }
        public string? territoryName { get; set; }
        public string? salesAgency { get; set; }
        public string? salesRep { get; set; }

        [ForeignKey("wholesalerId")]
        public virtual Wholesalers? Wholesalers { get; set; }
       
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
