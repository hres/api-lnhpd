using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lnhpdWebApi.Models
{
    interface IProductRouteRepository
    {
        IEnumerable<ProductRoute> GetAll(string lang);
        IEnumerable<ProductRoute> Get(int id, string lang);
    }
}
