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

        // ================= GET ALL =================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vehicle>>> GetVehicles()
        {
            var vehicles = await _context.Vehicles
                .Include(v => v.Branch)
                .Include(v => v.VehicleType)
                .Include(v => v.Vehicle_Images) // 🔥 thêm
                .ToListAsync();

            return Ok(vehicles);
        }

        // ================= GET BY ID =================
        [HttpGet("{id}")]
        public async Task<ActionResult<Vehicle>> GetVehicle(int id)
        {
            var vehicle = await _context.Vehicles
                .Include(v => v.Branch)
                .Include(v => v.VehicleType)
                .Include(v => v.Vehicle_Images) // 🔥 thêm
                .FirstOrDefaultAsync(v => v.Id == id);

            if (vehicle == null)
                return NotFound("Không tìm thấy xe");

            return Ok(vehicle);
        }

        // ================= GET AVAILABLE =================
        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<Vehicle>>> GetAvailableVehicles()
        {
            var vehicles = await _context.Vehicles
                .Include(v => v.Branch)
                .Include(v => v.VehicleType)
                .Where(v => v.status == "Available")
                .ToListAsync();

            return Ok(vehicles);
        }

        // ================= CREATE =================
        [HttpPost]
        public async Task<ActionResult> CreateVehicle(Vehicle vehicle)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            vehicle.license_plate = vehicle.license_plate?.Trim().ToUpper();
            vehicle.status = vehicle.status?.Trim();

            // validate engine
            if (vehicle.engine_capacity <= 0)
                return BadRequest("Dung tích xe phải > 0");

            // validate price
            if (vehicle.price_per_hour < 0 || vehicle.price_per_day < 0)
                return BadRequest("Giá không hợp lệ");

            // check trùng biển số
            var exists = await _context.Vehicles
                .AnyAsync(v => v.license_plate == vehicle.license_plate);

            if (exists)
                return BadRequest("Biển số đã tồn tại");

            // check branch
            var branchExists = await _context.Branches
                .AnyAsync(b => b.id == vehicle.branch_id);

            if (!branchExists)
                return BadRequest("Chi nhánh không tồn tại");

            // 🔥 check VehicleType
            var typeExists = await _context.VehicleTypes
                .AnyAsync(t => t.id == vehicle.vehicle_type_id);

            if (!typeExists)
                return BadRequest("Loại xe không tồn tại");

            if (string.IsNullOrEmpty(vehicle.status))
                vehicle.status = "Available";

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Thêm xe thành công",
                vehicleId = vehicle.Id
            });
        }

        // ================= UPDATE =================
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateVehicle(int id, Vehicle vehicle)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != vehicle.Id)
                return BadRequest("Id không khớp");

            var existing = await _context.Vehicles.FindAsync(id);
            if (existing == null)
                return NotFound("Không tìm thấy xe");

            vehicle.license_plate = vehicle.license_plate?.Trim().ToUpper();

            // check trùng biển số
            var duplicate = await _context.Vehicles
                .AnyAsync(v => v.license_plate == vehicle.license_plate && v.Id != id);

            if (duplicate)
                return BadRequest("Biển số đã tồn tại");

            // check branch
            var branchExists = await _context.Branches
                .AnyAsync(b => b.id == vehicle.branch_id);

            if (!branchExists)
                return BadRequest("Chi nhánh không tồn tại");

            // 🔥 check loại xe
            var typeExists = await _context.VehicleTypes
                .AnyAsync(t => t.id == vehicle.vehicle_type_id);

            if (!typeExists)
                return BadRequest("Loại xe không tồn tại");

            // validate engine
            if (vehicle.engine_capacity <= 0)
                return BadRequest("Dung tích không hợp lệ");

            // update
            existing.name = vehicle.name;
            existing.license_plate = vehicle.license_plate;
            existing.engine_capacity = vehicle.engine_capacity;
            existing.price_per_hour = vehicle.price_per_hour;
            existing.price_per_day = vehicle.price_per_day;
            existing.status = vehicle.status;
            existing.branch_id = vehicle.branch_id;
            existing.vehicle_type_id = vehicle.vehicle_type_id; // 🔥 thêm

            await _context.SaveChangesAsync();

            return Ok("Cập nhật xe thành công");
        }

        // ================= DELETE =================
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVehicle(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);

            if (vehicle == null)
                return NotFound("Không tìm thấy xe");

            var activeRental = await _context.Rentals
                .FirstOrDefaultAsync(r =>
                    r.vehicle_id == id &&
                    r.status == "Active");

            if (activeRental != null)
                return BadRequest("Xe đang được thuê, không thể xóa");

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();

            return Ok("Xóa xe thành công");
        }
    }
}