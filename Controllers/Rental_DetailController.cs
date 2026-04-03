using ConnectDB.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using thuydung484.Model;

namespace thuydung484.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Rental_DetailController : ControllerBase
    {
        private readonly AppDbContext _context;

        public Rental_DetailController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rental_Detail>>> GetAll()
        {
            return await _context.Rental_Details
                .Include(r => r.Rental)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Rental_Detail>> GetById(int id)
        {
            var item = await _context.Rental_Details
                .Include(r => r.Rental)
                .FirstOrDefaultAsync(r => r.id == id);

            if (item == null)
                return NotFound();

            return item;
        }

        [HttpPost]
        public async Task<ActionResult<Rental_Detail>> Create(Rental_Detail item)
        {
            try
            {
                _context.Rental_Details.Add(item);
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
        public async Task<IActionResult> Update(int id, Rental_Detail item)
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
            var item = await _context.Rental_Details.FindAsync(id);

            if (item == null)
                return NotFound();

            _context.Rental_Details.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}