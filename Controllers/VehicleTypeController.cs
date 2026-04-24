using ConnectDB.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using thuydung484.Model;

namespace thuydung484.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleTypeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VehicleTypeController(AppDbContext context)
        {
            _context = context;
        }

        // GET ALL
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleType>>> GetAll()
        {
            return await _context.VehicleTypes.ToListAsync();
        }

        // GET BY ID
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var data = await _context.VehicleTypes.FindAsync(id);

            if (data == null)
                return NotFound("Không tìm thấy loại xe");

            return Ok(data);
        }

        // CREATE
        [HttpPost]
        public async Task<IActionResult> Create(VehicleType model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var exists = await _context.VehicleTypes
                .AnyAsync(x => x.name == model.name);

            if (exists)
                return BadRequest("Loại xe đã tồn tại");

            _context.VehicleTypes.Add(model);
            await _context.SaveChangesAsync();

            return Ok("Thêm loại xe thành công");
        }

        // UPDATE
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, VehicleType model)
        {
            if (id != model.id)
                return BadRequest("Id không khớp");

            var data = await _context.VehicleTypes.FindAsync(id);

            if (data == null)
                return NotFound("Không tìm thấy");

            data.name = model.name;
            data.description = model.description;
            data.is_active = model.is_active;

            await _context.SaveChangesAsync();

            return Ok("Cập nhật thành công");
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _context.VehicleTypes.FindAsync(id);

            if (data == null)
                return NotFound();

            _context.VehicleTypes.Remove(data);
            await _context.SaveChangesAsync();

            return Ok("Xóa thành công");
        }
    }
}