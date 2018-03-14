using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using lnhpdWebApi.Models;

namespace lnhpdWebApi.Controllers
{
    public class MedicinalIngredientController : ApiController
    {
        static readonly IMedicinalIngredientRepository databasePlaceholder = new MedicinalIngredientRepository();

        public IEnumerable<MedicinalIngredient> GetAllMedicinalIngredient(string lang = "en")
        {

            return databasePlaceholder.GetAllMedicinal(lang);
        }


        public IEnumerable<MedicinalIngredient> GetMedicinalIngredientyByID(int id, string lang = "en")
        {
            //Ingredient ingredient = databasePlaceholder.Get(id, lang);
            //if (ingredient == null)
            //{
            //    throw new HttpResponseException(HttpStatusCode.NotFound);
            //}
            //return ingredient;
            return databasePlaceholder.GetMedicinal(id, lang);
        }
    }
}