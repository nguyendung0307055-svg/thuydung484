using ConnectDB.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using thuydung484.Model;

namespace thuydung484.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Vehicle_Status_LogController : ControllerBase
    {
        private readonly AppDbContext _context;

        public Vehicle_Status_LogController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vehicle_Status_Log>>> GetAll()
        {
            return await _context.Vehicle_Status_Logs
                .Include(v => v.Vehicle)
                .Include(v => v.User)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Vehicle_Status_Log>> GetById(int id)
        {
            var item = await _context.Vehicle_Status_Logs
                .Include(v => v.Vehicle)
                .Include(v => v.User)
                .FirstOrDefaultAsync(v => v.id == id);

            if (item == null)
                return NotFound();

            return item;
        }

        [HttpPost]
        public async Task<ActionResult<Vehicle_Status_Log>> Create(Vehicle_Status_Log item)
        {
            _context.Vehicle_Status_Logs.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById),
                new { id = item.id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Vehicle_Status_Log item)
        {
            if (id != item.id)
                return BadRequest();

            _context.Entry(item).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Vehicle_Status_Logs.FindAsync(id);

            if (item == null)
                return NotFound();

            _context.Vehicle_Status_Logs.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}