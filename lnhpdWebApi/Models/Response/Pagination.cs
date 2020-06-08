using System;

namespace lnhpdWebApi.Models.Response
{
    public class Pagination
    {
        public int limit { get; set; }
        public int page { get; set; }
        public int total { get; set; }
        public string next { get; set; }
        public string previous { get; set; }

    }
}