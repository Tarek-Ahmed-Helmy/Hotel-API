using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public sealed class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<IdentityUserRole<string>>()
               .HasOne<ApplicationRole>()
               .WithMany()
               .HasForeignKey(ur => ur.RoleId)
               .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
    
    public DbSet<Company> Companies { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<RequestDetails> RequestDetails { get; set; }
    public DbSet<RequestHeader> RequestHeaders { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<RoomType> RoomTypes { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Status> Statuses { get; set; }
    public DbSet<UseType> UseTypes { get; set; }

}
