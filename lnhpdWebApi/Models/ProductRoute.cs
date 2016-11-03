using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace lnhpdWebApi.Models
{
    public class ProductRoute
    {
        public int submission_id { get; set; }
        public int route_id { get; set; }
        public string route_type_desc { get; set; }
    }
}