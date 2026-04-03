using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace thuydung484.Model
{
    public class Payment_Log
    {
        [Key]
        public int id { get; set; }  // Primary Key

        [Required]
        public int payment_id { get; set; }  // FK -> Payment

        [Required]
        [StringLength(20)]
        public string status { get; set; }
        // Pending / Success / Failed

        [StringLength(255)]
        public string note { get; set; }
        // Ghi chú lỗi (nếu có)

        [Required]
        public DateTime created_at { get; set; }
        // Thời điểm ghi log

        // Navigation property
        [ForeignKey("payment_id")]
        public Payment Payment { get; set; }
    }
}