using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using lnhpdWebApi.Models;

namespace lnhpdWebApi.Controllers
{
    public class NonMedicinalIngredientController : ApiController
    {
        static readonly INonMedicinalIngredientRepository databasePlaceholder = new NonMedicinalIngredientRepository();

        public IEnumerable<NonMedicinalIngredient> GetAllNonMedicinalIngredient(string lang = "en")
        {

            return databasePlaceholder.GetAllNonMedicinal(lang);
        }


        public IEnumerable<NonMedicinalIngredient> GetNonMedicinalIngredientyByID(int id, string lang = "en")
        {
            //Ingredient ingredient = databasePlaceholder.Get(id, lang);
            //if (ingredient == null)
            //{
            //    throw new HttpResponseException(HttpStatusCode.NotFound);
            //}
            //return ingredient;
            return databasePlaceholder.GetNonMedicinal(id, lang);
        }
    }
}