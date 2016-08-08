using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace lnhpdWebApi.Models
{
    public class ProductRisk
    {
        public int SubmissionId { get; set; }
        public int RiskId { get; set; }
        public string RiskTypeDesc { get; set; }
        public string SubRiskTypeDesc { get; set; }
    }
}