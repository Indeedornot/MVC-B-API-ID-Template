using APIServer.Logging.Model;
using Microsoft.EntityFrameworkCore;

namespace APIServer.Database;

public class AppDbContext : DbContext {
  private readonly IHostEnvironment Env;
  public AppDbContext(IHostEnvironment env) { Env = env; }
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
    var projPath = Path.Combine(Env.ContentRootPath, Env.ApplicationName);
    var databasePath = projPath + @"\Database\Database.sqlite";
    optionsBuilder.UseSqlite($"Data Source={databasePath};");
  }

  public DbSet<Log> Logs { get; set; } = null!;

  //public DbSet<Log> Logs { get; set; } = null!;

  public override int SaveChanges() {
    var entities = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && x.State is EntityState.Added or EntityState.Modified);
    foreach (var entity in entities)
    {
      var baseEntity = (BaseEntity) entity.Entity;
      if (entity.State is EntityState.Added) { baseEntity.CreatedDate = DateTime.Now; }

      baseEntity.UpdatedDate = DateTime.Now;
    }

    return base.SaveChanges();
  }

  public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new()) {
    var entities = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && x.State is EntityState.Added or EntityState.Modified);
    foreach (var entity in entities)
    {
      var baseEntity = (BaseEntity) entity.Entity;
      if (entity.State is EntityState.Added) { baseEntity.CreatedDate = DateTime.Now; }

      baseEntity.UpdatedDate = DateTime.Now;
    }

    return base.SaveChangesAsync(cancellationToken);
  }
}