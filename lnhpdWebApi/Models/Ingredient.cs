using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace lnhpdWebApi.Models
{
    public class Ingredient
    {
        public int SubmissionId { get; set; }
        public string IngredientName { get; set; }
        public int MatrixId { get; set; }
        public int MatrixTypeCode { get; set; }
    }
}