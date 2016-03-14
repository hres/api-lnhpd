using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lnhpdWebApi.Models
{
    interface IIngredientSubmissionRepository
    {
        IEnumerable<IngredientSubmission> GetAll();
        IngredientSubmission Get(int id);
    }
}
