using lnhpd;
using System.Collections.Generic;

namespace lnhpdWebApi.Models
{
    public class IngredientRepository : IIngredientRepository
    {
        private List<Ingredient> _ingredients = new List<Ingredient>();
        private Ingredient _ingredient = new Ingredient();
        DBConnection dbConnection = new DBConnection("en");

        public IEnumerable<Ingredient> GetAll(string lang)
        {
            _ingredients = dbConnection.GetAllIngredient(lang);
            return _ingredients;
        }

        public Ingredient Get(int id, string lang)
        {
            _ingredient = dbConnection.GetIngredientById(id, lang);
            return _ingredient;
        }
    }
}