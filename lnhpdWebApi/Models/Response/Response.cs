namespace lnhpdWebApi.Models.Response
{
  public class Response<T>
  {
    public Metadata metadata { get; set; }
    public T data { get; set; }
  }
}