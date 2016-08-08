using lnhpd;
using System.Collections.Generic;

namespace lnhpdWebApi.Models
{
    public class ProductDoseRepository : IProductDoseRepository
    {
        private List<ProductDose> _doses = new List<ProductDose>();
        private ProductDose _dose = new ProductDose();
        DBConnection dbConnection = new DBConnection("en");

        public IEnumerable<ProductDose> GetAll(string lang)
        {
            _doses = dbConnection.GetAllProductDose(lang);
            return _doses;
        }

        public ProductDose Get(int id, string lang)
        {
            _dose = dbConnection.GetProductDoseById(id, lang);
            return _dose;
        }
    }
}