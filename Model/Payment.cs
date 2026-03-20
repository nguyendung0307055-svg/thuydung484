using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace thuydung484.Model
{
    public class Payment
    {
        [Key]
        public int id { get; set; }  // Primary Key

        [Required]
        public int rental_id { get; set; }  // FK -> Rental

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal amount { get; set; }  // Số tiền thanh toán

        [Required]
        [StringLength(50)]
        public string method { get; set; }  // Cash / Transfer

        [Required]
        [StringLength(20)]
        public string status { get; set; }  // Pending / Paid

        // Navigation property
        [ForeignKey("rental_id")]
        public Rental Rental { get; set; }
    }
}