using ClozerWoods.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClozerWoods.Models;

public class ApplicationDbContext : DbContext {
    protected override void OnConfiguring(DbContextOptionsBuilder options) {
        var connectionString = "server=localhost;database=clozerwoods;user=root;password=password";
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }

    public DbSet<User> ?Users { get; set; }
}
