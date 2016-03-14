using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace lnhpdWebApi.Models
{
    public class IngredientQuantity
    {
        public int IngredientAmountId { get; set; }
        public int MatrixId { get; set; }
        public int Quantity { get; set; }
        public int QuantityMinimum { get; set; }
        public int QuantityMaximum { get; set; }
        public string UomTypeDescAmtQuantityE { get; set; }
        public string UomTypeDescAmtQuantityF { get; set; }
        public string RatioNumerator { get; set; }
        public string RatioDenominator { get; set; }
        public string DriedHerbEquivalent { get; set; }
        public string UomTypeDescDheE { get; set; }
        public string UomTypeDescDheF { get; set; }
        public string ExtractTypeDescE { get; set; }
        public string ExtractTypeDescF { get; set; }
        public int PotencyAmount { get; set; }
        public string UomTypeDescPotencyE { get; set; }
        public string UomTypeDescPotencyF { get; set; }
        public string PotencyConstituent { get; set; }
    }
}