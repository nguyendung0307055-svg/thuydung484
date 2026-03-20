using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace thuydung484.Model
{
    public class Vehicle
    {
        [Key]
        public int Id { get; set; }  // Primary Key

        [Required]
        [StringLength(20)]
        public string license_plate { get; set; }  // Biển số xe (unique)

        [Required]
        [StringLength(50)]
        public string type { get; set; }  // Loại xe

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal price_per_hour { get; set; }  // Giá theo giờ

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal price_per_day { get; set; }  // Giá theo ngày

        [Required]
        [StringLength(20)]
        public string status { get; set; }  // Available / Rented / Maintenance
    }
}
