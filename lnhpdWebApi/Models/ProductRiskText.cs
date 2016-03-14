using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace lnhpdWebApi.Models
{
    public class ProductRiskText
    {
        public int TextId { get; set; }
        public int RiskId { get; set; }
        public string RiskTypeE { get; set; }
        public string RiskTypeF { get; set; }
    }
}