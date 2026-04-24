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
        [RegularExpression(@"^[0-9]{9,11}$", ErrorMessage = "SĐT phải là số (9-11 chữ số)")]
        public string phone { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{9,12}$", ErrorMessage = "CCCD phải là số")]
        public string cccd { get; set; }

        [Required]
        [StringLength(255)]
        public string address { get; set; }  // Địa chỉ

        [Required]
        [StringLength(20)]
        public string license_number { get; set; }  // Số GPLX

        [Required]
        [RegularExpression("^(A1|A)$", ErrorMessage = "Bằng lái chỉ được là A1 hoặc A")]
        public string license_type { get; set; }
        public DateTime? license_expiry { get; set; } // ngày hết hạn    }

    }
}