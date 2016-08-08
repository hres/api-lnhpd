using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using lnhpdWebApi.Models;

namespace lnhpdWebApi.Controllers
{
    public class IngredientController : ApiController
    {
        static readonly IIngredientRepository databasePlaceholder = new IngredientRepository();

        public IEnumerable<Ingredient> GetAllIngredient(string lang)
        {

            return databasePlaceholder.GetAll(lang);
        }


        public Ingredient GetIngredientyByID(int id, string lang)
        {
            Ingredient ingredient = databasePlaceholder.Get(id, lang);
            if (ingredient == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return ingredient;
        }
    }
}