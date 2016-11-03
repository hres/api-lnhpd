using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace lnhpdWebApi.Models
{
    public class ProductRisk
    {
        public int submission_id { get; set; }
        public int risk_id { get; set; }
        public string RiskTypeDesc { get; set; }
        public string SubRiskTypeDesc { get; set; }

        public List<ProductRiskText> RiskTextList { get; set; }
    }
}