using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using lnhpdWebApi.Models.Request;
using lnhpdWebApi.Models.Response;
using Oracle.ManagedDataAccess.Client;

namespace lnhpdWebApi.Models
{
    public class MedicinalIngredientContext
    {

        private class DBResult
        {
            public int count;
            public List<MedicinalIngredient> data { get; set; }

            public DBResult(List<MedicinalIngredient> data, int count)
            {
                this.count = count;
                this.data = data;
            }

        }

        public Response<List<MedicinalIngredient>> GetMedicinalIngredientById(int id, string lang = "en")
        {
            var countQuery = $"select count(*) {getQueryTable()} and r.submission_id = :id";
            var query = getQueryColumns(lang) + getQueryTable();
            query = query + $" and i.submission_id = :id";
            DBResult results = executeMany(countQuery, query, new Dictionary<string, string>() { { ":id", idCal(id).ToString() } }, lang);
            return new Response<List<MedicinalIngredient>>() { data = results.data };
        }

        public Response<List<MedicinalIngredient>> GetAllMedicinalIngredient(RequestInfo requestInfo)
        {
            return executeMany(requestInfo);
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
            var query = "select i.submission_id submission_id, i.matrix_id matrix_id, i.matrix_type_code matrix_type_code, ";
            query += " q.potency_amount potency_amount, q.potency_constituent potency_constituent, ";
            query += " q.quantity quantity, q.quantity_minimum quantity_minimum, q.quantity_maximum quantity_maximum, ";
            query += " q.ratio_numerator ratio_numerator, q.ratio_denominator ratio_denominator, q.dried_herb_equivalent dried_herb_equivalent, ";
            if (lang.Equals("fr"))
            {
                query += " i.ingredient_name_other as ingredient_name, q.uom_type_desc_potency_f potency_unit_of_measure,  q.uom_type_desc_amt_quantity_f quantity_unit_of_measure, ";
                query += " q.uom_type_desc_dhe_f dhe_unit_of_measure, q.extract_type_desc_f extract_type_desc, s.material_type_desc_f source_material ";
            }
            else
            {
                query += " i.ingredient_name ingredient_name , q.uom_type_desc_potency potency_unit_of_measure,  q.uom_type_desc_amt_quantity quantity_unit_of_measure, ";
                query += " q.uom_type_desc_dhe dhe_unit_of_measure, q.extract_type_desc extract_type_desc, s.material_type_desc source_material ";
            };

            return query;
        }

        private string getQueryTable()
        {
            var query = " from nhpplq_owner.ingredient_online i ";
            query += " left join nhpplq_owner.ingredient_quantity_online q on i.matrix_id= q.matrix_id";
            query += " left join nhpplq_owner.ingredient_source_online s on i.matrix_id = s.matrix_id";
            query += " where i.matrix_type_code=2 ";
            return query;
        }

        private Response<List<MedicinalIngredient>> executeMany(RequestInfo requestInfo)
        {
            var items = new List<MedicinalIngredient>();
            int count = 0;

            using (OracleConnection con = new OracleConnection(LnhpdDBConnection))
            {
                try
                {
                    con.Open();
                    var countQuery = "select count(*) " + getQueryTable();
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
                    var query = getQueryColumns(lang) + getQueryTable();
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
                                items.Add(MedicinalIngredientFactory(dr));
                            }
                        }
                    }

                    var response = new Response<List<MedicinalIngredient>> { data = items };

                    response.metadata = new Metadata();
                    var pagination = new Pagination();
                    pagination.limit = limit;
                    pagination.page = page;
                    pagination.total = count;
                    pagination.next = getNextPage(requestInfo, limit, page, count);
                    pagination.previous = getPreviousPage(requestInfo, limit, page, count);

                    response.metadata.pagination = pagination;

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

        private string getNextPage(RequestInfo requestInfo, int limit, int page, int count)
        {
            var next = buildBasePath(requestInfo);
            // page indexing starts at 1
            if ((page) * requestInfo.limit >= count) return null;
            next += $"?page={page + 1}&lang={requestInfo.languages}&type={requestInfo.type}";
            return next;
        }

