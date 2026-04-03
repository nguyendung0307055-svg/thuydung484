using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace thuydung484.Model
{
    public class Vehicle_Status_Log
    {
        [Key]
        public int id { get; set; }  // Primary Key

        [Required]
        public int vehicle_id { get; set; }  // FK -> Vehicle

        [Required]
        [StringLength(20)]
        public string old_status { get; set; }
        // Trạng thái cũ

        [Required]
        [StringLength(20)]
        public string new_status { get; set; }
        // Trạng thái mới

        [Required]
        public DateTime changed_at { get; set; }
        // Thời điểm thay đổi

        [Required]
        public int changed_by { get; set; }
        // FK -> User

        // Navigation property
        [ForeignKey("vehicle_id")]
        public Vehicle? Vehicle { get; set; }

        [ForeignKey("changed_by")]
        public User? User { get; set; }
    }
}