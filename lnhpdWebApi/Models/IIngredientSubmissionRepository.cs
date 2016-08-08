using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lnhpdWebApi.Models
{
    interface IIngredientSubmissionRepository
    {
        IEnumerable<IngredientSubmission> GetAll(string lang);
        IngredientSubmission Get(int id, string lang);
    }
}
