using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace lnhpdWebApi.Models
{
    public class IngredientQuantity
    {
        public int ingredient_amount_id { get; set; }
        public string ingredient_name { get; set; }
        public int matrix_id { get; set; }
        public double quantity { get; set; }
        public double quantity_minimum { get; set; }
        public double quantity_maximum { get; set; }
        public string quantity_uom_type_desc { get; set; }
        public string quantity_string { get; set; }
        public string ratio_numerator { get; set; }
        public string ratio_denominator { get; set; }
        public string dried_herb_equivalent { get; set; }
        public string dried_herb_equivalent_uom_type_desc { get; set; }
        public string extract_type_desc { get; set; }
        public string extract_string { get; set; }
        public double potency { get; set; }
        public string potency_uom_type_desc { get; set; }
        public string potency_constituent { get; set; }
        public string potency_string { get; set; }
    }
}