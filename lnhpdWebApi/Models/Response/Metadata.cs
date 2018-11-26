using System;

namespace lnhpdWebApi.Models.Response
{
  public class Metadata
  {
    public Pagination pagination { get; set; }
    public DateTime dateCreated { get; set; } = DateTime.Now.ToUniversalTime();
  }
}