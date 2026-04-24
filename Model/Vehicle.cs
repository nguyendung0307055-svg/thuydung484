using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace thuydung484.Model
{
    public class Vehicle
    {
        [Key]
        public int Id { get; set; }  // Primary Key
        [Required]
        [StringLength(100)]
        public string name { get; set; }
        [Required]
        [RegularExpression(@"^[0-9A-Z-]+$",
            ErrorMessage = "Biển số không hợp lệ")]
        public string license_plate { get; set; }
        [Required]
        public int vehicle_type_id { get; set; }

        [ForeignKey("vehicle_type_id")]
        public VehicleType? VehicleType { get; set; }
        [Required]
        public int engine_capacity { get; set; } // Dung tích động cơ (cc)

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal price_per_hour { get; set; }  // Giá theo giờ

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal price_per_day { get; set; }  // Giá theo ngày

        [Required]
        [RegularExpression("^(Available|Rented|Maintenance)$",
            ErrorMessage = "Status phải là Available / Rented / Maintenance")]
        public string status { get; set; }
        // FK Branch
        [Required]
        public int branch_id { get; set; }

        [ForeignKey("branch_id")]
        public Branch? Branch { get; set; }

        public ICollection<Vehicle_Image>? Vehicle_Images { get; set; }

        public ICollection<VehicleMaintenance>? VehicleMaintenances { get; set; }

    }
}
