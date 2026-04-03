using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConnectDB.Data;
using thuydung484.Model;

namespace thuydung484.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PenaltyController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PenaltyController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Penalty>>> GetAll()
        {
            return await _context.Penalties
                .Include(p => p.Rental)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Penalty>> GetById(int id)
        {
            var item = await _context.Penalties
                .Include(p => p.Rental)
                .FirstOrDefaultAsync(p => p.id == id);

            if (item == null)
                return NotFound();

            return item;
        }

        [HttpPost]
        public async Task<ActionResult<Penalty>> Create(Penalty item)
        {
            _context.Penalties.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById),
                new { id = item.id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Penalty item)
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
            var item = await _context.Penalties.FindAsync(id);

            if (item == null)
                return NotFound();

            _context.Penalties.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}