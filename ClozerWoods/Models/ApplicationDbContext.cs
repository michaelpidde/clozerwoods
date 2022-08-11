using ClozerWoods.Models.Entities;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace ClozerWoods.Models;

public class ApplicationDbContext : DbContext {
    private readonly IConfiguration _config;

    public ApplicationDbContext(IConfiguration config) : base() {
        _config = config;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options) {
        var connectionBuilder = new MySqlConnectionStringBuilder();
        connectionBuilder.ConnectionString = _config.GetConnectionString("MySQL");
        connectionBuilder.UserID = _config["MySqlUser"];
        connectionBuilder.Password = _config["MySqlPassword"];
        var connectionString = connectionBuilder.ConnectionString;
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }

    public DbSet<User> ?Users { get; set; }
}
