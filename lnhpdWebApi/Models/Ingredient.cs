using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace lnhpdWebApi.Models
{
    public class Ingredient
    {
        public int submission_id { get; set; }
        public string ingredient_name { get; set; }
        public int matrix_id { get; set; }
        public int matrix_type_code { get; set; }
        public List<IngredientQuantity> quantity_list { get; set; }
    }
}