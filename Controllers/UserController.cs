using ConnectDB.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using thuydung484.Model;

namespace thuydung484.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        private string GetRole()
        {
            return Request.Headers["role"].ToString();
        }

        // ================= GET ALL =================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // ================= GET BY ID =================
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            return user;
        }

        // ================= CREATE =================
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            if (GetRole() != "Admin")
                return Forbid("Chỉ Admin được tạo user");

            user.created_at = DateTime.Now;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        // ================= UPDATE =================
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            if (GetRole() != "Admin")
                return Forbid("Chỉ Admin được sửa");

            if (id != user.id) return BadRequest();

            user.updated_at = DateTime.Now;

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ================= DELETE =================
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (GetRole() != "Admin")
                return Forbid("Chỉ Admin được xóa");

            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ================= LOGIN =================
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _context.Users
                .Include(u => u.Branch)
                .FirstOrDefaultAsync(u =>
                    u.username == request.username &&
                    u.password_hash == request.password);

            if (user == null)
                return Unauthorized("Sai tài khoản");

            if (user.status != "Active")
                return BadRequest("Tài khoản bị khóa");

            return Ok(new
            {
                id = user.id,
                name = user.name,
                username = user.username,
                role = user.role,
                branch_id = user.branch_id,
                branch_name = user.Branch?.name
            });
        }

        public class LoginRequest
        {
            public string username { get; set; }
            public string password { get; set; }
        }
    }
}