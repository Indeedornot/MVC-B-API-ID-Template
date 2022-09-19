using APIServer.Logging.Models;
using Microsoft.EntityFrameworkCore;

namespace APIServer.Logging.Database;

public class LogDbContext : DbContext {
  private readonly IHostEnvironment Env;

  public LogDbContext(IHostEnvironment env) {
    Env = env;
  }

  public DbSet<Log> Logs { get; set; } = null!;

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
    string? databasePath = Env.ContentRootPath + @"\Logging\Database\LogDatabase.sqlite";
    optionsBuilder.UseSqlite($"Data Source={databasePath};");
  }
}