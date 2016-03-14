using lnhpd;
using System.Collections.Generic;

namespace lnhpdWebApi.Models
{
    public class IngredientQuantityRepository : IIngredientQuantityRepository
    {
        private List<IngredientQuantity> _quantities = new List<IngredientQuantity>();
        private IngredientQuantity _quantity = new IngredientQuantity();
        DBConnection dbConnection = new DBConnection("en");

        public IEnumerable<IngredientQuantity> GetAll()
        {
            _quantities = dbConnection.GetAllIngredientQuantity();
            return _quantities;
        }

        public IngredientQuantity Get(int id)
        {
            _quantity = dbConnection.GetIngredientQuantityById(id);
            return _quantity;
        }
    }
}