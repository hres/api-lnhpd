using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace lnhpdWebApi.Models
{
    public class IngredientSource
    {
        public int MaterialId { get; set; }
        public int MatrixId { get; set; }
        public string MaterialTypeDesc { get; set; }
    }
}