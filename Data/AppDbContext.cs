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
    public DbSet<Branch> Branches { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Vehicle_Image> Vehicle_Images { get; set; }
    public DbSet<VehicleMaintenance> Vehicle_Maintenances { get; set; }
    public DbSet<Vehicle_Status_Log> Vehicle_Status_Logs { get; set; }
    public DbSet<Rental_Detail> Rental_Details { get; set; }
    public DbSet<Payment_Log> Payment_Logs { get; set; }
    public DbSet<Penalty> Penalties { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 🔴 FIX Multiple Cascade Paths

        // Branch → Vehicle
        modelBuilder.Entity<Vehicle>()
            .HasOne(v => v.Branch)
            .WithMany(b => b.Vehicles)
            .HasForeignKey(v => v.branch_id)
            .OnDelete(DeleteBehavior.Restrict);

        // Branch → User
        modelBuilder.Entity<User>()
            .HasOne(u => u.Branch)
            .WithMany(b => b.Users)
            .HasForeignKey(u => u.branch_id)
            .OnDelete(DeleteBehavior.Restrict);

        // VehicleMaintenance → Vehicle
        modelBuilder.Entity<VehicleMaintenance>()
            .HasOne(vm => vm.Vehicle)
            .WithMany(v => v.VehicleMaintenances)
            .HasForeignKey(vm => vm.vehicle_id)
            .OnDelete(DeleteBehavior.Restrict);

        // VehicleMaintenance → User
        modelBuilder.Entity<VehicleMaintenance>()
            .HasOne(vm => vm.User)
            .WithMany(u => u.VehicleMaintenances)
            .HasForeignKey(vm => vm.user_id)
            .OnDelete(DeleteBehavior.Restrict);
    }
}