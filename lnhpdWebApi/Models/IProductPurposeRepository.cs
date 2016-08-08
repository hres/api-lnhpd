using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lnhpdWebApi.Models
{
    interface IProductPurposeRepository
    {
        IEnumerable<ProductPurpose> GetAll(string lang);
        ProductPurpose Get(int id, string lang);
    }
}
