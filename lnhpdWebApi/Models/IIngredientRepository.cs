using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lnhpdWebApi.Models
{
    interface IIngredientRepository
    {
        IEnumerable<Ingredient> GetAll(string lang);
        Ingredient Get(int id, string lang);
    }
}
