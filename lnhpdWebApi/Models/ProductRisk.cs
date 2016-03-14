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
        public string RiskTypeDescE { get; set; }
        public string RiskTypeDescF { get; set; }
        public string SubRiskTypeDescE { get; set; }
        public string SubRiskTypeDescF { get; set; }
    }
}