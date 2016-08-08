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

        public IEnumerable<ProductDose> GetAllProductDose(string lang)
        {

            return databasePlaceholder.GetAll(lang);
        }


        public ProductDose GetProductDoseByID(int id, string lang)
        {
            ProductDose dose = databasePlaceholder.Get(id, lang);
            if (dose == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return dose;
        }
    }
}