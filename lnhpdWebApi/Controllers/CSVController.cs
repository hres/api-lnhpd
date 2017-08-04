using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using System.Data;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using lnhpd;
using System.Web.Http;

namespace lnhpdWebApi.Controllers
{
    public class CSVController : ApiController
    {
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage DownloadCSV(string dataType, string lang)
        {
            DBConnection dbConnection = new DBConnection(lang);
            var jsonResult = string.Empty;
            var fileNameDate = string.Format("{0}{1}{2}",
                           DateTime.Now.Year.ToString(),
                           DateTime.Now.Month.ToString().PadLeft(2, '0'),
                           DateTime.Now.Day.ToString().PadLeft(2, '0'));
            var fileName = string.Format(dataType + "_{0}.csv", fileNameDate);
            byte[] outputBuffer = null;
            string resultString = string.Empty;
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

            var json = string.Empty;

            switch (dataType)
            {
                case "ingredient":
                    var ingredient = dbConnection.GetAllIngredient(lang).ToList();
                    if (ingredient.Count > 0)
                    {
                        json = JsonConvert.SerializeObject(ingredient);
                    }
                    break;

                case "ingQuantity":
                    var ingQuantity = dbConnection.GetAllIngredientQuantity(lang).ToList();
                    if (ingQuantity.Count > 0)
                    {
                        json = JsonConvert.SerializeObject(ingQuantity);
                    }
                    break;

                case "ingSource":
                    var ingSource = dbConnection.GetAllIngredientSource(lang).ToList();
                    if (ingSource.Count > 0)
                    {
                        json = JsonConvert.SerializeObject(ingSource);
                    }
                    break;

                case "ingSubmission":
                    var ingSubmission = dbConnection.GetAllIngredientSubmission(lang).ToList();
                    if (ingSubmission.Count > 0)
                    {
                        json = JsonConvert.SerializeObject(ingSubmission);
                    }
                    break;

                case "productDose":
                    var productDose = dbConnection.GetAllProductDose(lang).ToList();
                    if (productDose.Count > 0)
                    {
                        json = JsonConvert.SerializeObject(productDose);
                    }
                    break;

                case "productLicence":
                    var productLicence = dbConnection.GetAllProductLicence(lang).ToList();
                    if (productLicence.Count > 0)
                    {
                        json = JsonConvert.SerializeObject(productLicence);
                    }
                    break;

                case "productPurpose":
                    var productPurpose = dbConnection.GetAllProductPurpose(lang).ToList();
                    if (productPurpose.Count > 0)
                    {
                        json = JsonConvert.SerializeObject(productPurpose);
                    }
                    break;

                case "productRisk":
                    var productRisk = dbConnection.GetAllProductRisk(lang).ToList();
                    if (productRisk.Count > 0)
                    {
                        json = JsonConvert.SerializeObject(productRisk);
                    }
                    break;

                case "productRoute":
                    var productRoute = dbConnection.GetAllProductRoute(lang).ToList();
                    if (productRoute.Count > 0)
                    {
                        json = JsonConvert.SerializeObject(productRoute);
                    }
                    break;
            }

            if (!string.IsNullOrWhiteSpace(json))
            {
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(json);
                if (dt.Rows.Count > 0)
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        using (StreamWriter writer = new StreamWriter(stream))
                        {
                            UtilityHelper.WriteDataTable(dt, writer, true);
                            outputBuffer = stream.ToArray();
                            resultString = Encoding.UTF8.GetString(outputBuffer, 0, outputBuffer.Length);
                        }
                    }
                    result.Content = new StringContent(resultString);
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = fileName };
                }
            }

            return result;
        }
    }
}
