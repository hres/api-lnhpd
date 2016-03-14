using lnhpd;
using System.Collections.Generic;

namespace lnhpdWebApi.Models
{
    public class ProductDoseRepository : IProductDoseRepository
    {
        private List<ProductDose> _doses = new List<ProductDose>();
        private ProductDose _dose = new ProductDose();
        DBConnection dbConnection = new DBConnection("en");

        public IEnumerable<ProductDose> GetAll()
        {
            _doses = dbConnection.GetAllProductDose();
            return _doses;
        }

        public ProductDose Get(int id)
        {
            _dose = dbConnection.GetProductDoseById(id);
            return _dose;
        }
    }
}