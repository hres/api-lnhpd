using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using lnhpdWebApi.Models.Request;
using lnhpdWebApi.Models.Response;
using lnhpdWebApi.Utils;
using Oracle.ManagedDataAccess.Client;

namespace lnhpdWebApi.Models
{
    public class ProductRiskContext
    {

        private class DBResult
        {
            public int count;
            public List<ProductRisk> data { get; set; }

            public DBResult(List<ProductRisk> data, int count)
            {
                this.count = count;
                this.data = data;
            }
        }

        public Response<List<ProductRisk>> GetProductRiskById(int id, string lang = "en")
        {
            var countQuery = $"select count(*) {getQueryTable()} and r.submission_id = :id";
            var query = getQueryColumns(lang) + getQueryTable();
            query = query + $" and r.submission_id = :id";
            DBResult results = executeMany(countQuery, query, new Dictionary<string, string>() { { ":id", idCal(id).ToString() } }, lang);
            return new Response<List<ProductRisk>>() { data = results.data };
        }

        public Response<List<ProductRisk>> GetAllProductRisk(RequestInfo requestInfo)
        {
            return executeMany($"select count(*) {getQueryTable()}", $"{getQueryColumns(requestInfo.languages) + getQueryTable()}", requestInfo);

        }

        private string LnhpdDBConnection
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["lnhpd"].ToString();
            }
        }

        public int idCal(int id)
        {
            id = id / 13;
            id = id / 3;
            id = id + 256;
            return id;
        }

        public int idLnhpd(int id)
        {
            id = id - 256;
            id = id * 3;
            id = id * 13;
            return id;
        }

        private string getQueryColumns(string lang)
        {
            var query = "SELECT R.SUBMISSION_ID, R.RISK_ID, R.RISK_TYPE_DESC, R.RISK_TYPE_DESC_F, R.SUB_RISK_TYPE_DESC, R.SUB_RISK_TYPE_DESC_F, T.RISK_TEXT_E, T.RISK_TEXT_F ";
            return query;
        }

        private string getQueryTable()
        {
            var query = " FROM NHPPLQ_OWNER.PRODUCT_RISK_ONLINE R, NHPPLQ_OWNER.PRODUCT_RISK_TEXT_ONLINE T ";
            query += "WHERE R.RISK_ID = T.RISK_ID";
            return query;
        }

        private Response<List<ProductRisk>> executeMany(string countQuery, string query, RequestInfo requestInfo)
        {
            var items = new List<ProductRisk>();
            int count = 0;

            using (OracleConnection con = new OracleConnection(LnhpdDBConnection))
            {
                try
                {
                    con.Open();
                    OracleCommand cmd = new OracleCommand(countQuery, con);
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            count = dr.GetInt32(0);
                        }
                    }

                    var limit = 100;
                    var page = requestInfo.page ?? 1;
                    var start = 0;
                    var stop = 0;

                    start = (page - 1) * (int)limit;

                    // check for invalid page
                    if (start >= count || page < 1)
                    {
                        // throw new Exception();
                    }

                    // shorten the limit if it's the last page
                    if (start + limit > count)
                    {
                        limit = count - start;
                    }

                    stop = start + limit;

                    var lang = requestInfo.languages;
                    query = "select " +
                      (start != 0 ? "sq." : "") + "*" +
                      (start != 0 ? ", rownum as rnum " : " ") +
                      "from (" + query + ") " +
                      (start != 0 ? "sq " : "") + "where rownum <= " + stop;

                    if (start != 0)
                    {
                        query = "select * from (" + query + ") where rnum > " + start;
                    }

                    cmd = new OracleCommand(query, con);
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                items.Add(ProductRiskFactory(dr, lang));
                            }
                        }
                    }

                    var response = new Response<List<ProductRisk>> { data = items };

                    response.metadata = new Metadata();
                    response.metadata.pagination = ResponseHelper.PaginationFactory(requestInfo, limit, page, count);

                    return response;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            return null;
        }

        private DBResult executeMany(string countQuery, string query, Dictionary<string, string> parameters, string lang)
        {
            var items = new List<ProductRisk>();
            int count = 0;

            using (OracleConnection con = new OracleConnection(LnhpdDBConnection))
            {
                try
                {
                    con.Open();
                    OracleCommand cmd = new OracleCommand(query, con);
                    foreach (var parameter in parameters)
                    {
                        cmd.Parameters.Add(parameter.Key, parameter.Value);
                    }
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                items.Add(ProductRiskFactory(dr, lang));
                            }
                        }
                    }

                    cmd = new OracleCommand(countQuery, con);
                    foreach (var parameter in parameters)
                    {
                        cmd.Parameters.Add(parameter.Key, parameter.Value);
                    }
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            count = dr.GetInt32(0);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            return new DBResult(items, count);
        }

        private ProductRisk executeOne(string query, string lang)
        {
            ProductRisk item = null;

            using (OracleConnection con = new OracleConnection(LnhpdDBConnection))
            {
                try
                {
                    con.Open();
                    OracleCommand cmd = new OracleCommand(query, con);
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            item = ProductRiskFactory(dr, lang);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            return item;
        }

        private ProductRisk ProductRiskFactory(OracleDataReader reader, string lang)
        {
            var item = new ProductRisk();
            string risk_text_e = reader["RISK_TEXT_E"] == DBNull.Value ? string.Empty : reader["RISK_TEXT_E"].ToString().Trim();
            string risk_text_f = reader["RISK_TEXT_F"] == DBNull.Value ? string.Empty : reader["RISK_TEXT_F"].ToString().Trim();
            if (risk_text_e.Length > 3999)
            {
                risk_text_e = "N/A";
            }
            item.lnhpd_id = reader["SUBMISSION_ID"] == DBNull.Value ? 0 : idLnhpd(Convert.ToInt32(reader["SUBMISSION_ID"]));
            item.risk_id = reader["RISK_ID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["RISK_ID"]);

            if (lang.Equals("fr"))
            {
                item.risk_type_desc = reader["RISK_TYPE_DESC_F"] == DBNull.Value ? string.Empty : reader["RISK_TYPE_DESC_F"].ToString().Trim();
                item.sub_risk_type_desc = reader["SUB_RISK_TYPE_DESC_F"] == DBNull.Value ? string.Empty : reader["SUB_RISK_TYPE_DESC_F"].ToString().Trim();
                item.risk_text = risk_text_f == "" ? risk_text_e : risk_text_f;

            }
            else
            {
                item.risk_type_desc = reader["RISK_TYPE_DESC"] == DBNull.Value ? string.Empty : reader["RISK_TYPE_DESC"].ToString().Trim();
                item.sub_risk_type_desc = reader["SUB_RISK_TYPE_DESC"] == DBNull.Value ? string.Empty : reader["SUB_RISK_TYPE_DESC"].ToString().Trim();
                item.risk_text = risk_text_e == "N/A" ? risk_text_f : risk_text_e;
            }

            //item.risk_type_desc = reader["RISK_TYPE_DESC"] == DBNull.Value ? string.Empty : reader["RISK_TYPE_DESC"].ToString().Trim();
            //item.sub_risk_type_desc = reader["SUB_RISK_TYPE_DESC"] == DBNull.Value ? string.Empty : reader["SUB_RISK_TYPE_DESC"].ToString().Trim();
            //var riskTextList = GetAllProductRiskTextByRiskId(item.risk_id,lang);
            //if (riskTextList != null && riskTextList.Count > 0)
            //{
            //    item.risk_text_list = riskTextList;
            //}
            //item.risk_text = reader["RISK_TEXT"] == DBNull.Value ? string.Empty : reader["RISK_TEXT"].ToString().Trim();
            return item;
        }
    }
}
