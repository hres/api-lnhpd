using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using lnhpdWebApi.Models;

namespace lnhpdWebApi.Controllers
{
    public class ProductRiskController : ApiController
    {
        static readonly IProductRiskRepository databasePlaceholder = new ProductRiskRepository();

        public IEnumerable<ProductRisk> GetAllProductRisk(string lang)
        {

            return databasePlaceholder.GetAll(lang);
        }


        public ProductRisk GetProductRiskByID(int id, string lang)
        {
            ProductRisk risk = databasePlaceholder.Get(id, lang);
            if (risk == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return risk;
        }
    }
}