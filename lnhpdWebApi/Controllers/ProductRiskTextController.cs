using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using lnhpdWebApi.Models;

namespace lnhpdWebApi.Controllers
{
    public class ProductRiskTextController : ApiController
    {
        static readonly IProductRiskTextRepository databasePlaceholder = new ProductRiskTextRepository();

        public IEnumerable<ProductRiskText> GetAllProductRiskText(string lang)
        {

            return databasePlaceholder.GetAll(lang);
        }


        public ProductRiskText GetProductRiskTextByID(int id, string lang)
        {
            ProductRiskText riskText = databasePlaceholder.Get(id, lang);
            if (riskText == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return riskText;
        }
    }
}