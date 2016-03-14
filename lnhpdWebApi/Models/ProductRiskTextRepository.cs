using lnhpd;
using System.Collections.Generic;

namespace lnhpdWebApi.Models
{
    public class ProductRiskTextRepository : IProductRiskTextRepository
    {
        private List<ProductRiskText> _riskTexts = new List<ProductRiskText>();
        private ProductRiskText _riskText = new ProductRiskText();
        DBConnection dbConnection = new DBConnection("en");

        public IEnumerable<ProductRiskText> GetAll()
        {
            _riskTexts = dbConnection.GetAllProductRiskText();
            return _riskTexts;
        }

        public ProductRiskText Get(int id)
        {
            _riskText = dbConnection.GetProductRiskTextById(id);
            return _riskText;
        }
    }
}