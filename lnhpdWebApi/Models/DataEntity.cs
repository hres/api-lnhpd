using System;
using System.Collections.Generic;



namespace dhpr
{
    public enum programType
    {
        dhpr,
        dpd,
        rds,
        ssr,
        sbd,
        lnhpd
    }


    public class dpdSearchItem
    {
        public string Id { get; set; }
        public string BrandName { get; set; }
        public string company_name { get; set; }
        public string DrugIdentificationNumber { get; set; }
        
    }

}