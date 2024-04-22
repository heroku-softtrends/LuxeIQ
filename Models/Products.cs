using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace LuxeIQ.Models
{
    public class Products
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 productId { get; set; }
        public Int64 manufacturerId { get; set; }

        public string? tableName { get; set; }
        public string? productAttributes { get; set; }

        [ForeignKey("manufacturerId")]
        public virtual Manufacturers? Manufacturers { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }

    }
}
