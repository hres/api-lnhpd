using lnhpd;
using System.Collections.Generic;

namespace lnhpdWebApi.Models
{
    public class ProductRiskRepository : IProductRiskRepository
    {
        private List<ProductRisk> _risks = new List<ProductRisk>();
        private ProductRisk _risk = new ProductRisk();
        DBConnection dbConnection = new DBConnection("en");

        public IEnumerable<ProductRisk> GetAll()
        {
            _risks = dbConnection.GetAllProductRisk();
            return _risks;
        }

        public ProductRisk Get(int id)
        {
            _risk = dbConnection.GetProductRiskById(id);
            return _risk;
        }
    }
}