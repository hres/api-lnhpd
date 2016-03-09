using lnhpd;
using System.Collections.Generic;

namespace lnhpdWebApi.Models
{
    public class ProductLicenceRepository : IProductLicenceRepository
    {
        // We are using the list and _fakeDatabaseID to represent what would
    // most likely be a database of some sort, with an auto-incrementing ID field:
        private List<ProductLicence> _licences = new List<ProductLicence>();
        private ProductLicence _licence = new ProductLicence();
    DBConnection dbConnection = new DBConnection("en");


    public IEnumerable<ProductLicence> GetAll()
    {
        _licences = dbConnection.GetAllProductLicence();

        return _licences;
    }


    public ProductLicence Get(int id)
    {
        _licence = dbConnection.GetProductLicenceById(id);
        return _licence;
    }


    }
}