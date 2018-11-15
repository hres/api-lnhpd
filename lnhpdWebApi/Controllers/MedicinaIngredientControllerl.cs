using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using lnhpdWebApi.Models.Todo;
using lnhpdWebApi.Models.Response;
using lnhpdWebApi.Models.LNHPD;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace lnhpdWebApi.Controller
{
  [Produces("application/json", "application/xml")]
  [Route("api/medicinal-ingredient")]
  [Route("api/ingredient-medicinale")]
  [ApiController]
  public class MedicinalIngredientController : ControllerBase
  {
    // can't use DI as EF requires a DbContext reference, which limits our ability to transform data and there is no OracleProvider
    private readonly MedicinalIngredientContext _context = new MedicinalIngredientContext();

    [HttpGet("{id}")]
    public ActionResult<Response<MedicinalIngredient>> GetMedicinalIngredientById(int id, string lang = "en")
    {
      Response<MedicinalIngredient> response = _context.GetMedicinalIngredientById(id, lang);
      if (response == null) return NotFound();
      return response;
    }

    [HttpGet]
    public ActionResult<Response<List<MedicinalIngredient>>> GetAllMedicinalIngredient(int offset, int page, string lang = "en")
    {
      return _context.GetAllMedicinalIngredient(new Models.Request.RequestInfo { offset = offset, page = page });
    }
  }
}

// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Net;
// using System.Net.Http;
// using System.Web.Http;
// using lnhpdWebApi.Models;

// namespace lnhpdWebApi.Controllers
// {
//     public class MedicinalIngredientController : ApiController
//     {
//         static readonly IMedicinalIngredientRepository databasePlaceholder = new MedicinalIngredientRepository();

//         public IEnumerable<MedicinalIngredient> GetAllMedicinalIngredient(string lang = "en")
//         {

//             return databasePlaceholder.GetAllMedicinal(lang);
//         }


//         public IEnumerable<MedicinalIngredient> GetMedicinalIngredientyByID(int id, string lang = "en")
//         {
//             //Ingredient ingredient = databasePlaceholder.Get(id, lang);
//             //if (ingredient == null)
//             //{
//             //    throw new HttpResponseException(HttpStatusCode.NotFound);
//             //}
//             //return ingredient;
//             return databasePlaceholder.GetMedicinal(id, lang);
//         }
//     }
// }