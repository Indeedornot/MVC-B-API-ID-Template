namespace MVCServer.TokenMiddleware {
  public static class TokenMiddlewareExtensions {
    public static IApplicationBuilder UseTokenMiddleware(this IApplicationBuilder builder) {
      return builder.UseMiddleware<TokenMiddleware>();
    }
  }
}