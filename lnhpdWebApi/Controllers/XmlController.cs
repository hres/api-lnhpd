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
using System.Xml.Serialization;

namespace lnhpdWebApi.Controllers
{
    public class XmlController : ApiController
    {
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage DownloadXml(string dataType, string lang)
        {
            var jsonResult = string.Empty;
            var fileNameDate = string.Format("{0}{1}{2}",
                           DateTime.Now.Year.ToString(),
                           DateTime.Now.Month.ToString().PadLeft(2, '0'),
                           DateTime.Now.Day.ToString().PadLeft(2, '0'));
            var fileName = string.Format(dataType + "_{0}.xml", fileNameDate);
            byte[] outputBuffer = null;
            string resultString = string.Empty;
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var xmlString = string.Empty;
            switch (dataType)
            {
                case "MedicinalIngredient":
                    IMedicinalIngredientRepository miPlaceholder = new MedicinalIngredientRepository();
                    var medicinalIngredient = miPlaceholder.GetAllMedicinal(lang);
                    if (medicinalIngredient != null)
                    {
                        xmlString = GetXMLFromObject(medicinalIngredient);
                    }
                    break;
                case "NonMedicinalIngredient":
                    INonMedicinalIngredientRepository nmiPlaceholder = new NonMedicinalIngredientRepository();
                    var nonMedicinalIngredient = nmiPlaceholder.GetAllNonMedicinal(lang);
                    if (nonMedicinalIngredient != null)
                    {
                        xmlString = GetXMLFromObject(nonMedicinalIngredient);
                    }
                    break;
            }
            
            if (!string.IsNullOrWhiteSpace(xmlString))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                using (var stream = new MemoryStream())
                {
                    using (var writer = new XmlTextWriter(stream, System.Text.Encoding.UTF8))
                    {
                        writer.Formatting = System.Xml.Formatting.Indented;
                        writer.Indentation = 4;

                        // Write the XML declaration.
                        doc.WriteTo(writer);
                        //writer.WriteFullEndElement();
                        // writer.Flush();
                        outputBuffer = stream.ToArray();
                        resultString = Encoding.UTF8.GetString(outputBuffer, 0, outputBuffer.Length);
                        writer.Close();
                    }
                }
                result.Content = new StringContent(resultString);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = fileName };
            }          

            return result;
        }

        private string GetXMLFromObject(object o)
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter tw = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(o.GetType());
                tw = new XmlTextWriter(sw);
                serializer.Serialize(tw, o);
            }
            catch (Exception ex)
            {
                //Handle Exception Code
            }
            finally
            {
                sw.Close();
                if (tw != null)
                {
                    tw.Close();
                }
            }
            return sw.ToString();
        }

    }
}
