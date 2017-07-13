using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Threading;
using lnhpdWebApi.Models;
using System.Text;
using System.IO;
using System.Linq;
using System.Data;

namespace lnhpd
{
    /// <summary>
    /// Summary description for Common
    /// </summary>
    public static class UtilityHelper
    {
        public static void SetDefaultCulture(string lang)
        {
            if (lang == "en")
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-CA");
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("en-CA");
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("fr-FR");
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("fr-FR");
            }
        }

        public static List<ProductLicence> GetAllProductByCriteria(string lang, string term)
        {
            var items = new List<ProductLicence>();
            var filteredList = new List<ProductLicence>();
            var json = string.Empty;
            var din = term;
            var brandname = term;
            var ingredient = term;
            var company = term;

            var lnhpdJsonUrl = string.Format("{0}&brandname={1}&ingredient={2}&companyname={3}&din={4}&lang={5}", ConfigurationManager.AppSettings["lnhpdJsonUrl"].ToString(), brandname, ingredient, company, din, lang);

            try
            {
                using (var webClient = new System.Net.WebClient())
                {
                    webClient.Encoding = UTF8Encoding.UTF8;
                    json = webClient.DownloadString(lnhpdJsonUrl);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        items = JsonConvert.DeserializeObject<List<ProductLicence>>(json);

                    }
                }
            }
            catch (Exception ex)
            {
                var errorMessages = string.Format("UtilityHelper - GetJSonDataFromDPDAPI()- Error Message:{0}", ex.Message);
                ExceptionHelper.LogException(ex, errorMessages);
            }
            finally
            {

            }
            return items;
        }
        public static List<ProductLicence> GetDrugProductList(string lang)
        {
            // CertifySSL.EnableTrustedHosts();
            var items = new List<ProductLicence>();
            var filteredList = new List<ProductLicence>();
            var json = string.Empty;

            var lnhpdJsonUrl = string.Format("{0}&lang={1}", ConfigurationManager.AppSettings["lnhpdJsonUrl"].ToString(), lang);
            try
            {
                using (var webClient = new System.Net.WebClient())
                {
                    webClient.Encoding = Encoding.UTF8;
                    json = webClient.DownloadString(lnhpdJsonUrl);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        items = JsonConvert.DeserializeObject<List<ProductLicence>>(json);
                    }
                }
            }
            catch (Exception ex)
            {
                var errorMessages = string.Format("UtilityHelper - GetJSonDataFromRegAPI()- Error Message:{0}", ex.Message);
                ExceptionHelper.LogException(ex, errorMessages);
            }
            finally
            {

            }
            return items;
        }

        public static ProductLicence GetByID(string lnhpdID, string lang)
        {
            // CertifySSL.EnableTrustedHosts();
            var item = new ProductLicence();
            var json = string.Empty;
            var postData = new Dictionary<string, string>();
            var lnhpdJsonUrlbyID = string.Format("{0}&id={1}&lang={2}", ConfigurationManager.AppSettings["lnhpdJsonUrl"].ToString(), lnhpdID, lang);

            try
            {
                using (var webClient = new System.Net.WebClient())
                {
                    webClient.Encoding = Encoding.UTF8;
                    json = webClient.DownloadString(lnhpdJsonUrlbyID);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        item = JsonConvert.DeserializeObject<ProductLicence>(json);
                    }
                }
            }
            catch (Exception ex)
            {
                var errorMessages = string.Format("UtilityHelper - GetDrugProductByID()- Error Message:{0}", ex.Message);
                ExceptionHelper.LogException(ex, errorMessages);
            }
            finally
            {

            }
            return item;
        }

        public static void WriteDataTable(DataTable sourceTable, TextWriter writer, bool includeHeaders)
        {
            if (includeHeaders)
            {
                IEnumerable<String> headerValues = sourceTable.Columns
                    .OfType<DataColumn>()
                    .Select(column => QuoteValue(column.ColumnName));

                writer.WriteLine(String.Join(",", headerValues));
            }

            IEnumerable<String> items = null;

            foreach (DataRow row in sourceTable.Rows)
            {
                items = row.ItemArray.Select(o => QuoteValue(o.ToString()));
                writer.WriteLine(String.Join(",", items));
            }

            writer.Flush();
        }

        private static string QuoteValue(string value)
        {
            return String.Concat("\"",
            value.Replace("\"", "\"\""), "\"");
        }

    }
}