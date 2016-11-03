using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace lnhpdWebApi.Models
{
    public class ProductDose
    {
        public int submission_id { get; set; }
        public int dose_id { get; set; }
        public string population_type_desc { get; set; }
        public int age { get; set; }
        public int age_minimum { get; set; }
        public int age_maximum { get; set; }
        public string age_uom_type_desc { get; set; }
        public int quantity_dose { get; set; }
        public int quantity_dose_minimum { get; set; }
        public int quantity_dose_maximum { get; set; }
        public string quantity_dose_uom_type_desc { get; set; }
        public int frequency { get; set; }
        public int frequency_minimum { get; set; }
        public int frequency_maximum { get; set; }
        public string frequency_uom_type_desc { get; set; }
    }
}