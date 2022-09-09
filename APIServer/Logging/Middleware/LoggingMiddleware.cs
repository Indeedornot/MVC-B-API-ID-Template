using APIServer.Logging.Database;
using APIServer.Logging.Model;
using Microsoft.IO;

namespace APIServer.Logging.Middleware;

public class LoggingMiddleware {
  private readonly RequestDelegate _next;
  private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;

  public LoggingMiddleware(
    RequestDelegate next,
    ILoggerFactory loggerFactory) {
    _next = next;
    _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
  }

  public async Task Invoke(HttpContext context, LogDbContext logDbContext) {
    Console.WriteLine("LoggingMiddleware.Invoke");
    await LogRequest(context, logDbContext);
    await LogResponse(context, logDbContext);
  }

  private async Task LogRequest(HttpContext context, LogDbContext logDbContext) {
    context.Request.EnableBuffering();
    await using var requestStream = _recyclableMemoryStreamManager.GetStream();
    await context.Request.Body.CopyToAsync(requestStream);

    logDbContext.Logs.Add(
      new Log {
        Body = ReadStreamInChunks(requestStream),
        Schema = context.Request.Scheme,
        Host = context.Request.Host.ToString(),
        Path = context.Request.Path.ToString(),
        QueryString = context.Request.QueryString.ToString(),
        Type = "Request"
      }
    );
    await logDbContext.SaveChangesAsync();

    context.Request.Body.Position = 0;
  }

  private static string ReadStreamInChunks(Stream stream) {
    const int readChunkBufferLength = 4096;
    stream.Seek(0, SeekOrigin.Begin);
    using var textWriter = new StringWriter();
    using var reader = new StreamReader(stream);
    char[] readChunk = new char[readChunkBufferLength];
    int readChunkLength;
    do{
      readChunkLength = reader.ReadBlock(
        readChunk,
        0,
        readChunkBufferLength
      );
      textWriter.Write(readChunk, 0, readChunkLength);
    } while (readChunkLength > 0);

    return textWriter.ToString();
  }

  private async Task LogResponse(HttpContext context, LogDbContext logDbContext) {
    await using var originalBodyStream = context.Response.Body;
    try{
      using var responseBody = _recyclableMemoryStreamManager.GetStream();
      context.Response.Body = responseBody;

      await _next(context);

      context.Response.Body.Seek(0, SeekOrigin.Begin);
      string text = await new StreamReader(context.Response.Body).ReadToEndAsync();
      context.Response.Body.Seek(0, SeekOrigin.Begin);

      await responseBody.CopyToAsync(originalBodyStream);

      logDbContext.Logs.Add(
        new Log {
          Body = text,
          Schema = context.Request.Scheme,
          Host = context.Request.Host.ToString(),
          Path = context.Request.Path.ToString(),
          QueryString = context.Request.QueryString.ToString(),
          Type = "Response"
        }
      );
      await logDbContext.SaveChangesAsync();
    }
    finally{
      context.Response.Body = originalBodyStream;
    }
  }
}