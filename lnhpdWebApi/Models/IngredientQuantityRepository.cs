using lnhpd;
using System.Collections.Generic;

namespace lnhpdWebApi.Models
{
    public class IngredientQuantityRepository : IIngredientQuantityRepository
    {
        private List<IngredientQuantity> _quantities = new List<IngredientQuantity>();
        private IngredientQuantity _quantity = new IngredientQuantity();
        DBConnection dbConnection = new DBConnection("en");

        public IEnumerable<IngredientQuantity> GetAll(string lang)
        {
            _quantities = dbConnection.GetAllIngredientQuantity(lang);
            return _quantities;
        }

        public IngredientQuantity Get(int id, string lang)
        {
            _quantity = dbConnection.GetIngredientQuantityById(id, lang);
            return _quantity;
        }
    }
}