using ClozerWoods.Models.Entities;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace ClozerWoods.Models;

public class ApplicationDbContext : DbContext {
    private readonly IConfiguration _config;

    public DbSet<User>? Users { get; set; }
    public DbSet<Page>? Pages { get; set; }
    public DbSet<Gallery>? Galleries { get; set; }
    public DbSet<MediaItem>? MediaItems { get; set; }

    public ApplicationDbContext(IConfiguration config) : base() {
        _config = config;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options) {
        var connectionBuilder = new MySqlConnectionStringBuilder();
        connectionBuilder.UserID = _config["MySqlUser"];
        connectionBuilder.Password = _config["MySqlPassword"];
        connectionBuilder.Server = _config["MySqlServer"];
        connectionBuilder.Database = _config["MySqlDatabase"];
        var connectionString = connectionBuilder.ConnectionString;
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Gallery>()
            .HasMany(g => g.MediaItems)
            .WithOne(m => m.Gallery);

        modelBuilder.Entity<Gallery>()
            .Navigation(g => g.MediaItems)
            .UsePropertyAccessMode(PropertyAccessMode.Property);

        modelBuilder.Entity<Page>()
            .HasMany(p => p.Children)
            .WithOne(p => p.Parent)
            .HasForeignKey(p => p.ParentId);
    }
}
