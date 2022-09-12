using System.Data.Common;
using APIServer.Database;
using APIServer.Logging.Model;
using Microsoft.EntityFrameworkCore;

namespace APIServer.Logging.Database;

public class LogDbContext : DbContext {
  private readonly IHostEnvironment Env;
  public LogDbContext(IHostEnvironment env) { Env = env; }
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
    var projPath = Path.Combine(Env.ContentRootPath, Env.ApplicationName);
    var databasePath = projPath + @"\Logging\Database\LogDatabase.sqlite";
    optionsBuilder.UseSqlite($"Data Source={databasePath};");
  }

  public DbSet<Log> Logs { get; set; } = null!;
}