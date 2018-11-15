namespace lnhpdWebApi.Models.Response
{
  public class Pagination
  {
    public int limit { get; set; }
    public int offset { get; set; }
    public int page { get; set; }
    public int count { get; set; }
  }
}