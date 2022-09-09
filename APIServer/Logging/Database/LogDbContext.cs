using APIServer.Database;
using APIServer.Logging.Model;
using Microsoft.EntityFrameworkCore;

namespace APIServer.Logging.Database;

public class LogDbContext : DbContext {
  public LogDbContext(DbContextOptions<LogDbContext> options) : base(options) { }

  public DbSet<Log> Logs { get; set; } = null!;

  // public override int SaveChanges() {
  //   var entities = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && x.State is EntityState.Added or EntityState.Modified);
  //   foreach (var entity in entities){
  //     var baseEntity = (BaseEntity) entity.Entity;
  //     if (entity.State is EntityState.Added){ baseEntity.CreatedDate = DateTime.Now; }
  //     baseEntity.UpdatedDate = DateTime.Now;
  //   }
  //
  //   return base.SaveChanges();
  // }

  // public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new()) {
  //   var entities = ChangeTracker.Entries().Where(x => x.State is EntityState.Added);
  //   foreach (var entity in entities){
  //     ((Log) entity.Entity).CreatedDate = DateTime.Now;
  //   }
  //
  //   return base.SaveChangesAsync(cancellationToken);
  // }
}