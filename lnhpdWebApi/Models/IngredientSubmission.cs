using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace lnhpdWebApi.Models
{
    public class IngredientSubmission
    {
        public int MatrixId { get; set; }
        public int SubmissionId { get; set; }
        public int IngredientTypeCode { get; set; }
        public string NameUpper { get; set; }
    }
}