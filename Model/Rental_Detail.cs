using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace thuydung484.Model
{
    [Table("Rental_Detail")]
    public class Rental_Detail
    {
        [Key]
        public int id { get; set; }

        /*
        ==========================
        FK → Rental
        ==========================
        */

        [Required]
        public int rental_id { get; set; }

        [ForeignKey("rental_id")]
        public Rental? Rental { get; set; }
        /*
        ==========================
        Giá thuê tại thời điểm tạo hợp đồng
        ==========================
        */

        // Giá theo giờ
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal price_per_hour { get; set; }

        // Giá theo ngày
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal price_per_day { get; set; }

        /*
        ==========================
        Thông tin tiền
        ==========================
        */

        // Tổng tiền dự kiến
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal estimated_amount { get; set; }

        // Tổng tiền thực tế
        [Column(TypeName = "decimal(10,2)")]
        public decimal? actual_amount { get; set; }

        // Tổng tiền phụ phí
        [Column(TypeName = "decimal(10,2)")]
        public decimal? penalty_amount { get; set; }

        /*
        ==========================
        Metadata
        ==========================
        */

        [Required]
        public DateTime created_at { get; set; }
    }
}