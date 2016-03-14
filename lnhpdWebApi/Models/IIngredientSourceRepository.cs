using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lnhpdWebApi.Models
{
    interface IIngredientSourceRepository
    {
        IEnumerable<IngredientSource> GetAll();
        IngredientSource Get(int id);
    }
}
