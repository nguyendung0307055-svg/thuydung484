using ConnectDB.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using thuydung484.Model;

namespace thuydung484.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Payment_LogController : ControllerBase
    {
        private readonly AppDbContext _context;

        public Payment_LogController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment_Log>>> GetAll()
        {
            return await _context.Payment_Logs
                .Include(p => p.Payment)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Payment_Log>> GetById(int id)
        {
            var item = await _context.Payment_Logs
                .Include(p => p.Payment)
                .FirstOrDefaultAsync(p => p.id == id);

            if (item == null)
                return NotFound();

            return item;
        }

        [HttpPost]
        public async Task<ActionResult<Payment_Log>> Create(Payment_Log item)
        {
            try
            {
                _context.Payment_Logs.Add(item);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById),
                    new { id = item.id }, item);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Payment_Log item)
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
            var item = await _context.Payment_Logs.FindAsync(id);

            if (item == null)
                return NotFound();

            _context.Payment_Logs.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}