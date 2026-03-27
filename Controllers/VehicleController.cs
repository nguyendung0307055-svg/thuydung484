using ConnectDB.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using thuydung484.Model;

namespace thuydung484.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VehicleController(AppDbContext context)
        {
            _context = context;
        }

        // =========================================
        // GET: api/vehicle
        // Lấy tất cả xe
        // =========================================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vehicle>>> GetVehicles()
        {
            return await _context.Vehicles.ToListAsync();
        }

        // =========================================
        // GET: api/vehicle/5
        // Lấy 1 xe theo ID
        // =========================================
        [HttpGet("{id}")]
        public async Task<ActionResult<Vehicle>> GetVehicle(int id)
        {
            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(v => v.Id == id);

            if (vehicle == null)
            {
                return NotFound("Không tìm thấy xe");
            }

            return Ok(vehicle);
        }

        // =========================================
        // GET: api/vehicle/available
        // Lấy danh sách xe Available
        // =========================================
        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<Vehicle>>> GetAvailableVehicles()
        {
            var vehicles = await _context.Vehicles
                .Where(v => v.status == "Available")
                .ToListAsync();

            return Ok(vehicles);
        }

        // =========================================
        // POST: api/vehicle
        // Thêm xe mới
        // =========================================
        [HttpPost]
        public async Task<ActionResult> CreateVehicle(Vehicle vehicle)
        {
            // Kiểm tra trùng biển số
            var exists = await _context.Vehicles
                .FirstOrDefaultAsync(v =>
                    v.license_plate == vehicle.license_plate);

            if (exists != null)
            {
                return BadRequest("Biển số đã tồn tại");
            }

            // Default status
            if (string.IsNullOrEmpty(vehicle.status))
            {
                vehicle.status = "Available";
            }

            _context.Vehicles.Add(vehicle);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Thêm xe thành công",
                vehicleId = vehicle.Id
            });
        }

        // =========================================
        // PUT: api/vehicle/5
        // Cập nhật xe
        // =========================================
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateVehicle(int id, Vehicle vehicle)
        {
            if (id != vehicle.Id)
            {
                return BadRequest("Id không khớp");
            }

            var existingVehicle =
                await _context.Vehicles.FindAsync(id);

            if (existingVehicle == null)
            {
                return NotFound("Không tìm thấy xe");
            }

            // Update dữ liệu
            existingVehicle.license_plate =
                vehicle.license_plate;

            existingVehicle.type =
                vehicle.type;

            existingVehicle.price_per_hour =
                vehicle.price_per_hour;

            existingVehicle.price_per_day =
                vehicle.price_per_day;

            existingVehicle.status =
                vehicle.status;

            await _context.SaveChangesAsync();

            return Ok("Cập nhật xe thành công");
        }

        // =========================================
        // DELETE: api/vehicle/5
        // Xóa xe
        // Không cho xóa nếu đang được thuê
        // =========================================
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVehicle(int id)
        {
            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(v => v.Id == id);

            if (vehicle == null)
            {
                return NotFound("Không tìm thấy xe");
            }

            // Kiểm tra xe có rental active không
            var activeRental =
                await _context.Rentals
                .FirstOrDefaultAsync(r =>
                    r.vehicle_id == id &&
                    r.status == "Active");

            if (activeRental != null)
            {
                return BadRequest("Xe đang được thuê, không thể xóa");
            }

            _context.Vehicles.Remove(vehicle);

            await _context.SaveChangesAsync();

            return Ok("Xóa xe thành công");
        }
    }
}