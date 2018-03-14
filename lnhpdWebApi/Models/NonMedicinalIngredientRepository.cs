using lnhpd;
using System.Collections.Generic;

namespace lnhpdWebApi.Models
{
    public class NonMedicinalIngredientRepository : INonMedicinalIngredientRepository
    {
        private List<NonMedicinalIngredient> _ingredients = new List<NonMedicinalIngredient>();
        private NonMedicinalIngredient _ingredient = new NonMedicinalIngredient();
        DBConnection dbConnection = new DBConnection("en");

        
        public IEnumerable<NonMedicinalIngredient> GetAllNonMedicinal(string lang)
        {
            _ingredients = dbConnection.GetAllNonMedicinalIngredient(lang);
            return _ingredients;
        }

        public IEnumerable<NonMedicinalIngredient> GetNonMedicinal(int id, string lang)
        {
            //_ingredient = dbConnection.GetIngredientById(id, lang);
            //return _ingredient;
            _ingredients = dbConnection.GetNonMedicinalIngredientById(id, lang);
            return _ingredients;
        }
    }
}