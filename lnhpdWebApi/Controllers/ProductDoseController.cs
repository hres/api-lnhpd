using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using lnhpdWebApi.Models;

namespace lnhpdWebApi.Controllers
{
    public class ProductDoseController : ApiController
    {
        static readonly IProductDoseRepository databasePlaceholder = new ProductDoseRepository();

        public IEnumerable<ProductDose> GetAllProductDose(string lang = "en")
        {

            return databasePlaceholder.GetAll(lang);
        }


        public IEnumerable<ProductDose> GetProductDoseByID(int id, string lang = "en")
        {
             return databasePlaceholder.Get(id, lang);
            //if (dose == null)
            //{
            //    throw new HttpResponseException(HttpStatusCode.NotFound);
            //}
            //return dose;
        }
    }
}