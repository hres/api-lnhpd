using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lnhpdWebApi.Models
{
    interface IProductLicenceRepository
    {
        IEnumerable<ProductLicence> GetAll(string lang);
        IEnumerable<ProductLicence> Get(int id, string lang);
        IEnumerable<ProductLicence> GetAllProductByCriteria(string brandname, string ingredient, string companyname, string din, string lang);
        
    }
}
