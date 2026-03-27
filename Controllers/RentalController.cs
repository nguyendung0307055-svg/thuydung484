using ConnectDB.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using thuydung484.Model;

namespace thuydung484.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RentalController(AppDbContext context)
        {
            _context = context;
        }

        // =========================================
        // GET: api/rental
        // Lấy tất cả hợp đồng
        // =========================================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rental>>> GetRentals()
        {
            var rentals = await _context.Rentals
                .Include(r => r.Customer)
                .Include(r => r.Vehicle)
                .ToListAsync();

            return Ok(rentals);
        }

        // =========================================
        // GET: api/rental/5
        // Lấy 1 hợp đồng
        // =========================================
        [HttpGet("{id}")]
        public async Task<ActionResult<Rental>> GetRental(int id)
        {
            var rental = await _context.Rentals
                .Include(r => r.Customer)
                .Include(r => r.Vehicle)
                .FirstOrDefaultAsync(r => r.id == id);

            if (rental == null)
            {
                return NotFound("Không tìm thấy hợp đồng");
            }

            return Ok(rental);
        }

        // =========================================
        // POST: api/rental
        // Tạo hợp đồng thuê xe
        // =========================================
        [HttpPost]
        public async Task<ActionResult> CreateRental(Rental rental)
        {
            // Kiểm tra xe tồn tại
            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(v => v.Id == rental.vehicle_id);

            if (vehicle == null)
            {
                return BadRequest("Xe không tồn tại");
            }

            // Kiểm tra trạng thái xe
            if (vehicle.status != "Available")
            {
                return BadRequest("Xe không khả dụng");
            }

            // Kiểm tra đã có rental active chưa
            var activeRental = await _context.Rentals
                .FirstOrDefaultAsync(r =>
                    r.vehicle_id == rental.vehicle_id &&
                    r.status == "Active");

            if (activeRental != null)
            {
                return BadRequest("Xe đang được thuê");
            }

            // Set trạng thái hợp đồng
            rental.status = "Active";

            // Cập nhật trạng thái xe
            vehicle.status = "Rented";

            _context.Rentals.Add(rental);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Tạo hợp đồng thành công",
                rentalId = rental.id
            });
        }

        // =========================================
        // PUT: api/rental/return/5
        // Trả xe
        // =========================================
        [HttpPut("return/{id}")]
        public async Task<ActionResult> ReturnVehicle(int id)
        {
            var rental = await _context.Rentals
                .Include(r => r.Vehicle)
                .FirstOrDefaultAsync(r => r.id == id);

            if (rental == null)
            {
                return NotFound("Không tìm thấy hợp đồng");
            }

            if (rental.status != "Active")
            {
                return BadRequest("Hợp đồng đã hoàn thành");
            }

            // Set thời gian trả
            rental.actual_end_time = DateTime.Now;

            var vehicle = rental.Vehicle;

            // Tính số giờ thuê
            double totalHours =
                (rental.actual_end_time.Value -
                 rental.start_time).TotalHours;

            decimal totalAmount = 0;

            if (rental.rent_type == "HOUR")
            {
                totalAmount =
                    (decimal)totalHours *
                    vehicle.price_per_hour;
            }
            else if (rental.rent_type == "DAY")
            {
                double totalDays =
                    Math.Ceiling(totalHours / 24);

                totalAmount =
                    (decimal)totalDays *
                    vehicle.price_per_day;
            }

            rental.total_amount = totalAmount;

            // Update trạng thái
            rental.status = "Completed";

            vehicle.status = "Available";

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Trả xe thành công",
                totalAmount = totalAmount
            });
        }

        // =========================================
        // DELETE: api/rental/5
        // Hủy hợp đồng
        // =========================================
        [HttpDelete("{id}")]
        public async Task<ActionResult> CancelRental(int id)
        {
            var rental = await _context.Rentals
                .Include(r => r.Vehicle)
                .FirstOrDefaultAsync(r => r.id == id);

            if (rental == null)
            {
                return NotFound("Không tìm thấy hợp đồng");
            }

            if (rental.status == "Completed")
            {
                return BadRequest("Không thể hủy hợp đồng đã hoàn thành");
            }

            rental.status = "Cancelled";

            rental.Vehicle.status = "Available";

            await _context.SaveChangesAsync();

            return Ok("Hủy hợp đồng thành công");
        }
    }
}