        private string getPreviousPage(RequestInfo requestInfo, int limit, int page, int count)
        {
            var previous = buildBasePath(requestInfo);
            if (page <= 1) return null;
            previous += $"?page={page - 1}&lang={requestInfo.languages}&type={requestInfo.type}";

            return previous;
        }

        private string buildBasePath(RequestInfo requestInfo)
        {
            // DOES NOT WORK WITH URL ALIASES!
            //var request = requestInfo.context.Request;
            //var scheme = request.Url.Scheme;
            //var port = request.Url.Port;
            //var portValue = ((scheme == "http" && port == 80) || (scheme == "https" && port == 443)) ? "" : (":" + port);
            //return $"{scheme}://{request.Url.Host}{portValue}{request.Path}";

            return requestInfo.path;
        }

        private DBResult executeMany(string countQuery, string query, Dictionary<string, string> parameters, string lang)
        {
            var items = new List<MedicinalIngredient>();
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
                                items.Add(MedicinalIngredientFactory(dr));
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

        private MedicinalIngredient executeOne(string query)
        {
            MedicinalIngredient item = null;

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
                            item = MedicinalIngredientFactory(dr);
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

        private MedicinalIngredient MedicinalIngredientFactory(OracleDataReader reader)
        {
            var item = new MedicinalIngredient();
            item.lnhpd_id = reader["SUBMISSION_ID"] == DBNull.Value ? 0 : idLnhpd(Convert.ToInt32(reader["SUBMISSION_ID"]));
            item.ingredient_name = reader["INGREDIENT_NAME"] == DBNull.Value ? string.Empty : reader["INGREDIENT_NAME"].ToString().Trim();
            //item.matrix_id = reader["MATRIX_ID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["MATRIX_ID"]);
            //item.matrix_type_code = reader["MATRIX_TYPE_CODE"] == DBNull.Value ? 0 : Convert.ToInt32(reader["MATRIX_TYPE_CODE"]);
            //item.quantity_list = GetAllIngredientQuantityByMatrixId(item.matrix_id, lang);
            item.potency_amount = reader["potency_amount"] == DBNull.Value ? 0 : Convert.ToDouble(reader["potency_amount"]);
            item.potency_constituent = reader["potency_constituent"] == DBNull.Value ? string.Empty : reader["potency_constituent"].ToString().Trim();
            item.potency_unit_of_measure = reader["potency_unit_of_measure"] == DBNull.Value ? string.Empty : reader["potency_unit_of_measure"].ToString().Trim();
            item.quantity = reader["quantity"] == DBNull.Value ? 0 : Convert.ToDouble(reader["quantity"]);
            item.quantity_minimum = reader["quantity_minimum"] == DBNull.Value ? 0 : Convert.ToInt32(reader["quantity_minimum"]);
            item.quantity_maximum = reader["quantity_maximum"] == DBNull.Value ? 0 : Convert.ToInt32(reader["quantity_maximum"]);
            item.quantity_unit_of_measure = reader["quantity_unit_of_measure"] == DBNull.Value ? string.Empty : reader["quantity_unit_of_measure"].ToString().Trim();
            item.ratio_numerator = reader["ratio_numerator"] == DBNull.Value ? string.Empty : reader["ratio_numerator"].ToString().Trim();
            item.ratio_denominator = reader["ratio_denominator"] == DBNull.Value ? string.Empty : reader["ratio_denominator"].ToString().Trim();
            item.dried_herb_equivalent = reader["dried_herb_equivalent"] == DBNull.Value ? string.Empty : reader["dried_herb_equivalent"].ToString().Trim();
            item.dhe_unit_of_measure = reader["dhe_unit_of_measure"] == DBNull.Value ? string.Empty : reader["dhe_unit_of_measure"].ToString().Trim();
            item.extract_type_desc = reader["extract_type_desc"] == DBNull.Value ? string.Empty : reader["extract_type_desc"].ToString().Trim();
            item.source_material = reader["source_material"] == DBNull.Value ? string.Empty : reader["source_material"].ToString().Trim();
            return item;
        }
    }
}