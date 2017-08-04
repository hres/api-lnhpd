<%@ WebHandler Language="C#" Class="dhpr.dhprController" %>

using System;
using System.Web;
using System.Collections.Generic;
using lnhpdWebApi.Models;


namespace dhpr
{
    public class dhprController : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.AppendHeader("Access-Control-Allow-Origin", "*");

            try
            {
                var jsonResult = string.Empty;
                var lang = string.IsNullOrEmpty(context.Request.QueryString.GetLang().Trim()) ? "en" : context.Request.QueryString.GetLang().Trim();
                if (lang == "en")
                {
                    UtilityHelper.SetDefaultCulture("en");
                }
                else
                {
                    UtilityHelper.SetDefaultCulture("fr");
                }

                //Get All the QueryStrings
                var term  = context.Request.QueryString.GetSearchTerm().ToLower().Trim();
                var pType = string.IsNullOrEmpty(context.Request.QueryString.GetProgramType().Trim()) ? programType.lnhpd : (programType)Enum.Parse(typeof(programType), context.Request.QueryString.GetProgramType().Trim());
                var linkId = string.IsNullOrWhiteSpace(context.Request.QueryString.GetLinkID().Trim())? string.Empty: context.Request.QueryString.GetLinkID().Trim();

                if( !string.IsNullOrWhiteSpace(linkId))
                {
                    switch (pType)
                    {
                        case programType.lnhpd:r
                            var lnhpdItem = new ProductLicence();
                            lnhpdItem = UtilityHelper.GetByID(linkId, lang);
                            if(lnhpdItem.submission_id > 0)
                            {
                                jsonResult = JsonHelper.JsonSerializer<ProductLicence>(lnhpdItem);
                                context.Response.Write(jsonResult);
                            }
                            else
                            {
                                context.Response.Write("{\"id\":\"\"}");
                            }
                            break;
                        
                        default:
                            context.Response.Write("{\"id\":\"\"}");
                            break;
                    }
                }
                else if( string.IsNullOrWhiteSpace(term))
                {
                    switch (pType)
                    {
                        case programType.lnhpd:
                            var lnhpdList = new List<ProductLicence>();
                            lnhpdList =  UtilityHelper.GetDrugProductList(lang);
                            if (lnhpdList != null && lnhpdList.Count > 0)
                            {
                                jsonResult = JsonHelper.JsonSerializer<List<ProductLicence>>(lnhpdList);
                                jsonResult = "{\"data\":" + jsonResult + "}";
                                context.Response.Write(jsonResult);
                            }
                            else
                            {
                                context.Response.Write("{\"data\":[]}");
                            }
                           break;
                
                        default:
                            context.Response.Write("{\"data\":[]}");
                            break;
                    }
                }
                else
                {
                    switch (pType)
                    {
                        case programType.lnhpd:
                            var lnhpdList = new List<ProductLicence>();
                            lnhpdList =  UtilityHelper.GetAllProductByCriteria(lang, term);
                            if (lnhpdList != null && lnhpdList.Count > 0)
                            {
                                jsonResult = JsonHelper.JsonSerializer<List<ProductLicence>>(lnhpdList);
                                jsonResult = "{\"data\":" + jsonResult + "}";
                                context.Response.Write(jsonResult);
                            }
                            else
                            {
                                context.Response.Write("{\"data\":[]}");
                            }
                           break;
                
                        default:
                            context.Response.Write("{\"data\":[]}");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.LogException(ex, "dhprController.ashx");
                context.Response.Write("{\"data\":[]}");
            }
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}