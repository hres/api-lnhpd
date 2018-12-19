using lnhpdWebApi.Models;
using lnhpdWebApi.Models.Request;
using lnhpdWebApi.Models.Response;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.Http;

namespace lnhpdWebApi.Controllers
{
    public class ProductPurposeController : ApiController
    {
        // can't use DI as EF requires a DbContext reference, which limits our ability to transform data and there is no OracleProvider
        private readonly ProductPurposeContext _context = new ProductPurposeContext();

        public Response<List<ProductPurpose>> GetProductPurposeById(int id, string lang = "en")
        {
            Response<List<ProductPurpose>> response = _context.GetProductPurposeById(id, lang);
            return response;
        }

        public Response<List<ProductPurpose>> GetAllProductPurpose(string type = "json", int page = 1, string lang = "en")
        {
            var context = System.Web.HttpContext.Current;
            return _context.GetAllProductPurpose(new RequestInfo { page = page, context = context, languages = lang, type = type });
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
//    public class ProductPurposeController : ApiController
//    {
//        static readonly IProductPurposeRepository databasePlaceholder = new ProductPurposeRepository();

//        public IEnumerable<ProductPurpose> GetAllProductPurpose(string lang = "en")
//        {
//            return databasePlaceholder.GetAll(lang);
//        }


//        public IEnumerable<ProductPurpose> GetProductPurposeByID(int id, string lang = "en")
//        {
//             return databasePlaceholder.Get(id, lang);
//            //if (purpose == null)
//            //{
//            //    throw new HttpResponseException(HttpStatusCode.NotFound);
//            //}
//            //return purpose;
//        }
//    }
//}