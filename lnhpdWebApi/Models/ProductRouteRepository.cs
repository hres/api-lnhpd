using lnhpd;
using System.Collections.Generic;

namespace lnhpdWebApi.Models
{
    public class ProductRouteRepository : IProductRouteRepository
    {
        private List<ProductRoute> _routes = new List<ProductRoute>();
        private ProductRoute _route = new ProductRoute();
        DBConnection dbConnection = new DBConnection("en");

        public IEnumerable<ProductRoute> GetAll()
        {
            _routes = dbConnection.GetAllProductRoute();
            return _routes;
        }

        public ProductRoute Get(int id)
        {
            _route = dbConnection.GetProductRouteById(id);
            return _route;
        }
    }
}