using lnhpdWebApi.Models;
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

    public class biliLang
    {
        public string en { get; set; }
        public string fr { get; set; } 

    }

    public class pageNation
    {
        public int limit { get; set; }
        public int totalCount { get; set; }

    }

    public class Result<T>
    {
        public pageNation pageInfo { get; set; }
        public IEnumerable<T> GetAllProductRoute { get; set; }
    }

}