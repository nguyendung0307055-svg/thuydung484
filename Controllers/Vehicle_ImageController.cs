using ConnectDB.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using thuydung484.Model;

namespace thuydung484.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Vehicle_ImageController : ControllerBase
    {
        private readonly AppDbContext _context;

        public Vehicle_ImageController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vehicle_Image>>> GetAll()
        {
            return await _context.Vehicle_Images.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Vehicle_Image>> GetById(int id)
        {
            var item = await _context.Vehicle_Images.FindAsync(id);

            if (item == null)
                return NotFound();

            return item;
        }

        [HttpPost]
        public async Task<ActionResult<Vehicle_Image>> Create(Vehicle_Image item)
        {
            try
            {
                _context.Vehicle_Images.Add(item);
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
        public async Task<IActionResult> Update(int id, Vehicle_Image item)
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
            var item = await _context.Vehicle_Images.FindAsync(id);

            if (item == null)
                return NotFound();

            _context.Vehicle_Images.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}