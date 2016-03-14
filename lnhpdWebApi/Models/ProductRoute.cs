using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace lnhpdWebApi.Models
{
    public class ProductRoute
    {
        public int SubmissionId { get; set; }
        public int RouteId { get; set; }
        public string RouteTypeDescE { get; set; }
        public string RouteTypeDescF { get; set; }
    }
}