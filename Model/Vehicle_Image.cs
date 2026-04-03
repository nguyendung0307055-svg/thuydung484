using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace thuydung484.Model
{
    public class Vehicle_Image
    {
        [Key]
        public int id { get; set; }  // Primary Key

        [Required]
        public int vehicle_id { get; set; }  // FK -> Vehicle

        [Required]
        [StringLength(255)]
        public string image_url { get; set; }
        // Đường dẫn ảnh

        [Required]
        public bool is_primary { get; set; }
        // Ảnh đại diện (true = ảnh chính)

        [Required]
        public DateTime created_at { get; set; }
        // Ngày tạo

        // Navigation property
        [ForeignKey("vehicle_id")]
        public Vehicle? Vehicle { get; set; }
    }
}