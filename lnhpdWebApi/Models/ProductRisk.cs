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
        public string risk_type_desc { get; set; }
        public string sub_risk_type_desc { get; set; }

        //public List<ProductRiskText> risk_text_list { get; set; }
        public string risk_text { get; set; }
    }
}