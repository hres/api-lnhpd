using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using lnhpdWebApi.Models;

namespace lnhpdWebApi.Controllers
{
    public class ProductRouteController : ApiController
    {
        static readonly IProductRouteRepository databasePlaceholder = new ProductRouteRepository();

        public IEnumerable<ProductRoute> GetAllProductRoute(string lang)
        {

            return databasePlaceholder.GetAll(lang);
        }


        public ProductRoute GetProductRouteByID(int id, string lang)
        {
            ProductRoute route = databasePlaceholder.Get(id, lang);
            if (route == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return route;
        }
    }
}