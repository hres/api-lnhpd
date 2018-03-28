using lnhpd;
using System.Collections.Generic;

namespace lnhpdWebApi.Models
{
    public class ProductPurposeRepository : IProductPurposeRepository
    {
        private List<ProductPurpose> _purposes = new List<ProductPurpose>();
        private ProductPurpose _purpose = new ProductPurpose();
        DBConnection dbConnection = new DBConnection("en");

        public IEnumerable<ProductPurpose> GetAll(string lang)
        {
            _purposes = dbConnection.GetAllProductPurpose(lang);
            return _purposes;
        }

        public IEnumerable<ProductPurpose> Get(int id, string lang)
        {
            _purposes = dbConnection.GetProductPurposeById(id, lang);
            return _purposes;
        }
    }
}