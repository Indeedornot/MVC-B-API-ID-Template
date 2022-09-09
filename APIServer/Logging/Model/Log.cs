using System.ComponentModel.DataAnnotations;

namespace APIServer.Logging.Model;

public class Log {
  /*
   $"Http Response Information:{Environment.NewLine}" +
        $"Schema:{context.Request.Scheme} " +
        $"Host: {context.Request.Host} " +
        $"Path: {context.Request.Path} " +
        $"QueryString: {context.Request.QueryString} " +
        $"Body: {text}"
        Type: Response/Request
   */
  [Key]
  public int Id { get; set; }
  public string Type { get; set; }
  public string Host { get; set; }
  public string User { get; set; } = "Anonymous";
  public string Body { get; set; }
  public string Path { get; set; }
  public string Schema { get; set; }
  public string QueryString { get; set; }

  public DateTime CreatedDate { get; set; } = DateTime.Now;
}