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
        [HttpPost]
        public async Task<ActionResult<VehicleMaintenance>> Create(VehicleMaintenance item)
        {
            try
            {
                _context.Vehicle_Maintenances.Add(item);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById),
                    new { id = item.id }, item);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // UPDATE
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, VehicleMaintenance item)
        {
            if (id != item.id)
                return BadRequest();

            _context.Entry(item).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
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