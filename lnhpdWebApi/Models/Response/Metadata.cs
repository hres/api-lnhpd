using System;

namespace lnhpdWebApi.Models.Response
{
  public class Metadata
  {
    public Pagination pagination { get; set; }
    public String dateReceived { get; set; } = DateTime.UtcNow.ToString("O");
  }
}