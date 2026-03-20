using Microsoft.EntityFrameworkCore;
using thuydung484.Model;
namespace ConnectDB.Data;
public class AppDbContext : DbContext
{
 
public AppDbContext(DbContextOptions<AppDbContext> options) :
base(options)
    { }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Rental> Rentals { get; set; }

    public DbSet<Payment> Payments { get; set; }

}