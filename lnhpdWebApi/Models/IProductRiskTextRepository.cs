using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lnhpdWebApi.Models
{
    interface IProductRiskTextRepository
    {
        IEnumerable<ProductRiskText> GetAll();
        ProductRiskText Get(int id);
    }
}
