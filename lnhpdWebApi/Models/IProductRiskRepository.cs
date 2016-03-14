using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lnhpdWebApi.Models
{
    interface IProductRiskRepository
    {
        IEnumerable<ProductRisk> GetAll();
        ProductRisk Get(int id);
    }
}
