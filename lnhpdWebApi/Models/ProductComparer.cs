using System;
using System.Collections.Generic;
using System.Web;

namespace lnhpdWebApi.Models
{
    public class ProductComparer : IEqualityComparer<ProductLicence>
    {

        public bool Equals(ProductLicence p1, ProductLicence p2)
        {
            //Check whether the objects are the same object. 
            if (Object.ReferenceEquals(p1, p2)) return true;

            //Check whether the reports' properties are equal.
            return p1.lnhpd_id == p2.lnhpd_id;
        }

        public int GetHashCode(ProductLicence p)
        {
            return p.lnhpd_id.GetHashCode();
        }

    }
}
