using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using lnhpdWebApi.Models;

namespace lnhpdWebApi.Controllers
{
    public class IngredientQuantityController : ApiController
    {
        static readonly IIngredientQuantityRepository databasePlaceholder = new IngredientQuantityRepository();

        public IEnumerable<IngredientQuantity> GetAllIngredientQuantity(string lang="en")
        {

            return databasePlaceholder.GetAll(lang);
        }


        public IngredientQuantity GetIngredientQuantityByID(int id, string lang = "en")
        {
            IngredientQuantity quantity = databasePlaceholder.Get(id, lang);
            if (quantity == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return quantity;
        }
    }
}