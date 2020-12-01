using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace lnhpdWebApi.Models
{
    public class ProductDose
    {
        public int lnhpd_id { get; set; }
        public int dose_id { get; set; }
        public string population_type_desc { get; set; }
        public int age { get; set; }
        public double age_minimum { get; set; }
        public double age_maximum { get; set; }
        public string uom_type_desc_age { get; set; }
        public double quantity_dose { get; set; }
        public double quantity_dose_minimum { get; set; }
        public double quantity_dose_maximum { get; set; }
        public string uom_type_desc_quantity_dose { get; set; }
        public double frequency { get; set; }
        public double frequency_minimum { get; set; }
        public double frequency_maximum { get; set; }
        public string uom_type_desc_frequency { get; set; }
    }
}