using ConnectDB.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using thuydung484.Model;

namespace thuydung484.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Vehicle_MaintenanceController : ControllerBase
    {
        private readonly AppDbContext _context;

        public Vehicle_MaintenanceController(AppDbContext context)
        {
            _context = context;
        }

        // GET ALL
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleMaintenance>>> GetAll()
        {
            return await _context.Vehicle_Maintenances
                .Include(v => v.Vehicle)
                .ToListAsync();
        }

        // GET BY ID
        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleMaintenance>> GetById(int id)
        {
            var item = await _context.Vehicle_Maintenances
                .Include(v => v.Vehicle)
                .FirstOrDefaultAsync(v => v.id == id);

            if (item == null)
                return NotFound();

            return item;
        }

        // CREATE
        // CREATE: Tự động chuyển xe sang trạng thái 'Maintenance'
        [HttpPost]
        public async Task<ActionResult<VehicleMaintenance>> Create(VehicleMaintenance item)
        {
            try
            {
                // 1. Tìm xe và cập nhật trạng thái xe sang Maintenance
                var vehicle = await _context.Vehicles.FindAsync(item.vehicle_id);
                if (vehicle != null)
                {
                    vehicle.status = "Maintenance";
                }

                // 2. Thiết lập dữ liệu mặc định cho bản ghi bảo trì
                item.created_at = DateTime.Now;
                if (string.IsNullOrEmpty(item.status)) item.status = "InProgress";

                _context.Vehicle_Maintenances.Add(item);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = item.id }, item);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // UPDATE: Nếu bảo trì 'Completed', tự động chuyển xe về 'Available'
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, VehicleMaintenance item)
        {
            if (id != item.id) return BadRequest();

            try
            {
                _context.Entry(item).State = EntityState.Modified;

                // Nếu trạng thái bảo trì chuyển sang hoàn thành, trả xe về trạng thái sẵn sàng
                if (item.status == "Completed")
                {
                    var vehicle = await _context.Vehicles.FindAsync(item.vehicle_id);
                    if (vehicle != null)
                    {
                        vehicle.status = "Available";
                    }
                }

                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Vehicle_Maintenances.FindAsync(id);

            if (item == null)
                return NotFound();

            _context.Vehicle_Maintenances.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}