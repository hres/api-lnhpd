using lnhpd;
using System.Collections.Generic;

namespace lnhpdWebApi.Models
{
    public class ProductPurposeRepository : IProductPurposeRepository
    {
        private List<ProductPurpose> _purposes = new List<ProductPurpose>();
        private ProductPurpose _purpose = new ProductPurpose();
        DBConnection dbConnection = new DBConnection("en");

        public IEnumerable<ProductPurpose> GetAll()
        {
            _purposes = dbConnection.GetAllProductPurpose();
            return _purposes;
        }

        public ProductPurpose Get(int id)
        {
            _purpose = dbConnection.GetProductPurposeById(id);
            return _purpose;
        }
    }
}