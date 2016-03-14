using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace lnhpdWebApi.Models
{
    public class ProductDose
    {
        public int SubmissionId { get; set; }
        public int DoseId { get; set; }
        public string PopulationTypeDescE { get; set; }
        public string PopulationTypeDescF { get; set; }
        public int Age { get; set; }
        public int AgeMinimum { get; set; }
        public int AgeMaximum { get; set; }
        public string UomTypeDescAgeE { get; set; }
        public string UomTypeDescAgeF { get; set; }
        public int QuantityDose { get; set; }
        public int QuantityMinimumDose { get; set; }
        public int QuantityMaximumDose { get; set; }
        public string UomTypeDescQuantityDoseE { get; set; }
        public string UomTypeDescQuantityDoseF { get; set; }
        public int Frequency { get; set; }
        public int FrequencyMinimum { get; set; }
        public int FrequencyMaximum { get; set; }
        public string UomTypeDescFrequencyE { get; set; }
        public string UomTypeDescFrequencyF { get; set; }
    }
}