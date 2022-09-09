using System.Data;
using Microsoft.EntityFrameworkCore;

namespace APIServer.Database; 

public static class CrudExtensions {
  public async static Task<T> Create<T>(this DbContext connection, T entity) where T : class {
    var result = await connection.AddAsync(entity);
    return result.Entity;
  }
}