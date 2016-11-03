using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace lnhpdWebApi.Models
{
    public class IngredientSource
    {
        public int material_id { get; set; }
        public int matrix_id { get; set; }
        public string material_type_desc { get; set; }
    }
}