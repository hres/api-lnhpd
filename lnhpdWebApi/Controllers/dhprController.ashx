﻿<%@ WebHandler Language="C#" Class="dhpr.dhprController" %>

using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using dhpr;


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
                var pType = string.IsNullOrEmpty(context.Request.QueryString.GetProgramType().Trim()) ? programType.dhpr : (programType)Enum.Parse(typeof(programType), context.Request.QueryString.GetProgramType().Trim());
                var linkId = string.IsNullOrWhiteSpace(context.Request.QueryString.GetLinkID().Trim())? string.Empty: context.Request.QueryString.GetLinkID().Trim();

                if( !string.IsNullOrWhiteSpace(linkId))
                {
                    switch (pType)
                    {
                        case programType.rds:
                            var rdsItem = new regulatoryDecisionItem();
                            rdsItem = UtilityHelper.GetRdsByID(linkId, lang);
                            if( !string.IsNullOrWhiteSpace(rdsItem.LinkId))
                            {
                                jsonResult = JsonHelper.JsonSerializer<regulatoryDecisionItem>(rdsItem);
                                context.Response.Write(jsonResult);
                            }
                            else
                            {
                                context.Response.Write("{\"id\":\"\"}");
                            }
                            break;
                        case programType.ssr:
                            var ssrItem = new summarySafetyItem();
                            ssrItem = UtilityHelper.GetSsrByID(linkId, lang);
                            if( ssrItem.ConclusionList == null || ssrItem.ConclusionList.Count == 0)
                            {
                                ssrItem.ConclusionList = new List<BulletPoint>();
                                var temp = new BulletPoint();
                                temp.FieldId = 1;
                                temp.OrderNo = 1;
                                temp.Bullet = "Health Canada1";
                                ssrItem.ConclusionList.Add(temp);
                                var temp2 = new BulletPoint();
                                temp2.FieldId = 1;
                                temp2.OrderNo = 1;
                                temp2.Bullet = "Health Canada2";
                                ssrItem.ConclusionList.Add(temp2);
                                var temp3 = new BulletPoint();
                                temp3.FieldId = 1;
                                temp3.OrderNo = 2;
                                temp3.Bullet = "Health Canada3";
                                ssrItem.ConclusionList.Add(temp3);
                                var temp4 = new BulletPoint();
                                temp4.FieldId = 1;
                                temp4.OrderNo = 2;
                                temp4.Bullet = "Health Canada4";
                                ssrItem.ConclusionList.Add(temp4);
                                var temp5 = new BulletPoint();
                                temp5.FieldId = 1;
                                temp5.OrderNo = 3;
                                temp5.Bullet = "Health Canada5";
                                ssrItem.ConclusionList.Add(temp5);
                                var temp6 = new BulletPoint();
                                temp6.FieldId = 1;
                                temp6.OrderNo = 3;
                                temp6.Bullet = "Health Canada6";
                                ssrItem.ConclusionList.Add(temp6);
                            }
                            //if (ssrItem.ConclusionList.Count > 0)
                            //{
                            //    var cGroupByOrderNo = from g in ssrItem.ConclusionList
                            //                          group g by g.OrderNo into Group1
                            //                          select new { No = Group1.Key, Items = Group1 };
                            //      foreach (var p in cGroupByOrderNo)
                            //      {
                            //            var item = new BulletItem();
                            //            item.title = "Conclusion";
                            //            item.bulletList = new List<string>();
                            //            foreach (var c in p.Items)
                            //            {
                            //                item.title = string.Format("Conclusion {0}",c.OrderNo);
                            //                item.bulletList.Add(c.Bullet);
                            //            }
                            //            ssrItem.Conclusion.Add(item);
                            //     }
                            //}
                            if ( !string.IsNullOrWhiteSpace(ssrItem.LinkId))
                            {
                                jsonResult = JsonHelper.JsonSerializer<summarySafetyItem>(ssrItem);
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
                else
                {
                    switch (pType)
                    {
                        case programType.rds:
                            var rdsList = new List<rdsSearchItem>();
                            rdsList =  UtilityHelper.GetRegulatoryDecisionList(lang, term);
                            if (rdsList != null && rdsList.Count > 0)
                            {
                                rdsList.ForEach(x =>
                                {
                                    x.Outcome = "";
                                }); //will be removed later
                                jsonResult = JsonHelper.JsonSerializer<List<rdsSearchItem>>(rdsList);
                                jsonResult = "{\"data\":" + jsonResult + "}";
                                context.Response.Write(jsonResult);
                            }
                            else
                            {
                                context.Response.Write("{\"data\":[]}");
                            }
                            break;
                        case programType.ssr:
                            var ssrList = new List<ssrSearchItem>();
                            ssrList =  UtilityHelper.GetSummarySafetyList(lang, term);
                            if (ssrList != null && ssrList.Count > 0)
                            {
                                jsonResult = JsonHelper.JsonSerializer<List<ssrSearchItem>>(ssrList);
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