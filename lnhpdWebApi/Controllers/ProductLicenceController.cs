using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using lnhpdWebApi.Models;
namespace lnhpdWebApi.Controllers
{
    public class ProductLicenceController : ApiController
    {
        static readonly IProductLicenceRepository databasePlaceholder = new ProductLicenceRepository();

        
        public IEnumerable<ProductLicence> GetByCriteria(string brandname, string ingredient, string companyname, string din, string lang = "en")
        {

            return databasePlaceholder.GetAllProductByCriteria(brandname, ingredient, companyname, din, lang);
        }


        public IEnumerable<ProductLicence> GetLicenceCompanyByID(int id, string lang = "en")
        {

            return databasePlaceholder.Get(id, lang);
            //if (licence == null)
            //{
            //    throw new HttpResponseException(HttpStatusCode.NotFound);
            //}
            //return licence;
        }

        public IEnumerable<ProductLicence> GetAllProductLicence(string lang = "en")
        {

            return databasePlaceholder.GetAll(lang);
        }
    }
}
