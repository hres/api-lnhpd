using lnhpd;
using System.Collections.Generic;

namespace lnhpdWebApi.Models
{
    public class IngredientSourceRepository : IIngredientSourceRepository
    {
        private List<IngredientSource> _sources = new List<IngredientSource>();
        private IngredientSource _source = new IngredientSource();
        DBConnection dbConnection = new DBConnection("en");

        public IEnumerable<IngredientSource> GetAll()
        {
            _sources = dbConnection.GetAllIngredientSource();
            return _sources;
        }

        public IngredientSource Get(int id)
        {
            _source = dbConnection.GetIngredientSourceById(id);
            return _source;
        }
    }
}