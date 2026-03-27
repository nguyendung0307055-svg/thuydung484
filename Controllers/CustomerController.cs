using ConnectDB.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        // =========================================
        // GET: api/customer
        // Lấy tất cả khách hàng
        // =========================================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            return await _context.Customers.ToListAsync();
        }

        // =========================================
        // GET: api/customer/5
        // Lấy khách hàng theo ID
        // =========================================
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.id == id);

            if (customer == null)
            {
                return NotFound("Không tìm thấy khách hàng");
            }

            return Ok(customer);
        }

        // =========================================
        // POST: api/customer
        // Thêm khách hàng
        // =========================================
        [HttpPost]
        public async Task<ActionResult> CreateCustomer(Customer customer)
        {
            // Kiểm tra trùng CCCD
            var exists = await _context.Customers
                .FirstOrDefaultAsync(c =>
                    c.cccd == customer.cccd);

            if (exists != null)
            {
                return BadRequest("CCCD đã tồn tại");
            }

            _context.Customers.Add(customer);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Thêm khách hàng thành công",
                customerId = customer.id
            });
        }

        // =========================================
        // PUT: api/customer/5
        // Cập nhật khách hàng
        // =========================================
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCustomer(int id, Customer customer)
        {
            if (id != customer.id)
            {
                return BadRequest("Id không khớp");
            }

            var existingCustomer =
                await _context.Customers.FindAsync(id);

            if (existingCustomer == null)
            {
                return NotFound("Không tìm thấy khách hàng");
            }

            // Update dữ liệu
            existingCustomer.name = customer.name;
            existingCustomer.phone = customer.phone;
            existingCustomer.cccd = customer.cccd;
            existingCustomer.address = customer.address;

            await _context.SaveChangesAsync();

            return Ok("Cập nhật khách hàng thành công");
        }

        // =========================================
        // DELETE: api/customer/5
        // Xóa khách hàng
        // Không cho xóa nếu đang thuê xe
        // =========================================
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.id == id);

            if (customer == null)
            {
                return NotFound("Không tìm thấy khách hàng");
            }

            // Kiểm tra có rental active không
            var activeRental =
                await _context.Rentals
                .FirstOrDefaultAsync(r =>
                    r.customer_id == id &&
                    r.status == "Active");

            if (activeRental != null)
            {
                return BadRequest("Khách hàng đang thuê xe, không thể xóa");
            }

            _context.Customers.Remove(customer);

            await _context.SaveChangesAsync();

            return Ok("Xóa khách hàng thành công");
        }
    }
}