using System;

namespace lnhpdWebApi.Models.Response
{
  public class Metadata
  {
    public Pagination pagination { get; set; }
    public String dateCreated { get; set; } = DateTime.UtcNow.ToString("O");
  }
}