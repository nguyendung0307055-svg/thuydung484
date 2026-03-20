using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace thuydung484.Model
{
    public class Rental
    {
        [Key]
        public int id { get; set; }  // Primary Key

        [Required]
        public int customer_id { get; set; }  // FK -> Customer

        [Required]
        public int vehicle_id { get; set; }  // FK -> Vehicle

        [Required]
        [StringLength(10)]
        public string rent_type { get; set; }  // HOUR / DAY

        [Required]
        public DateTime start_time { get; set; }  // Bắt đầu thuê

        [Required]
        public DateTime expected_end_time { get; set; }  // Dự kiến trả

        public DateTime? actual_end_time { get; set; }  // Trả thực tế (nullable)

        [Column(TypeName = "decimal(10,2)")]
        public decimal? total_amount { get; set; }  // Tổng tiền

        [Required]
        [StringLength(20)]
        public string status { get; set; }  // Active / Completed / Cancelled

        // Navigation properties (quan hệ)
        [ForeignKey("customer_id")]
        public Customer Customer { get; set; }

        [ForeignKey("vehicle_id")]
        public Vehicle Vehicle { get; set; }
    }
}