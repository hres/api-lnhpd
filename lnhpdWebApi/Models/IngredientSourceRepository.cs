using lnhpd;
using System.Collections.Generic;

namespace lnhpdWebApi.Models
{
    public class IngredientSourceRepository : IIngredientSourceRepository
    {
        private List<IngredientSource> _sources = new List<IngredientSource>();
        private IngredientSource _source = new IngredientSource();
        DBConnection dbConnection = new DBConnection("en");

        public IEnumerable<IngredientSource> GetAll(string lang)
        {
            _sources = dbConnection.GetAllIngredientSource(lang);
            return _sources;
        }

        public IngredientSource Get(int id, string lang)
        {
            _source = dbConnection.GetIngredientSourceById(id, lang);
            return _source;
        }
    }
}