using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using lnhpdWebApi.Models;

namespace lnhpdWebApi.Controllers
{
    public class ProductPurposeController : ApiController
    {
        static readonly IProductPurposeRepository databasePlaceholder = new ProductPurposeRepository();

        public IEnumerable<ProductPurpose> GetAllProductPurpose(string lang = "en")
        {

            return databasePlaceholder.GetAll(lang);
        }


        public ProductPurpose GetProductPurposeByID(int id, string lang = "en")
        {
            ProductPurpose purpose = databasePlaceholder.Get(id, lang);
            if (purpose == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return purpose;
        }
    }
}