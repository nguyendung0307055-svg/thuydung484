using System;

namespace thuydung484.DTOs
{
    public class RentalImportDto
    {
        public string customerName { get; set; }
        public string vehicleName { get; set; }
        public DateTime? start_time { get; set; }
        public DateTime? end_time { get; set; }
        public decimal? total_amount { get; set; }
    }
}