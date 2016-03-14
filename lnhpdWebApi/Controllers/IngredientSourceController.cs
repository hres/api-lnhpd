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

        public IEnumerable<IngredientSource> GetAllIngredientSource()
        {

            return databasePlaceholder.GetAll();
        }


        public IngredientSource GetIngredientSourceByID(int id)
        {
            IngredientSource source = databasePlaceholder.Get(id);
            if (source == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return source;
        }
    }
}