using lnhpd;
using System.Collections.Generic;

namespace lnhpdWebApi.Models
{
    public class IngredientSubmissionRepository : IIngredientSubmissionRepository
    {
        private List<IngredientSubmission> _submissions = new List<IngredientSubmission>();
        private IngredientSubmission _submission = new IngredientSubmission();
        DBConnection dbConnection = new DBConnection("en");

        public IEnumerable<IngredientSubmission> GetAll(string lang)
        {
            _submissions = dbConnection.GetAllIngredientSubmission(lang);
            return _submissions;
        }

        public IngredientSubmission Get(int id, string lang)
        {
            _submission = dbConnection.GetIngredientSubmissionById(id, lang);
            return _submission;
        }
    }
}