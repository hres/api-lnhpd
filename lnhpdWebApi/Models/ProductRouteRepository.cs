using lnhpd;
using System.Collections.Generic;

namespace lnhpdWebApi.Models
{
    public class ProductRouteRepository : IProductRouteRepository
    {
        private List<ProductRoute> _routes = new List<ProductRoute>();
        private ProductRoute _route = new ProductRoute();
        DBConnection dbConnection = new DBConnection("en");

        public IEnumerable<ProductRoute> GetAll(string lang)
        {
            _routes = dbConnection.GetAllProductRoute(lang);
            return _routes;
        }

        public ProductRoute Get(int id, string lang)
        {
            _route = dbConnection.GetProductRouteById(id, lang);
            return _route;
        }
    }
}