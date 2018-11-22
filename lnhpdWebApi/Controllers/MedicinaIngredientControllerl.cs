
using lnhpdWebApi.Models;
using lnhpdWebApi.Models.Request;
using lnhpdWebApi.Models.Response;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.Http;

namespace lnhpdWebApi.Controllers
{
    // [Route("api/medicinal-ingredient")]
    // [Route("api/ingredient-medicinale")]
    public class MedicinalIngredientController : ApiController
    {
        // can't use DI as EF requires a DbContext reference, which limits our ability to transform data and there is no OracleProvider
        private readonly MedicinalIngredientContext _context = new MedicinalIngredientContext();

        public Response<MedicinalIngredient> GetMedicinalIngredientById(int id, string lang = "en")
        {
            Response<MedicinalIngredient> response = _context.GetMedicinalIngredientById(id, lang);
            return response;
        }

        public Response<List<MedicinalIngredient>> GetAllMedicinalIngredient(int page = 1, string lang = "en")
        {
            var context = System.Web.HttpContext.Current;
            return _context.GetAllMedicinalIngredient(new RequestInfo { page = page, context = context });
        }
    }
}


/*
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
 */