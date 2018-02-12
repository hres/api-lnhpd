using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lnhpdWebApi.Models
{
    interface IMedicinalIngredientRepository
    {
        IEnumerable<MedicinalIngredient> GetAllMedicinal(string lang);
        IEnumerable<MedicinalIngredient> GetMedicinal(int id, string lang);
        
    }
}
