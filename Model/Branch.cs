using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace thuydung484.Model
{
    [Table("Branch")]
    public class Branch
    {
        [Key]
        public int id { get; set; }

        // Mã chi nhánh (BR001)
        [Required]
        [StringLength(20)]
        public string code { get; set; }

        // Tên chi nhánh
        [Required]
        [StringLength(100)]
        public string name { get; set; }

        // Địa chỉ
        [Required]
        [StringLength(255)]
        public string address { get; set; }

        // Số điện thoại
        [Required]
        [StringLength(15)]
        public string phone { get; set; }

        // Email
        [StringLength(100)]
        public string email { get; set; }

        // Tên quản lý
        [StringLength(100)]
        public string manager_name { get; set; }

        // Active / Inactive
        [Required]
        [StringLength(20)]
        public string status { get; set; }

        // Ngày tạo
        [Required]
        public DateTime created_at { get; set; }

        // Ngày cập nhật
        public DateTime? updated_at { get; set; }

        /*
         ==========================
         Navigation Properties
         ==========================
        */

        // Một chi nhánh có nhiều xe
        public ICollection<Vehicle>? Vehicles { get; set; }

        // Một chi nhánh có nhiều nhân viên
        public ICollection<User>? Users { get; set; }

        // Một chi nhánh có nhiều hợp đồng
        public ICollection<Rental>? Rentals { get; set; }
    }
}