using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lnhpdWebApi.Models
{
    interface IIngredientQuantityRepository
    {
        IEnumerable<IngredientQuantity> GetAll();
        IngredientQuantity Get(int id);
    }
}
