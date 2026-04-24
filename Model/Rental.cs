using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

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
        [RegularExpression("^(HOUR|DAY)$", ErrorMessage = "Chỉ được HOUR hoặc DAY")]
        public string rent_type { get; set; }
        [Required]
        public DateTime start_time { get; set; }  // Bắt đầu thuê

        [Required]
        public DateTime expected_end_time { get; set; }  // Dự kiến trả

        public DateTime? actual_end_time { get; set; }  // Trả thực tế (nullable)

        [Column(TypeName = "decimal(10,2)")]
        public decimal? total_amount { get; set; }  // Tổng tiền
        [Required]
        [RegularExpression("^(Active|Completed|Cancelled)$", ErrorMessage = "Status không hợp lệ")]
        public string? status { get; set; }
        // Navigation properties (quan hệ)
        [ForeignKey("customer_id")]
        public Customer? Customer { get; set; }

        [ForeignKey("vehicle_id")]
        public Vehicle? Vehicle { get; set; }
        public ICollection<Penalty>? Penalties { get; set; }

        public Rental_Detail? Rental_Detail { get; set; }
    }
}