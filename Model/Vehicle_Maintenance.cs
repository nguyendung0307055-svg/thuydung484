using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace thuydung484.Model
{
    [Table("Vehicle_Maintenance")]
    public class VehicleMaintenance
    {
        [Key]
        public int id { get; set; }

        // ==========================
        // FK → Vehicle
        // ==========================
        [Required]
        public int vehicle_id { get; set; }

        [ForeignKey("vehicle_id")]
        public Vehicle? Vehicle { get; set; }  // <-- thêm ? để nullable

        // ==========================
        // Thông tin bảo trì
        // ==========================
        [Required]
        [StringLength(255)]
        public string description { get; set; }

        [Required]
        public DateTime start_date { get; set; }

        public DateTime? end_date { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? cost { get; set; }

        [Required]
        [StringLength(20)]
        public string status { get; set; }  // InProgress / Completed

        // ==========================
        // FK → User (người tạo)
        // ==========================
        [Required]
        public int user_id { get; set; }

        [ForeignKey("user_id")]
        public User? User { get; set; }  // <-- thêm ? để nullable

        // ==========================
        // Metadata
        // ==========================
        [Required]
        public DateTime created_at { get; set; }
    }
}