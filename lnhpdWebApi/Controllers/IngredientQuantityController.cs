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

        public IEnumerable<IngredientQuantity> GetAllIngredientQuantity()
        {

            return databasePlaceholder.GetAll();
        }


        public IngredientQuantity GetIngredientQuantityByID(int id)
        {
            IngredientQuantity quantity = databasePlaceholder.Get(id);
            if (quantity == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return quantity;
        }
    }
}