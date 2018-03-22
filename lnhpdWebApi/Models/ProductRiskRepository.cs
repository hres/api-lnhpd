using lnhpd;
using System.Collections.Generic;

namespace lnhpdWebApi.Models
{
    public class ProductRiskRepository : IProductRiskRepository
    {
        private List<ProductRisk> _risks = new List<ProductRisk>();
        private ProductRisk _risk = new ProductRisk();
        DBConnection dbConnection = new DBConnection("en");

        public IEnumerable<ProductRisk> GetAll(string lang)
        {
            _risks = dbConnection.GetAllProductRisk(lang);
            return _risks;
        }

        public IEnumerable<ProductRisk> Get(int id, string lang)
        {
            _risks = dbConnection.GetProductRiskById(id, lang);
            return _risks;
        }
    }
}