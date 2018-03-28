using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace lnhpdWebApi.Models
{
    public class ProductLicence
    {
        //public int file_number { get; set; }
        public int lnhpd_id { get; set; }
        public string licence_number { get; set; }
        public DateTime? licence_date { get; set; }
        public DateTime? revised_date { get; set; }
        public DateTime? time_receipt { get; set; }
        public DateTime? date_start { get; set; }
        public string notes { get; set; }
        public int product_name_id { get; set; }
        public string product_name { get; set; }
        public string dosage_form { get; set; }
        public int company_id { get; set; }
        public int company_name_id { get; set; }
        public string company_name { get; set; }
        public int sub_submission_type_code { get; set; }
        public string sub_submission_type_desc { get; set; }
        public int flag_primary_name { get; set; }
        public int flag_product_status { get; set; }
        public int flag_attested_monograph { get; set; }
        
        //public List<string> secondary_brand_name_list { get; set; }
        //public List<ProductDose> dose_list { get; set; }
        //public List<ProductRoute> route_list { get; set; }
        //public List<ProductPurpose> purpose_list { get; set; }
        //public List<ProductRisk> risk_list { get; set; }
        //public List<MedicinalIngredient> medicinal_ingredient_list { get; set; }
        //public List<MedicinalIngredient> non_medicinal_ingredient_list { get; set; }
    }
}