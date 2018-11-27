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
    public class ProductRiskController : ApiController
    {
        // can't use DI as EF requires a DbContext reference, which limits our ability to transform data and there is no OracleProvider
        private readonly ProductRiskContext _context = new ProductRiskContext();

        public Response<List<ProductRisk>> GetProductRiskById(int id, string lang = "en")
        {
            Response<List<ProductRisk>> response = _context.GetProductRiskById(id, lang);
            return response;
        }

        public Response<List<ProductRisk>> GetAllProductRisk(string type = "json", int page = 1, string lang = "en")
        {
            var context = System.Web.HttpContext.Current;
            return _context.GetAllProductRisk(new RequestInfo { page = page, context = context, languages = lang, type = type });
        }
    }
}

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web.Http;
//using lnhpdWebApi.Models;

//namespace lnhpdWebApi.Controllers
//{
//    public class ProductRiskController : ApiController
//    {
//        static readonly IProductRiskRepository databasePlaceholder = new ProductRiskRepository();

//        public IEnumerable<ProductRisk> GetAllProductRisk(string lang = "en")
//        {

//            return databasePlaceholder.GetAll(lang);
//        }


//        public IEnumerable<ProductRisk> GetProductRiskByID(int id, string lang = "en")
//        {
//             return databasePlaceholder.Get(id, lang);
//            //if (risk == null)
//            //{
//            //    throw new HttpResponseException(HttpStatusCode.NotFound);
//            //}
//            //return risk;
//        }
//    }
//}