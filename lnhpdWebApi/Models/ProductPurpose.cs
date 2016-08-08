using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace lnhpdWebApi.Models
{
    public class ProductPurpose
    {
        public int TextId { get; set; }
        public int SubmissionId { get; set; }
        public String Purpose { get; set; }
    }
}