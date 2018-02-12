using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lnhpdWebApi.Models
{
    interface INonMedicinalIngredientRepository
    {
        IEnumerable<NonMedicinalIngredient> GetAllNonMedicinal(string lang);
        IEnumerable<NonMedicinalIngredient> GetNonMedicinal(int id, string lang);
    }
}
