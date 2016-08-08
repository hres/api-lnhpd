using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lnhpdWebApi.Models
{
    interface IProductDoseRepository
    {
        IEnumerable<ProductDose> GetAll(string lang);
        ProductDose Get(int id, string lang);
    }
}
