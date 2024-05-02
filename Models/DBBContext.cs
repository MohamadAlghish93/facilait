using Microsoft.EntityFrameworkCore;
using FacilaIT.Models.RABC;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace FacilaIT.Models;

public class DBBContext : IdentityDbContext<IdentityUser> //DbContext
{
    public DBBContext(DbContextOptions<DBBContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Ignore<IdentityUserLogin<string>>();
        // modelBuilder.Ignore<IdentityUserRole<string>>();
        modelBuilder.Ignore<IdentityUserClaim<string>>();
        modelBuilder.Ignore<IdentityUserToken<string>>();
        modelBuilder.Ignore<IdentityUser<string>>();
        modelBuilder.Ignore<User>();
        // modelBuilder.Entity<ParameterItem>()
        //     .HasNoKey();
        modelBuilder.Entity<IdentityUserRole<Guid>>().HasKey(p => new { p.UserId, p.RoleId });
        // modelBuilder.Entity<QueryItem>()
        //     .HasNoKey();
        // modelBuilder.Entity<QueryItem>()
        //     .HasMany(q => q.Parameters);
        // Configure the one-to - many relationship
    }


    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

    // public DbSet<SystemMonitor> SystemMonitor { get; set; } = null!;
}

// public class ApplicationDbContext : IdentityDbContext<IdentityUser>
// {
//     public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
//     {
//     }
//     protected override void OnModelCreating(ModelBuilder builder)
//     {
//         base.OnModelCreating(builder);
//     }
// }