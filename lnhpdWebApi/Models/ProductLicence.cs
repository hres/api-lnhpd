using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace lnhpdWebApi.Models
{
    public class ProductLicence
    {
        public int FileNumber { get; set; }
        public int SubmissionId { get; set; }
        public string LicenceNumber { get; set; }
        public DateTime? LicenceDate { get; set; }
        public DateTime? RevisedDate { get; set; }
        public DateTime? TimeReceipt { get; set; }
        public DateTime? DateStart { get; set; }
        public string Notes { get; set; }
        public int ProductNameId { get; set; }
        public string ProductName { get; set; }
        public string DosageForm { get; set; }
        public int CompanyId { get; set; }
        public int CompanyNameId { get; set; }
        public string CompanyName { get; set; }
        public int SubSubmissionTypeCode { get; set; }
        public string SubSubmissionTypeDesc { get; set; }
        public int FlagPrimaryName { get; set; }
        public int FlagProductStatus { get; set; }
        public int FlagAttestedMonograph { get; set; }
    }
}