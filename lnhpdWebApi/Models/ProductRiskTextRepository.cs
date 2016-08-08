using lnhpd;
using System.Collections.Generic;

namespace lnhpdWebApi.Models
{
    public class ProductRiskTextRepository : IProductRiskTextRepository
    {
        private List<ProductRiskText> _riskTexts = new List<ProductRiskText>();
        private ProductRiskText _riskText = new ProductRiskText();
        DBConnection dbConnection = new DBConnection("en");

        public IEnumerable<ProductRiskText> GetAll(string lang)
        {
            _riskTexts = dbConnection.GetAllProductRiskText(lang);
            return _riskTexts;
        }

        public ProductRiskText Get(int id, string lang)
        {
            _riskText = dbConnection.GetProductRiskTextById(id, lang);
            return _riskText;
        }
    }
}