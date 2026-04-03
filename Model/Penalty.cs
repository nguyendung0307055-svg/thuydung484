using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace thuydung484.Model
{
    [Table("Penalty")]
    public class Penalty
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
        public Rental Rental { get; set; }

        /*
        ==========================
        Thông tin phụ phí
        ==========================
        */

        // Loại phụ phí
        // Ví dụ: LateReturn, Damage, LostVehicle
        [Required]
        [StringLength(50)]
        public string penalty_type { get; set; }

        // Số tiền phạt
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal amount { get; set; }

        // Mô tả thêm
        [StringLength(255)]
        public string description { get; set; }

        // Ngày tạo
        [Required]
        public DateTime created_at { get; set; }
    }
}