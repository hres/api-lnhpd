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
        public string RiskType { get; set; }
    }
}