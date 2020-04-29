using dhpr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace lnhpdWebApi.Models
{
    public class MedicinalIngredient
    {
        public int lnhpd_id { get; set; }
        public string ingredient_name { get; set; }
        public biliLang ingredient_Text { get; set; }
        
        //public int matrix_id { get; set; }
        //public int matrix_type_code { get; set; }
        //public List<IngredientQuantity> quantity_list { get; set; }
        public double potency_amount { get; set; }
        public string potency_constituent { get; set; }
        public string potency_unit_of_measure { get; set; }
        public double quantity { get; set; }
        public double quantity_minimum { get; set; }
        public double quantity_maximum { get; set; }
        public string quantity_unit_of_measure { get; set; }
        public string ratio_numerator { get; set; }
        public string ratio_denominator { get; set; }
        public string dried_herb_equivalent { get; set; }       
        public string dhe_unit_of_measure { get; set; }
        public string extract_type_desc { get; set; }
        public string source_material { get; set; }

        
        //public string brand_name { get; set; }
    }
        
}