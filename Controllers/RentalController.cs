using ConnectDB.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using thuydung484.DTOs;
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
        // GET ALL RENTALS
        // =========================================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rental>>> GetRentals()
        {
            var rentals = await _context.Rentals
                .Include(r => r.Customer)
                .Include(r => r.Vehicle)
                .Include(r => r.Rental_Detail)
                .Include(r => r.Penalties)
                .ToListAsync();
            return Ok(rentals);
        }

        // =========================================
        // CREATE RENTAL (Lập hợp đồng)
        // =========================================
        [HttpPost]
        public async Task<ActionResult> CreateRental(Rental rental)
        {
            var vehicle = await _context.Vehicles.FindAsync(rental.vehicle_id);
            if (vehicle == null || vehicle.status != "Available")
                return BadRequest("Xe không sẵn sàng");

            rental.status = "Active";
            rental.start_time = DateTime.Now;
            vehicle.status = "Rented";

            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Tạo hợp đồng thành công", id = rental.id });
        }
        [HttpPost("import")]
        public async Task<IActionResult> ImportRentals(List<RentalImportDto> data) // Bỏ [FromBody] vẫn chạy vì có [ApiController]
        {
            if (data == null || !data.Any())
                return BadRequest("Dữ liệu danh sách trống");

            // Xóa validate object để tránh lỗi 400 "oan"
            ModelState.Remove("Customer");
            ModelState.Remove("Vehicle");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            foreach (var item in data)
            {
                // 🔍 Tìm customer bằng tên (Trim để tránh lỗi khoảng trắng trong Excel)
                var customer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.name.Trim().ToLower() == item.customerName.Trim().ToLower());

                if (customer == null)
                    return BadRequest($"Không có khách: {item.customerName}");

                // 🔍 Tìm vehicle bằng tên
                var vehicle = await _context.Vehicles
                    .FirstOrDefaultAsync(v => v.name.Trim().ToLower() == item.vehicleName.Trim().ToLower());

                if (vehicle == null)
                    return BadRequest($"Không có xe: {item.vehicleName}");

                var rental = new Rental
                {
                    customer_id = customer.id,
                    vehicle_id = vehicle.Id,
                    rent_type = "DAY",
                    start_time = item.start_time ?? DateTime.Now,
                    expected_end_time = item.end_time ?? DateTime.Now.AddDays(1),
                    actual_end_time = item.end_time,
                    total_amount = item.total_amount ?? 0,
                    status = "Completed"
                };

                _context.Rentals.Add(rental);
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Import thành công", count = data.Count });
        }
        // PUT: TRẢ XE & QUYẾT TOÁN CHI TIẾT
        // =========================================
        [HttpPut("return/{id}")]
        public async Task<ActionResult> ReturnVehicle(int id, [FromBody] ReturnRequest request)
        {
            // 1. Lấy thông tin Hợp đồng kèm theo Xe và Chi tiết phí
            var rental = await _context.Rentals
                .Include(r => r.Vehicle)
                .Include(r => r.Rental_Detail)
                .FirstOrDefaultAsync(r => r.id == id);

            if (rental == null) return NotFound("Hợp đồng không tồn tại");
            if (rental.status != "Active") return BadRequest("Hợp đồng này đã kết thúc");

            var vehicle = rental.Vehicle;

            // 2. Tính toán thời gian thực tế
            rental.actual_end_time = request.actual_end_time;
            double totalHours = (rental.actual_end_time.Value - rental.start_time).TotalHours;
            if (totalHours < 0) totalHours = 0;

            // 3. Tính tiền thuê gốc (Base Amount)
            decimal baseAmount = 0;
            if (rental.rent_type == "HOUR")
            {
                // Tính chính xác theo giờ lẻ (ví dụ 1.5 giờ)
                baseAmount = (decimal)totalHours * vehicle.price_per_hour;
            }
            else // Theo DAY
            {
                baseAmount = (decimal)(totalHours / 24) * vehicle.price_per_day;
            }

            // 4. Lấy tiền phạt từ Frontend gửi về
            decimal penaltyAmount = request.penalty?.amount ?? 0;

            // 5. Cập nhật bảng Rental (Hợp đồng)
            rental.total_amount = baseAmount + penaltyAmount;
            rental.status = "Completed";

            // 6. Cập nhật bảng Vehicle (Xe)
            vehicle.status = "Available";

            // 7. Cập nhật bảng Rental_Detail (Chi tiết hóa đơn)
            if (rental.Rental_Detail != null)
            {
                rental.Rental_Detail.actual_amount = baseAmount;
                rental.Rental_Detail.penalty_amount = penaltyAmount;
            }

            // 8. Nếu có tiền phạt -> Tạo mới dòng Penalty
            if (penaltyAmount > 0)
            {
                var penaltyEntry = new Penalty
                {
                    rental_id = rental.id,
                    penalty_type = request.penalty.penalty_type ?? "Trễ hạn/Hư hỏng",
                    amount = penaltyAmount,
                    description = request.penalty.description ?? "Không có mô tả",
                    created_at = DateTime.Now
                };
                _context.Penalties.Add(penaltyEntry);
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Quyết toán thành công!",
                baseAmount = baseAmount,
                penaltyAmount = penaltyAmount,
                totalAmount = rental.total_amount
            });
        }
    }

    // =========================================
    // DTO CLASSES (Đặt ở cuối file theo Cách 2)
    // =========================================
    public class ReturnRequest
    {
        public DateTime actual_end_time { get; set; }
        public PenaltyRequest penalty { get; set; }
        public string payment_method { get; set; }
    }

    public class PenaltyRequest
    {
        public string penalty_type { get; set; }
        public decimal amount { get; set; }
        public string description { get; set; }
    }
}