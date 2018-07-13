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
using lnhpdWebApi.Models;
using System.Xml.Linq;
using System.Xml;

namespace lnhpdWebApi.Controllers
{
    public class JsonController : ApiController
    {
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage DownloadJson(string dataType, string lang)
        {
            var jsonResult = string.Empty;
            var fileNameDate = string.Format("{0}{1}{2}",
                           DateTime.Now.Year.ToString(),
                           DateTime.Now.Month.ToString().PadLeft(2, '0'),
                           DateTime.Now.Day.ToString().PadLeft(2, '0'));
            var fileName = string.Format(dataType + "_{0}.json", fileNameDate);
            byte[] outputBuffer = null;
            string resultString = string.Empty;
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var json = string.Empty;
            switch (dataType)
            {
                case "MedicinalIngredient":
                    IMedicinalIngredientRepository miPlaceholder = new MedicinalIngredientRepository();
                    var medicinalIngredient = miPlaceholder.GetAllMedicinal(lang);
                    if (medicinalIngredient != null)
                    {
                        json = JsonConvert.SerializeObject(medicinalIngredient);
                    }
                    break;
                case "NonMedicinalIngredient":
                    INonMedicinalIngredientRepository nmiPlaceholder = new NonMedicinalIngredientRepository();
                    var nonMedicinalIngredient = nmiPlaceholder.GetAllNonMedicinal(lang);
                    if (nonMedicinalIngredient != null)
                    { 
                        json = JsonConvert.SerializeObject(nonMedicinalIngredient);
                    }
                    break;
            }


            if (!string.IsNullOrWhiteSpace(json))
            {
                using (var stream = new MemoryStream())
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.WriteLine(json);
                        // writer.Flush();
                        outputBuffer = stream.ToArray();
                        resultString = Encoding.UTF8.GetString(outputBuffer, 0, outputBuffer.Length);
                    }
                }
                result.Content = new StringContent(resultString);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = fileName };
            }
            return result;
        }


    }
}
