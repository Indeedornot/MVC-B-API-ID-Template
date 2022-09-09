using APIServer.Logging.Database;

namespace APIServer.Logging.Middleware;

public static class LoggingMiddlewareExtensions {
  public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder builder)
    => builder.UseMiddleware<LoggingMiddleware>();
}