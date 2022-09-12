namespace MVCServer.TokenMiddleware {
  public class TokenMiddleware {
    private readonly RequestDelegate _next;
    public TokenMiddleware(RequestDelegate next) {
      _next = next;
    }

    // public async Task Invoke(HttpContext ctx) {
    //   if (context.Request)
    //     await _next.Invoke(ctx);
    // }
  }
}