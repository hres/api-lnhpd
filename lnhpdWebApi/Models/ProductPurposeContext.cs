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
    public class ProductPurposeContext
    {

        private class DBResult
        {
            public int count;
            public List<ProductPurpose> data { get; set; }

            public DBResult(List<ProductPurpose> data, int count)
            {
                this.count = count;
                this.data = data;
            }
        }

        public Response<List<ProductPurpose>> GetProductPurposeById(int id, string lang = "en")
        {
            var countQuery = $"select count(*) {getQueryTable()} where SUBMISSION_ID = :id";
            var query = getQueryColumns(lang) + getQueryTable();
            query = query + $" where SUBMISSION_ID = :id";
            DBResult results = executeMany(countQuery, query, new Dictionary<string, string>() { { ":id", idCal(id).ToString() } }, lang);
            return new Response<List<ProductPurpose>>() { data = results.data };
        }

        public Response<List<ProductPurpose>> GetAllProductPurpose(RequestInfo requestInfo)
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
            var query = "SELECT TEXT_ID, SUBMISSION_ID, PURPOSE_F, PURPOSE_E ";
            return query;
        }

        private string getQueryTable()
        {
            var query = " FROM NHPPLQ_OWNER.PRODUCT_PURPOSE_ONLINE";
            return query;
        }

        private Response<List<ProductPurpose>> executeMany(string countQuery, string query, RequestInfo requestInfo)
        {
            var items = new List<ProductPurpose>();
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
                                items.Add(ProductPurposeFactory(dr, lang));
                            }
                        }
                    }

                    var response = new Response<List<ProductPurpose>> { data = items };

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
            var items = new List<ProductPurpose>();
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
                                items.Add(ProductPurposeFactory(dr, lang));
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

        private ProductPurpose executeOne(string query, string lang)
        {
            ProductPurpose item = null;

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
                            item = ProductPurposeFactory(dr, lang);
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

        private ProductPurpose ProductPurposeFactory(OracleDataReader dr, string lang)
        {
            var item = new ProductPurpose();
            item.text_id = dr["TEXT_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["TEXT_ID"]);
            item.lnhpd_id = dr["SUBMISSION_ID"] == DBNull.Value ? 0 : idLnhpd(Convert.ToInt32(dr["SUBMISSION_ID"]));
            if (lang.Equals("fr"))
            {
                item.purpose = dr["PURPOSE_F"] == DBNull.Value ? dr["PURPOSE_E"].ToString().Trim() : dr["PURPOSE_F"].ToString().Trim();
            }
            else
            {
                item.purpose = dr["PURPOSE_E"] == DBNull.Value ? dr["PURPOSE_F"].ToString().Trim() : dr["PURPOSE_E"].ToString().Trim();
            }

            return item;
        }
    }
}
