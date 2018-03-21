using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace lnhpdWebApi.Models
{
    public class IngredientSubmission
    {
        public int matrix_id { get; set; }
        public int lnhpd_id { get; set; }
        public int ingredient_type_code { get; set; }
        public string brand_name { get; set; }
    }
}