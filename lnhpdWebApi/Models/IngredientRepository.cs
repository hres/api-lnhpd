using lnhpd;
using System.Collections.Generic;

namespace lnhpdWebApi.Models
{
    public class IngredientRepository : IIngredientRepository
    {
        private List<Ingredient> _ingredients = new List<Ingredient>();
        private Ingredient _ingredient = new Ingredient();
        DBConnection dbConnection = new DBConnection("en");

        public IEnumerable<Ingredient> GetAll()
        {
            _ingredients = dbConnection.GetAllIngredient();
            return _ingredients;
        }

        public Ingredient Get(int id)
        {
            _ingredient = dbConnection.GetIngredientById(id);
            return _ingredient;
        }
    }
}