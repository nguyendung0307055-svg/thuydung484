using System.ComponentModel.DataAnnotations;

namespace thuydung484.Model
{
    public class Customer
    {
        [Key]
        public int id { get; set; }  // Primary Key

        [Required]
        [StringLength(100)]
        public string name { get; set; }  // Họ tên

        [Required]
        [StringLength(15)]
        public string phone { get; set; }  // SĐT

        [Required]
        [StringLength(20)]
        public string cccd { get; set; }  // CCCD/CMND

        [Required]
        [StringLength(255)]
        public string address { get; set; }  // Địa chỉ
    }
}