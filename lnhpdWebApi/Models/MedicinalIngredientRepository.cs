using lnhpd;
using System.Collections.Generic;

namespace lnhpdWebApi.Models
{
    public class MedicinalIngredientRepository : IMedicinalIngredientRepository
    {
        private List<MedicinalIngredient> _ingredients = new List<MedicinalIngredient>();
        private MedicinalIngredient _ingredient = new MedicinalIngredient();
        DBConnection dbConnection = new DBConnection("en");

        public IEnumerable<MedicinalIngredient> GetAllMedicinal(string lang)
        {
            _ingredients = dbConnection.GetAllMedicinalIngredient(lang);
            return _ingredients;
        }

        public IEnumerable<MedicinalIngredient> GetMedicinal(int id, string lang)
        {
            //_ingredient = dbConnection.GetIngredientById(id, lang);
            //return _ingredient;
            _ingredients = dbConnection.GetMedicinalIngredientById(id, lang);
            return _ingredients;
        }
        
    }
}