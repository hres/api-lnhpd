using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lnhpdWebApi.Models
{
    interface IIngredientRepository
    {
        IEnumerable<Ingredient> GetAll();
        Ingredient Get(int id);
    }
}
