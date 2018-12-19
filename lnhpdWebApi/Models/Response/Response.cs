namespace lnhpdWebApi.Models.Response
{
  public class Response<T>
  {
    public Metadata metadata { get; set; } = new Metadata();
    public T data { get; set; }
  }
}