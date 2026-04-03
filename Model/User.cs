using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace thuydung484.Model
{
    [Table("User")]
    public class User
    {
        [Key]
        public int id { get; set; }

        // Họ tên
        [Required]
        [StringLength(100)]
        public string name { get; set; }

        // Username đăng nhập
        [Required]
        [StringLength(50)]
        public string username { get; set; }

        // Mật khẩu
        [Required]
        [StringLength(255)]
        public string password_hash { get; set; }

        // SĐT
        [StringLength(15)]
        public string phone { get; set; }

        // Email
        [StringLength(100)]
        public string email { get; set; }

        // Role
        [Required]
        [StringLength(20)]
        public string role { get; set; }

        // Status
        [Required]
        [StringLength(20)]
        public string status { get; set; }

        // FK → Branch
        [Required]
        public int branch_id { get; set; }

        [ForeignKey("branch_id")]
        public Branch? Branch { get; set; }

        // Ngày tạo
        [Required]
        public DateTime created_at { get; set; }

        public DateTime? updated_at { get; set; }

        /*
        Navigation Properties
        */
        public ICollection<Rental>? Rentals { get; set; }

        public ICollection<VehicleMaintenance>? VehicleMaintenances { get; set; }
    }
}