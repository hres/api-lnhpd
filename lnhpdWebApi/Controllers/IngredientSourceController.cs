using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using lnhpdWebApi.Models;

namespace lnhpdWebApi.Controllers
{
    public class IngredientSourceController : ApiController
    {
        static readonly IIngredientSourceRepository databasePlaceholder = new IngredientSourceRepository();

        public IEnumerable<IngredientSource> GetAllIngredientSource(string lang = "en")
        {

            return databasePlaceholder.GetAll(lang);
        }


        public IngredientSource GetIngredientSourceByID(int id, string lang = "en")
        {
            IngredientSource source = databasePlaceholder.Get(id, lang);
            if (source == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return source;
        }
    }
}