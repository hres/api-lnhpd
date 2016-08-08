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
        public string UomTypeDescAmtQuantity { get; set; }
        public string RatioNumerator { get; set; }
        public string RatioDenominator { get; set; }
        public string DriedHerbEquivalent { get; set; }
        public string UomTypeDescDhe { get; set; }
        public string ExtractTypeDesc { get; set; }
        public int PotencyAmount { get; set; }
        public string UomTypeDescPotency { get; set; }
        public string PotencyConstituent { get; set; }
    }
}