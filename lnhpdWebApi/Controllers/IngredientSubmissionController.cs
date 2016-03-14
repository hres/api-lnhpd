using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using lnhpdWebApi.Models;

namespace lnhpdWebApi.Controllers
{
    public class IngredientSubmissionController : ApiController
    {
        static readonly IIngredientSubmissionRepository databasePlaceholder = new IngredientSubmissionRepository();

        public IEnumerable<IngredientSubmission> GetAllIngredientSubmission()
        {

            return databasePlaceholder.GetAll();
        }


        public IngredientSubmission GetIngredientSubmissionByID(int id)
        {
            IngredientSubmission submission = databasePlaceholder.Get(id);
            if (submission == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return submission;
        }
    }
}