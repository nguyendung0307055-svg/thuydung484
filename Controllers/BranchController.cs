using ConnectDB.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using thuydung484.Model;

namespace thuydung484.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BranchController(AppDbContext context)
        {
            _context = context;
        }

        // ================= GET ALL =================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Branch>>> GetBranches()
        {
            return await _context.Branches.ToListAsync();
        }

        // ================= GET BY ID =================
        [HttpGet("{id}")]
        public async Task<ActionResult<Branch>> GetBranch(int id)
        {
            var branch = await _context.Branches.FindAsync(id);

            if (branch == null)
                return NotFound("Không tìm thấy chi nhánh");

            return branch;
        }

        // ================= CREATE =================
        [HttpPost]
        public async Task<ActionResult> CreateBranch(Branch branch)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // validate phone
            if (!Regex.IsMatch(branch.phone ?? "", @"^0\d{9}$"))
                return BadRequest("SĐT không hợp lệ");

            // auto code
            if (string.IsNullOrEmpty(branch.code))
            {
                var count = await _context.Branches.CountAsync() + 1;
                branch.code = "BR" + count.ToString("D3");
            }

            branch.status ??= "Active";
            branch.created_at = DateTime.Now;

            _context.Branches.Add(branch);
            await _context.SaveChangesAsync();

            return Ok(branch);
        }

        // ================= UPDATE =================
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBranch(int id, Branch branch)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != branch.id)
                return BadRequest("ID không khớp");

            var existing = await _context.Branches.FindAsync(id);
            if (existing == null)
                return NotFound("Không tìm thấy");

            if (!Regex.IsMatch(branch.phone ?? "", @"^0\d{9}$"))
                return BadRequest("SĐT không hợp lệ");

            // ❌ KHÔNG update code
            existing.name = branch.name;
            existing.address = branch.address;
            existing.phone = branch.phone;
            existing.status = branch.status;
            existing.email = branch.email;
            existing.manager_name = branch.manager_name;
            existing.updated_at = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok("Cập nhật thành công");
        }

        // ================= DELETE =================
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            var branch = await _context.Branches.FindAsync(id);
            if (branch == null)
                return NotFound();

            var hasVehicle = await _context.Vehicles
                .AnyAsync(v => v.branch_id == id);

            if (hasVehicle)
                return BadRequest("Chi nhánh đang có xe");

            _context.Branches.Remove(branch);
            await _context.SaveChangesAsync();

            return Ok("Xóa thành công");
        }
    }
}