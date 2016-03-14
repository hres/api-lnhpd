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

        public IEnumerable<ProductRisk> GetAllProductRisk()
        {

            return databasePlaceholder.GetAll();
        }


        public ProductRisk GetProductRiskByID(int id)
        {
            ProductRisk risk = databasePlaceholder.Get(id);
            if (risk == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return risk;
        }
    }
}