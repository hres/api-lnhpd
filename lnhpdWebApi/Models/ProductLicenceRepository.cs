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


    public IEnumerable<ProductLicence> GetAll(string lang)
    {
        _licences = dbConnection.GetAllProductLicence(lang);

        return _licences;
    }
    public IEnumerable<ProductLicence> GetAllProductByCriteria(string brandname, string ingredient, string companyname, string din, string lang)
        {
        //_licences = dbConnection.GetAllProductByCriteria(brandname, ingredient, companyname, din, lang);
        _licences = dbConnection.GetAllProductBySingleTerm(din, lang);

        return _licences;
    }


        public ProductLicence Get(int id, string lang)
    {
        _licence = dbConnection.GetProductLicenceById(id, lang);
        _licence.secondary_brand_name_list = dbConnection.GetSecondaryBrandNameList(id, lang);
        _licence.route_list = dbConnection.GetProductRouteByLicenceNumber(id, lang);
        _licence.dose_list = dbConnection.GetProductDoseByLicenceNumber(id, lang);
        _licence.purpose_list = dbConnection.GetProductPurposeByLicenceNumber(id, lang);
        _licence.risk_list = dbConnection.GetRiskByLicenceNumber(id, lang);
        _licence.medicinal_ingredient_list = dbConnection.GetMedIngredientByLicenceNumber(id, lang);
        _licence.non_medicinal_ingredient_list = dbConnection.GetNonMedIngredientByLicenceNumber(id, lang);

        return _licence;
    }


    }
}