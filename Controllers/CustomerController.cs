using ConnectDB.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using thuydung484.Model;

namespace thuydung484.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CustomerController(AppDbContext context)
        {
            _context = context;
        }

        // ================= GET ALL =================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            return await _context.Customers.ToListAsync();
        }

        // ================= GET BY ID =================
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.id == id);

            if (customer == null)
                return NotFound("Không tìm thấy khách hàng");

            return Ok(customer);
        }

        // ================= CREATE =================
        [HttpPost]
        public async Task<IActionResult> CreateCustomer(Customer customer)
        {
            // 🔥 validate model
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // 🔥 normalize (trim)
            customer.phone = customer.phone?.Trim();
            customer.cccd = customer.cccd?.Trim();
            customer.license_type = customer.license_type?.Trim();

            // 🔥 validate số
            if (!Regex.IsMatch(customer.phone, @"^[0-9]{9,11}$"))
                return BadRequest("SĐT phải là số (9-11 chữ số)");

            if (!Regex.IsMatch(customer.cccd, @"^[0-9]{9,12}$"))
                return BadRequest("CCCD phải là số");

            // 🔥 validate bằng lái
            if (customer.license_type != "A1" && customer.license_type != "A")
                return BadRequest("Bằng lái chỉ được là A1 hoặc A");

            // 🔥 check trùng CCCD
            var exists = await _context.Customers
                .AnyAsync(c => c.cccd == customer.cccd);

            if (exists)
                return BadRequest("CCCD đã tồn tại");

            // 🔥 check GPLX hết hạn
            if (customer.license_expiry != null &&
                customer.license_expiry < DateTime.Now)
            {
                return BadRequest("GPLX đã hết hạn");
            }

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Thêm khách hàng thành công",
                id = customer.id
            });
        }

        // ================= UPDATE =================
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, Customer customer)
        {
            // 🔥 validate model
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != customer.id)
                return BadRequest("Id không khớp");

            // 🔥 normalize
            customer.phone = customer.phone?.Trim();
            customer.cccd = customer.cccd?.Trim();
            customer.license_type = customer.license_type?.Trim();

            var existing = await _context.Customers.FindAsync(id);
            if (existing == null)
                return NotFound("Không tìm thấy khách hàng");

            // 🔥 validate số
            if (!Regex.IsMatch(customer.phone, @"^[0-9]{9,11}$"))
                return BadRequest("SĐT không hợp lệ");

            if (!Regex.IsMatch(customer.cccd, @"^[0-9]{9,12}$"))
                return BadRequest("CCCD không hợp lệ");

            // 🔥 validate bằng lái
            if (customer.license_type != "A1" && customer.license_type != "A")
                return BadRequest("Bằng lái chỉ được là A1 hoặc A");

            // 🔥 check trùng CCCD
            var duplicate = await _context.Customers
                .AnyAsync(c => c.cccd == customer.cccd && c.id != id);

            if (duplicate)
                return BadRequest("CCCD đã tồn tại");

            // 🔥 check GPLX
            if (customer.license_expiry != null &&
                customer.license_expiry < DateTime.Now)
            {
                return BadRequest("GPLX đã hết hạn");
            }

            // 🔥 update
            existing.name = customer.name;
            existing.phone = customer.phone;
            existing.cccd = customer.cccd;
            existing.address = customer.address;
            existing.license_number = customer.license_number;
            existing.license_type = customer.license_type;
            existing.license_expiry = customer.license_expiry;

            await _context.SaveChangesAsync();

            return Ok("Cập nhật khách hàng thành công");
        }

        // ================= DELETE =================
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.id == id);

            if (customer == null)
                return NotFound("Không tìm thấy khách hàng");

            // 🔥 check đang thuê xe
            var activeRental = await _context.Rentals
                .AnyAsync(r => r.customer_id == id && r.status == "Active");

            if (activeRental)
                return BadRequest("Khách hàng đang thuê xe, không thể xóa");

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return Ok("Xóa khách hàng thành công");
        }
    }
}