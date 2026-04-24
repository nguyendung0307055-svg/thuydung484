
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace thuydung484.Model
{
    [Table("VehicleTypes")]
    public class VehicleType
    {
        [Key]
        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string name { get; set; } // Xe số, Xe ga, Xe điện

        [StringLength(255)]
        public string? description { get; set; }

        [Required]
        public bool is_active { get; set; } = true;

        // Navigation
        public ICollection<Vehicle>? Vehicles { get; set; }
    }
}
