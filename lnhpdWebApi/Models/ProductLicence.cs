using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace lnhpdWebApi.Models
{
    public class ProductLicence
    {
        public int SubmissionId { get; set; }
        public string LicenceNumber { get; set; }
        public int CompanyNameId { get; set; }
        public string CompanyName { get; set; }
        public string ProductName { get; set; }
        public string DosageFormE { get; set; }
        public string DosageFormF { get; set; }
        public string SubSubmissionTypeDescE { get; set; }
        public string SubSubmissionTypeDescF { get; set; }
        public int FlagPrimaryName { get; set; }
        public int FlagProductStatus { get; set; }
        public int FlagAttestedMonograph { get; set; }
    }
}