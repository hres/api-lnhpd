using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lnhpdWebApi.Models
{
    interface IIngredientSourceRepository
    {
        IEnumerable<IngredientSource> GetAll(string lang);
        IngredientSource Get(int id, string lang);
    }
}
