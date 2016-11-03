﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
//using System.Data.SqlClient;
using System.Text;
using System.Configuration;
using lnhpdWebApi.Models;
using System.Data.Odbc;
using Oracle.ManagedDataAccess.Client;
using dhpr;

namespace lnhpd
{

    public class DBConnection
    {

        private string _lang;
        public string Lang
        {
            get { return this._lang; }
            set { this._lang = value; }
        }

        public DBConnection(string lang)
        {
            this._lang = lang;
        }

        private string LnhpdDBConnection
        {
            get { return ConfigurationManager.ConnectionStrings["lnhpd"].ToString(); }
        }

        public List<ProductLicence> GetAllProductLicence(string lang)
        {
            var items = new List<ProductLicence>();
            string commandText = "SELECT FILE_NUMBER, SUBMISSION_ID, LICENCE_NUMBER, LICENCE_DATE, REVISED_DATE, TIME_RECEIPT, DATE_START, NOTES, PRODUCT_NAME_ID, PRODUCT_NAME, COMPANY_ID, COMPANY_NAME_ID, COMPANY_NAME, SUB_SUBMISSION_TYPE_CODE, FLAG_PRIMARY_NAME, FLAG_PRODUCT_STATUS, FLAG_ATTESTED_MONOGRAPH, ";
            if (lang.Equals("fr"))
            {
                commandText += "DOSAGE_FORM_F as DOSAGE_FORM, SUB_SUBMISSION_TYPE_DESC_F as SUB_SUBMISSION_TYPE_DESC ";
            }
            else {
                commandText += "DOSAGE_FORM, SUB_SUBMISSION_TYPE_DESC ";
            }
            commandText += "FROM NHPPLQ_OWNER.PRODUCT_LICENCE_ONLINE";

            using (OracleConnection con = new OracleConnection(LnhpdDBConnection))
            {
                OracleCommand cmd = new OracleCommand(commandText, con);
                try
                {
                    con.Open();
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var item = new ProductLicence();

                                item.file_number = dr["FILE_NUMBER"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FILE_NUMBER"]);
                                item.submission_id = dr["SUBMISSION_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUBMISSION_ID"]);
                                item.licence_number = dr["LICENCE_NUMBER"] == DBNull.Value ? string.Empty : dr["LICENCE_NUMBER"].ToString().Trim();
                                item.licence_date = dr["LICENCE_DATE"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["LICENCE_DATE"]);
                                item.revised_date = dr["REVISED_DATE"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["REVISED_DATE"]);
                                item.time_receipt = dr["TIME_RECEIPT"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["TIME_RECEIPT"]);
                                item.date_start = dr["DATE_START"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["DATE_START"]);
                                item.notes = dr["NOTES"] == DBNull.Value ? string.Empty : dr["NOTES"].ToString().Trim(); ;
                                item.product_name_id = dr["PRODUCT_NAME_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["PRODUCT_NAME_ID"]);
                                item.product_name = dr["PRODUCT_NAME"] == DBNull.Value ? string.Empty : dr["PRODUCT_NAME"].ToString().Trim();
                                item.dosage_form = dr["DOSAGE_FORM"] == DBNull.Value ? string.Empty : dr["DOSAGE_FORM"].ToString().Trim();
                                item.company_id = dr["COMPANY_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["COMPANY_ID"]);
                                item.company_name_id = dr["COMPANY_NAME_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["COMPANY_NAME_ID"]);
                                item.company_name = dr["COMPANY_NAME"] == DBNull.Value ? string.Empty : dr["COMPANY_NAME"].ToString().Trim();
                                item.sub_submission_type_code = dr["SUB_SUBMISSION_TYPE_CODE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUB_SUBMISSION_TYPE_CODE"]);
                                item.sub_submission_type_desc = dr["SUB_SUBMISSION_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["SUB_SUBMISSION_TYPE_DESC"].ToString().Trim();
                                item.flag_primary_name = dr["FLAG_PRIMARY_NAME"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FLAG_PRIMARY_NAME"]);
                                item.flag_product_status = dr["FLAG_PRODUCT_STATUS"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FLAG_PRODUCT_STATUS"]);
                                item.flag_attested_monograph = dr["FLAG_ATTESTED_MONOGRAPH"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FLAG_ATTESTED_MONOGRAPH"]);

                                items.Add(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessages = string.Format("DbConnection.cs - GetAllProductLicence()");
                    ExceptionHelper.LogException(ex, errorMessages);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            return items;
        }


        public List<ProductLicence> GetAllProductByCriteria(string brandname, string ingredient, string companyname, string din, string lang)
        {
            var orderClause = "";
            var items = new List<ProductLicence>();
            string commandText = "SELECT FILE_NUMBER, SUBMISSION_ID, LICENCE_NUMBER, LICENCE_DATE, REVISED_DATE, TIME_RECEIPT, DATE_START, NOTES, PRODUCT_NAME_ID, PRODUCT_NAME, COMPANY_ID, COMPANY_NAME_ID, COMPANY_NAME, SUB_SUBMISSION_TYPE_CODE, FLAG_PRIMARY_NAME, FLAG_PRODUCT_STATUS, FLAG_ATTESTED_MONOGRAPH, ";
            if (lang.Equals("fr"))
            {
                commandText += "DOSAGE_FORM_F as DOSAGE_FORM, SUB_SUBMISSION_TYPE_DESC_F as SUB_SUBMISSION_TYPE_DESC ";
            }
            else {
                commandText += "DOSAGE_FORM, SUB_SUBMISSION_TYPE_DESC ";
            }
            commandText += "FROM NHPPLQ_OWNER.PRODUCT_LICENCE_ONLINE";

            //commandText += " LEFT OUTER JOIN NHPPLQ_OWNER.INGREDIENT_SUBMISSION_ONLINE B ON A.SUBMISSION_ID = B.SUBMISSION_ID";
            commandText += " WHERE (";
            //commandText += " B.SUBMISSION_ID IN (SELECT b.SUBMISSION_ID FROM NHPPLQ_OWNER.INGREDIENT_SUBMISSION_ONLINE B WHERE A.SUBMISSION_ID = B.SUBMISSION_ID) AND ";
            //commandText += " (";



            if (din != null)
            {
                commandText += " UPPER(LICENCE_NUMBER) LIKE '%" + din.ToUpper() + "%'";
            }
            if (brandname != null)
            {
                if (din != null) commandText += " OR";

                commandText += " UPPER(PRODUCT_NAME) LIKE '%" + brandname.ToUpper() + "%'";

            }
            if (ingredient != null)
            {
                //    commandText += " UPPER(DRUG_IDENTIFICATION_NUMBER) LIKE '%" + din.ToUpper() + "%'";
            }
            if (companyname != null)
            {
                if ((din != null) || (brandname != null)) commandText += " OR";
                commandText += " UPPER(COMPANY_NAME) LIKE '%" + companyname.ToUpper() + "%'";
            }
            commandText += ")";
            if (lang.Equals("fr"))
            {
                orderClause += " translate(COMPANY_NAME,'ÀÂÄÇÈÉËÊÌÎÏÒÔÖÙÚÛÜ','AAACEEEEIIIOOOUUUU'), translate(BRAND_NAME_F,'ÀÂÄÇÈÉËÊÌÎÏÒÔÖÙÚÛÜ','AAACEEEEIIIOOOUUUU'),";
            }
            else
            {
                orderClause += " translate(COMPANY_NAME,'ÀÂÄÇÈÉËÊÌÎÏÒÔÖÙÚÛÜ','AAACEEEEIIIOOOUUUU'), translate(BRAND_NAME,'ÀÂÄÇÈÉËÊÌÎÏÒÔÖÙÚÛÜ','AAACEEEEIIIOOOUUUU'),";
            }

            commandText += " ORDER BY LICENCE_NUMBER, PRODUCT_NAME";
            using (OracleConnection con = new OracleConnection(LnhpdDBConnection))
            {
                OracleCommand cmd = new OracleCommand(commandText, con);
                try
                {
                    con.Open();
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var item = new ProductLicence();

                                item.file_number = dr["FILE_NUMBER"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FILE_NUMBER"]);
                                item.submission_id = dr["SUBMISSION_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUBMISSION_ID"]);
                                item.licence_number = dr["LICENCE_NUMBER"] == DBNull.Value ? string.Empty : dr["LICENCE_NUMBER"].ToString().Trim();
                                item.licence_date = dr["LICENCE_DATE"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["LICENCE_DATE"]);
                                item.revised_date = dr["REVISED_DATE"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["REVISED_DATE"]);
                                item.time_receipt = dr["TIME_RECEIPT"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["TIME_RECEIPT"]);
                                item.date_start = dr["DATE_START"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["DATE_START"]);
                                item.notes = dr["NOTES"] == DBNull.Value ? string.Empty : dr["NOTES"].ToString().Trim(); ;
                                item.product_name_id = dr["PRODUCT_NAME_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["PRODUCT_NAME_ID"]);
                                item.product_name = dr["PRODUCT_NAME"] == DBNull.Value ? string.Empty : dr["PRODUCT_NAME"].ToString().Trim();
                                item.dosage_form = dr["DOSAGE_FORM"] == DBNull.Value ? string.Empty : dr["DOSAGE_FORM"].ToString().Trim();
                                item.company_id = dr["COMPANY_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["COMPANY_ID"]);
                                item.company_name_id = dr["COMPANY_NAME_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["COMPANY_NAME_ID"]);
                                item.company_name = dr["COMPANY_NAME"] == DBNull.Value ? string.Empty : dr["COMPANY_NAME"].ToString().Trim();
                                item.sub_submission_type_code = dr["SUB_SUBMISSION_TYPE_CODE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUB_SUBMISSION_TYPE_CODE"]);
                                item.sub_submission_type_desc = dr["SUB_SUBMISSION_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["SUB_SUBMISSION_TYPE_DESC"].ToString().Trim();
                                item.flag_primary_name = dr["FLAG_PRIMARY_NAME"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FLAG_PRIMARY_NAME"]);
                                item.flag_product_status = dr["FLAG_PRODUCT_STATUS"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FLAG_PRODUCT_STATUS"]);
                                item.flag_attested_monograph = dr["FLAG_ATTESTED_MONOGRAPH"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FLAG_ATTESTED_MONOGRAPH"]);

                                items.Add(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessages = string.Format("DbConnection.cs - GetAllProductByCriteria()");
                    ExceptionHelper.LogException(ex, errorMessages);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            return items;
        }

        public List<ProductLicence> GetAllProductBySingleTerm(string term, string lang)
        {
            var items = new List<ProductLicence>();
            var items1 = new List<ProductLicence>();
            var items2 = new List<ProductLicence>();

            string commandText1 = "SELECT DISTINCT SUBMISSION_ID, LICENCE_NUMBER, PRODUCT_NAME, COMPANY_NAME, FLAG_PRIMARY_NAME, FLAG_PRODUCT_STATUS ";
            commandText1 += "FROM NHPPLQ_OWNER.PRODUCT_LICENCE_ONLINE ";
            commandText1 += "WHERE (";
            if (term != null)
            {
                commandText1 += "UPPER(LICENCE_NUMBER) LIKE '%" + term.ToUpper() + "%' OR ";
                commandText1 += "UPPER(PRODUCT_NAME) LIKE '%" + term.ToUpper() + "%' OR ";
                commandText1 += "UPPER(COMPANY_NAME) LIKE '%" + term.ToUpper() + "%'";
            }
            commandText1 += ")";

            commandText1 += " ORDER BY LICENCE_NUMBER, PRODUCT_NAME";

            string commandText2 = "SELECT DISTINCT SUBMISSION_ID, LICENCE_NUMBER, PRODUCT_NAME, COMPANY_NAME, FLAG_PRIMARY_NAME, FLAG_PRODUCT_STATUS ";
            commandText2 += "FROM NHPPLQ_OWNER.PRODUCT_LICENCE_ONLINE ";
            commandText2 += "WHERE SUBMISSION_ID IN ( ";
            commandText2 += "SELECT SUBMISSION_ID ";
            commandText2 += "FROM NHPPLQ_OWNER.INGREDIENT_SUBMISSION_ONLINE ";
            commandText2 += "WHERE ";
            if (term != null)
            {
                commandText2 += "(UPPER(NAME_UPPER) LIKE '%" + term.ToUpper() + "%') AND (INGREDIENT_TYPE_CODE = 2)";
            }
            commandText2 += ") ";
            commandText2 += "AND FLAG_PRIMARY_NAME = 1 ";
            commandText2 += "ORDER BY LICENCE_NUMBER, PRODUCT_NAME";

            using (OracleConnection con = new OracleConnection(LnhpdDBConnection))
            {
                OracleCommand cmd1 = new OracleCommand(commandText1, con);
                OracleCommand cmd2 = new OracleCommand(commandText2, con);
                try
                {
                    con.Open();
                    using (OracleDataReader dr = cmd1.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var item1 = new ProductLicence();

                                item1.submission_id = dr["SUBMISSION_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUBMISSION_ID"]);
                                item1.licence_number = dr["LICENCE_NUMBER"] == DBNull.Value ? string.Empty : dr["LICENCE_NUMBER"].ToString().Trim();
                                item1.product_name = dr["PRODUCT_NAME"] == DBNull.Value ? string.Empty : dr["PRODUCT_NAME"].ToString().Trim();
                                //item1.secondary_brand_name_list = GetSecondaryBrandNameList(Convert.ToInt32(item1.licence_number), lang);
                                item1.company_name = dr["COMPANY_NAME"] == DBNull.Value ? string.Empty : dr["COMPANY_NAME"].ToString().Trim();
                                item1.flag_primary_name = dr["FLAG_PRIMARY_NAME"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FLAG_PRIMARY_NAME"]);
                                item1.flag_product_status = dr["FLAG_PRODUCT_STATUS"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FLAG_PRODUCT_STATUS"]);

                                items1.Add(item1);
                            }
                        }
                    }

                    using (OracleDataReader dr = cmd2.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var item2 = new ProductLicence();

                                item2.submission_id = dr["SUBMISSION_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUBMISSION_ID"]);
                                item2.licence_number = dr["LICENCE_NUMBER"] == DBNull.Value ? string.Empty : dr["LICENCE_NUMBER"].ToString().Trim();
                                item2.product_name = dr["PRODUCT_NAME"] == DBNull.Value ? string.Empty : dr["PRODUCT_NAME"].ToString().Trim();
                                //item2.secondary_brand_name_list = GetSecondaryBrandNameList(Convert.ToInt32(item2.LicenceNumber), lang);
                                item2.flag_primary_name = dr["FLAG_PRIMARY_NAME"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FLAG_PRIMARY_NAME"]);
                                item2.flag_product_status = dr["FLAG_PRODUCT_STATUS"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FLAG_PRODUCT_STATUS"]);

                                items2.Add(item2);
                            }
                        }
                    }

                    if (items2 != null && items2.Count > 0)
                    {
                        var mergedList = items1.Union(items2, new ProductComparer());
                        items = mergedList.ToList();
                    }
                    else
                    {
                        items = items1;
                    }
                }
                catch (Exception ex)
                {
                    string errorMessages = string.Format("DbConnection.cs - GetAllProductByCriteria()");
                    ExceptionHelper.LogException(ex, errorMessages);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            return items;
        }

        public ProductLicence GetProductLicenceById(int id, string lang)
        {
            var licence = new ProductLicence();
            string commandText = "SELECT FILE_NUMBER, SUBMISSION_ID, LICENCE_NUMBER, LICENCE_DATE, REVISED_DATE, TIME_RECEIPT, DATE_START, NOTES, PRODUCT_NAME_ID, PRODUCT_NAME, COMPANY_ID, COMPANY_NAME_ID, COMPANY_NAME, SUB_SUBMISSION_TYPE_CODE, FLAG_PRIMARY_NAME, FLAG_PRODUCT_STATUS, FLAG_ATTESTED_MONOGRAPH, ";
            if (lang.Equals("fr"))
            {
                commandText += "DOSAGE_FORM_F as DOSAGE_FORM, SUB_SUBMISSION_TYPE_DESC_F as SUB_SUBMISSION_TYPE_DESC ";
            }
            else {
                commandText += "DOSAGE_FORM, SUB_SUBMISSION_TYPE_DESC ";
            }
            commandText += "FROM NHPPLQ_OWNER.PRODUCT_LICENCE_ONLINE WHERE FLAG_PRIMARY_NAME = 1 AND LICENCE_NUMBER = " + id;


            using (

                OracleConnection con = new OracleConnection(LnhpdDBConnection))
            {
                OracleCommand cmd = new OracleCommand(commandText, con);
                try
                {
                    con.Open();
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var item = new ProductLicence();

                                item.file_number = dr["FILE_NUMBER"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FILE_NUMBER"]);
                                item.submission_id = dr["SUBMISSION_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUBMISSION_ID"]);
                                item.licence_number = dr["LICENCE_NUMBER"] == DBNull.Value ? string.Empty : dr["LICENCE_NUMBER"].ToString().Trim();
                                item.licence_date = dr["LICENCE_DATE"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["LICENCE_DATE"]);
                                item.revised_date = dr["REVISED_DATE"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["REVISED_DATE"]);
                                item.time_receipt = dr["TIME_RECEIPT"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["TIME_RECEIPT"]);
                                item.date_start = dr["DATE_START"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["DATE_START"]);
                                item.notes = dr["NOTES"] == DBNull.Value ? string.Empty : dr["NOTES"].ToString().Trim(); ;
                                item.product_name_id = dr["PRODUCT_NAME_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["PRODUCT_NAME_ID"]);
                                item.product_name = dr["PRODUCT_NAME"] == DBNull.Value ? string.Empty : dr["PRODUCT_NAME"].ToString().Trim();
                                item.dosage_form = dr["DOSAGE_FORM"] == DBNull.Value ? string.Empty : dr["DOSAGE_FORM"].ToString().Trim();
                                item.company_id = dr["COMPANY_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["COMPANY_ID"]);
                                item.company_name_id = dr["COMPANY_NAME_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["COMPANY_NAME_ID"]);
                                item.company_name = dr["COMPANY_NAME"] == DBNull.Value ? string.Empty : dr["COMPANY_NAME"].ToString().Trim();
                                item.sub_submission_type_code = dr["SUB_SUBMISSION_TYPE_CODE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUB_SUBMISSION_TYPE_CODE"]);
                                item.sub_submission_type_desc = dr["SUB_SUBMISSION_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["SUB_SUBMISSION_TYPE_DESC"].ToString().Trim();
                                item.flag_primary_name = dr["FLAG_PRIMARY_NAME"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FLAG_PRIMARY_NAME"]);
                                item.flag_product_status = dr["FLAG_PRODUCT_STATUS"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FLAG_PRODUCT_STATUS"]);
                                item.flag_attested_monograph = dr["FLAG_ATTESTED_MONOGRAPH"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FLAG_ATTESTED_MONOGRAPH"]);

                                licence = item;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessages = string.Format("DbConnection.cs - GetProductLicenceById()");
                    ExceptionHelper.LogException(ex, errorMessages);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            return licence;
        }

        public List<Ingredient> GetAllIngredient(string lang)
        {
            var items = new List<Ingredient>();
            string commandText = "SELECT SUBMISSION_ID, MATRIX_ID, MATRIX_TYPE_CODE, ";
            if (lang.Equals("fr"))
            {
                commandText += "INGREDIENT_NAME_OTHER as INGREDIENT_NAME ";
            }
            else {
                commandText += "INGREDIENT_NAME ";
            }
            commandText += "FROM NHPPLQ_OWNER.INGREDIENT_ONLINE";

            using (OracleConnection con = new OracleConnection(LnhpdDBConnection))
            {
                OracleCommand cmd = new OracleCommand(commandText, con);
                try
                {
                    con.Open();
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var item = new Ingredient();

                                item.submission_id = dr["SUBMISSION_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUBMISSION_ID"]);
                                item.ingredient_name = dr["INGREDIENT_NAME"] == DBNull.Value ? string.Empty : dr["INGREDIENT_NAME"].ToString().Trim();
                                item.matrix_id = dr["MATRIX_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["MATRIX_ID"]);
                                item.matrix_type_code = dr["MATRIX_TYPE_CODE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["MATRIX_TYPE_CODE"]);
                                item.quantity_list = GetAllIngredientQuantityByMatrixId(item.matrix_id, lang);

                                items.Add(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessages = string.Format("DbConnection.cs - GetAllIngredient()");
                    ExceptionHelper.LogException(ex, errorMessages);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            return items;
        }

        public Ingredient GetIngredientById(int id, string lang)
        {
            var ingredient = new Ingredient();
            string commandText = "SELECT SUBMISSION_ID, MATRIX_ID, MATRIX_TYPE_CODE, ";
            if (lang.Equals("fr"))
            {
                commandText += "INGREDIENT_NAME_OTHER as INGREDIENT_NAME ";
            }
            else {
                commandText += "INGREDIENT_NAME ";
            }
            commandText += "FROM NHPPLQ_OWNER.INGREDIENT_ONLINE WHERE SUBMISSION_ID = " + id;


            using (

                OracleConnection con = new OracleConnection(LnhpdDBConnection))
            {
                OracleCommand cmd = new OracleCommand(commandText, con);
                try
                {
                    con.Open();
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var item = new Ingredient();

                                item.submission_id = dr["SUBMISSION_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUBMISSION_ID"]);
                                item.ingredient_name = dr["INGREDIENT_NAME"] == DBNull.Value ? string.Empty : dr["INGREDIENT_NAME"].ToString().Trim();
                                item.matrix_id = dr["MATRIX_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["MATRIX_ID"]);
                                item.matrix_type_code = dr["MATRIX_TYPE_CODE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["MATRIX_TYPE_CODE"]);
                                item.quantity_list = GetAllIngredientQuantityByMatrixId(item.matrix_id, lang);

                                ingredient = item;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessages = string.Format("DbConnection.cs - GetIngredientById()");
                    ExceptionHelper.LogException(ex, errorMessages);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            return ingredient;
        }

        public List<Ingredient> GetMedIngredientByLicenceNumber(int licenceNumber, string lang)
        {
            var items = new List<Ingredient>();
            string commandText = "SELECT DISTINCT I.SUBMISSION_ID, I.MATRIX_ID, I.MATRIX_TYPE_CODE, ";
            if (lang.Equals("fr"))
            {
                commandText += "I.INGREDIENT_NAME_OTHER as I.INGREDIENT_NAME ";
            }
            else {
                commandText += "I.INGREDIENT_NAME ";
            }
            commandText += "FROM NHPPLQ_OWNER.INGREDIENT_ONLINE I, NHPPLQ_OWNER.PRODUCT_LICENCE_ONLINE L WHERE L.SUBMISSION_ID = I.SUBMISSION_ID AND I.MATRIX_TYPE_CODE = 2 AND L.LICENCE_NUMBER = " + licenceNumber;
            commandText += " ORDER BY I.INGREDIENT_NAME ASC";


            using (

                OracleConnection con = new OracleConnection(LnhpdDBConnection))
            {
                OracleCommand cmd = new OracleCommand(commandText, con);
                try
                {
                    con.Open();
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var item = new Ingredient();

                                item.submission_id = dr["SUBMISSION_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUBMISSION_ID"]);
                                item.ingredient_name = dr["INGREDIENT_NAME"] == DBNull.Value ? string.Empty : dr["INGREDIENT_NAME"].ToString().Trim();
                                item.matrix_id = dr["MATRIX_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["MATRIX_ID"]);
                                item.matrix_type_code = dr["MATRIX_TYPE_CODE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["MATRIX_TYPE_CODE"]);
                                item.quantity_list = GetAllIngredientQuantityByMatrixId(item.matrix_id, lang);

                                items.Add(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessages = string.Format("DbConnection.cs - GetMedIngredientByLicenceNumber()");
                    ExceptionHelper.LogException(ex, errorMessages);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            return items;
        }

        public List<Ingredient> GetNonMedIngredientByLicenceNumber(int licenceNumber, string lang)
        {
            var items = new List<Ingredient>();
            string commandText = "SELECT DISTINCT I.SUBMISSION_ID, I.MATRIX_ID, I.MATRIX_TYPE_CODE, ";
            if (lang.Equals("fr"))
            {
                commandText += "I.INGREDIENT_NAME_OTHER as I.INGREDIENT_NAME ";
            }
            else {
                commandText += "I.INGREDIENT_NAME ";
            }
            commandText += "FROM NHPPLQ_OWNER.INGREDIENT_ONLINE I, NHPPLQ_OWNER.PRODUCT_LICENCE_ONLINE L WHERE L.SUBMISSION_ID = I.SUBMISSION_ID AND I.MATRIX_TYPE_CODE = 3 AND L.LICENCE_NUMBER = " + licenceNumber;
            commandText += " ORDER BY I.INGREDIENT_NAME ASC";


            using (

                OracleConnection con = new OracleConnection(LnhpdDBConnection))
            {
                OracleCommand cmd = new OracleCommand(commandText, con);
                try
                {
                    con.Open();
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var item = new Ingredient();

                                item.submission_id = dr["SUBMISSION_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUBMISSION_ID"]);
                                item.ingredient_name = dr["INGREDIENT_NAME"] == DBNull.Value ? string.Empty : dr["INGREDIENT_NAME"].ToString().Trim();
                                item.matrix_id = dr["MATRIX_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["MATRIX_ID"]);
                                item.matrix_type_code = dr["MATRIX_TYPE_CODE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["MATRIX_TYPE_CODE"]);

                                items.Add(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessages = string.Format("DbConnection.cs - GetNonMedIngredientByLicenceNumber()");
                    ExceptionHelper.LogException(ex, errorMessages);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            return items;
        }

        public List<IngredientQuantity> GetAllIngredientQuantity(string lang)
        {
            var items = new List<IngredientQuantity>();
            string commandText = "SELECT INGREDIENT_AMOUNT_ID, MATRIX_ID, QUANTITY, QUANTITY_MINIMUM, QUANTITY_MAXIMUM, RATIO_NUMERATOR, RATIO_DENOMINATOR, DRIED_HERB_EQUIVALENT, POTENCY_AMOUNT, POTENCY_CONSTITUENT, ";
            if (lang.Equals("fr"))
            {
                commandText += "UOM_TYPE_DESC_AMT_QUANTITY_F as UOM_TYPE_DESC_AMT_QUANTITY, UOM_TYPE_DESC_DHE_F as UOM_TYPE_DESC_DHE, EXTRACT_TYPE_DESC_F as EXTRACT_TYPE_DESC, UOM_TYPE_DESC_POTENCY_F as UOM_TYPE_DESC_POTENCY ";
            }
            else {
                commandText += "UOM_TYPE_DESC_AMT_QUANTITY, UOM_TYPE_DESC_DHE, EXTRACT_TYPE_DESC, UOM_TYPE_DESC_POTENCY ";
            }
            commandText += "FROM NHPPLQ_OWNER.INGREDIENT_QUANTITY_ONLINE";


            using (OracleConnection con = new OracleConnection(LnhpdDBConnection))
            {
                OracleCommand cmd = new OracleCommand(commandText, con);
                try
                {
                    con.Open();
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var item = new IngredientQuantity();

                                item.ingredient_amount_id = dr["INGREDIENT_AMOUNT_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["INGREDIENT_AMOUNT_ID"]);
                                item.matrix_id = dr["MATRIX_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["MATRIX_ID"]);
                                item.quantity = dr["QUANTITY"] == DBNull.Value ? 0 : Convert.ToDouble(dr["QUANTITY"]);
                                item.quantity_minimum = dr["QUANTITY_MINIMUM"] == DBNull.Value ? 0 : Convert.ToDouble(dr["QUANTITY_MINIMUM"]);
                                item.quantity_maximum = dr["QUANTITY_MAXIMUM"] == DBNull.Value ? 0 : Convert.ToDouble(dr["QUANTITY_MAXIMUM"]);
                                item.quantity_uom_type_desc = dr["UOM_TYPE_DESC_AMT_QUANTITY"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_AMT_QUANTITY"].ToString().Trim();
                                item.ratio_numerator = dr["RATIO_NUMERATOR"] == DBNull.Value ? string.Empty : dr["RATIO_NUMERATOR"].ToString().Trim();
                                item.ratio_denominator = dr["RATIO_DENOMINATOR"] == DBNull.Value ? string.Empty : dr["RATIO_DENOMINATOR"].ToString().Trim();
                                item.dried_herb_equivalent = dr["DRIED_HERB_EQUIVALENT"] == DBNull.Value ? string.Empty : dr["DRIED_HERB_EQUIVALENT"].ToString().Trim();
                                item.dried_herb_equivalent_uom_type_desc = dr["UOM_TYPE_DESC_DHE"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_DHE"].ToString().Trim();
                                item.extract_type_desc = dr["EXTRACT_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["EXTRACT_TYPE_DESC"].ToString().Trim();
                                item.potency = dr["POTENCY_AMOUNT"] == DBNull.Value ? 0 : Convert.ToDouble(dr["POTENCY_AMOUNT"]);
                                item.potency_uom_type_desc = dr["UOM_TYPE_DESC_POTENCY"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_POTENCY"].ToString().Trim();
                                item.potency_constituent = dr["POTENCY_CONSTITUENT"] == DBNull.Value ? string.Empty : dr["POTENCY_CONSTITUENT"].ToString().Trim();

                                items.Add(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessages = string.Format("DbConnection.cs - GetAllIngredientQuantity()");
                    ExceptionHelper.LogException(ex, errorMessages);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            return items;
        }

        public List<IngredientQuantity> GetAllIngredientQuantityByMatrixId(int matrix_id, string lang)
        {
            var items = new List<IngredientQuantity>();
            var newItems = new List<IngredientQuantity>(); // MassagedItems contains merged Potencies with the same ingredient names

            string commandText = "SELECT I.INGREDIENT_NAME, Q.INGREDIENT_AMOUNT_ID, Q.MATRIX_ID, Q.QUANTITY, Q.QUANTITY_MINIMUM, Q.QUANTITY_MAXIMUM, Q.RATIO_NUMERATOR, Q.RATIO_DENOMINATOR, Q.DRIED_HERB_EQUIVALENT, Q.POTENCY_AMOUNT, Q.POTENCY_CONSTITUENT, ";
            if (lang.Equals("fr"))
            {
                commandText += "Q.UOM_TYPE_DESC_AMT_QUANTITY_F as Q.UOM_TYPE_DESC_AMT_QUANTITY, Q.UOM_TYPE_DESC_DHE_F as Q.UOM_TYPE_DESC_DHE, Q.EXTRACT_TYPE_DESC_F as Q.EXTRACT_TYPE_DESC, Q.UOM_TYPE_DESC_POTENCY_F as Q.UOM_TYPE_DESC_POTENCY ";
            }
            else {
                commandText += "Q.UOM_TYPE_DESC_AMT_QUANTITY, Q.UOM_TYPE_DESC_DHE, Q.EXTRACT_TYPE_DESC, Q.UOM_TYPE_DESC_POTENCY ";
            }
            commandText += "FROM NHPPLQ_OWNER.INGREDIENT_QUANTITY_ONLINE Q, NHPPLQ_OWNER.INGREDIENT_ONLINE I WHERE I.MATRIX_ID = Q.MATRIX_ID AND Q.MATRIX_ID = " + matrix_id;
            commandText += " ORDER BY UPPER(I.INGREDIENT_NAME), Q.QUANTITY, Q.RATIO_NUMERATOR ASC";


            using (OracleConnection con = new OracleConnection(LnhpdDBConnection))
            {
                OracleCommand cmd = new OracleCommand(commandText, con);
                try
                {
                    con.Open();
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var item = new IngredientQuantity();
                                var quantityString = "";
                                var extractString = "";
                                var potencyString = "";

                                item.ingredient_amount_id = dr["INGREDIENT_AMOUNT_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["INGREDIENT_AMOUNT_ID"]);
                                item.ingredient_name = dr["INGREDIENT_NAME"] == DBNull.Value ? string.Empty : dr["INGREDIENT_NAME"].ToString().Trim();
                                item.matrix_id = dr["MATRIX_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["MATRIX_ID"]);
                                item.quantity = dr["QUANTITY"] == DBNull.Value ? 0 : Convert.ToDouble(dr["QUANTITY"]);
                                item.quantity_minimum = dr["QUANTITY_MINIMUM"] == DBNull.Value ? 0 : Convert.ToDouble(dr["QUANTITY_MINIMUM"]);
                                item.quantity_maximum = dr["QUANTITY_MAXIMUM"] == DBNull.Value ? 0 : Convert.ToDouble(dr["QUANTITY_MAXIMUM"]);
                                item.quantity_uom_type_desc = dr["UOM_TYPE_DESC_AMT_QUANTITY"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_AMT_QUANTITY"].ToString().Trim();
                                item.ratio_numerator = dr["RATIO_NUMERATOR"] == DBNull.Value ? string.Empty : dr["RATIO_NUMERATOR"].ToString().Trim();
                                item.ratio_denominator = dr["RATIO_DENOMINATOR"] == DBNull.Value ? string.Empty : dr["RATIO_DENOMINATOR"].ToString().Trim();
                                item.dried_herb_equivalent = dr["DRIED_HERB_EQUIVALENT"] == DBNull.Value ? string.Empty : dr["DRIED_HERB_EQUIVALENT"].ToString().Trim();
                                item.dried_herb_equivalent_uom_type_desc = dr["UOM_TYPE_DESC_DHE"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_DHE"].ToString().Trim();
                                item.extract_type_desc = dr["EXTRACT_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["EXTRACT_TYPE_DESC"].ToString().Trim();
                                item.potency = dr["POTENCY_AMOUNT"] == DBNull.Value ? 0 : Convert.ToDouble(dr["POTENCY_AMOUNT"]);
                                item.potency_uom_type_desc = dr["UOM_TYPE_DESC_POTENCY"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_POTENCY"].ToString().Trim();
                                item.potency_constituent = dr["POTENCY_CONSTITUENT"] == DBNull.Value ? string.Empty : dr["POTENCY_CONSTITUENT"].ToString().Trim();

                                // Construct full Quantity String
                                if (item.quantity != 0)
                                {
                                    if ((item.quantity % 1) == 0)
                                    {
                                        quantityString += item.quantity + ".0&nbsp;" + item.quantity_uom_type_desc + "&nbsp;";
                                    }
                                    else {
                                        quantityString += item.quantity + "&nbsp;" + item.quantity_uom_type_desc + "&nbsp;";
                                    }
                                }
                                if (item.quantity_minimum != 0 && item.quantity_maximum != 0)
                                {
                                    quantityString += "Min:&nbsp;" + item.quantity_minimum + "&nbsp;" + item.quantity_uom_type_desc;
                                    quantityString += "<br />Max:&nbsp;" + item.quantity_maximum + "&nbsp;" + item.quantity_uom_type_desc + "&nbsp;";
                                }
                                else {
                                    if (item.quantity_minimum != 0)
                                    {
                                        quantityString += "Min:&nbsp;" + item.quantity_minimum + "&nbsp;" + item.quantity_uom_type_desc;
                                    }
                                    if (item.quantity_maximum != 0)
                                    {
                                        quantityString += "<br />Max:&nbsp;" + item.quantity_maximum + "&nbsp;" + item.quantity_uom_type_desc;
                                    }
                                }

                                item.quantity_string = quantityString;

                                // Construct full Extract String
                                if (item.ratio_numerator != null)
                                {
                                    extractString += item.ratio_numerator + " : ";
                                    if (item.ratio_denominator != null)
                                    {
                                        extractString += item.ratio_denominator + "<br />";
                                    }
                                }
                                if (item.dried_herb_equivalent != null)
                                {
                                    extractString += "DHE:&nbsp;" + item.dried_herb_equivalent;
                                }
                                if (item.dried_herb_equivalent_uom_type_desc != null)
                                {
                                    extractString += "&nbsp;" + item.dried_herb_equivalent_uom_type_desc + "&nbsp;";
                                }
                                if (item.extract_type_desc != null)
                                {
                                    extractString += "<br />" + item.extract_type_desc;
                                }

                                item.extract_string = extractString;

                                // Construct full Potency String
                                if (item.potency != 0)
                                {
                                    if ((item.potency % 1) == 0)
                                    {
                                        potencyString += item.potency + ".0";
                                    }
                                    else {
                                        potencyString += item.potency;
                                    }

                                if (item.potency_uom_type_desc != null)
                                {
                                    potencyString += "&nbsp;" + item.potency_uom_type_desc;
                                }
                                if (item.potency_constituent != null)
                                {
                                    potencyString += "<br />" + item.potency_constituent;
                                }
                                }

                                item.potency_string = potencyString;

                                items.Add(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessages = string.Format("DbConnection.cs - GetAllIngredientQuantity()");
                    ExceptionHelper.LogException(ex, errorMessages);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }

            return this.mergeMedIngredientRow(items, newItems);
        }

        public List<IngredientQuantity> mergeMedIngredientRow(List<IngredientQuantity> medInglist, List<IngredientQuantity> newMedInglist)
        {
            if (medInglist == null || medInglist.Count == 0)
            {
                return newMedInglist;
            }

            if (newMedInglist == null || newMedInglist.Count == 0)
            {
                newMedInglist.Add(medInglist[0]);
                medInglist.Remove(medInglist[0]);
                return mergeMedIngredientRow(medInglist, newMedInglist);
            }

            IngredientQuantity iq = new IngredientQuantity();

            if (medInglist[0].ingredient_name == newMedInglist[newMedInglist.Count - 1].ingredient_name)
            {
                if (medInglist[0].quantity > 0)
                {
                    // Error: second Qunatity exists.
                }
                if (medInglist[0].ratio_numerator != null)
                {
                    // Error: second Extract exsits.
                }
                if (medInglist[0].potency_string != "")
                {
                    newMedInglist[newMedInglist.Count - 1].potency_string += "<br />" + medInglist[0].potency_string;
                }

                medInglist.Remove(medInglist[0]);
            }
            else
            {
                newMedInglist.Add(medInglist[0]);
                medInglist.Remove(medInglist[0]);
            }
            if (medInglist.Count > 0)
            {
                return mergeMedIngredientRow(medInglist, newMedInglist);
            }
            else
            {
                return newMedInglist;
            }
        }

        public IngredientQuantity GetIngredientQuantityById(int id, string lang)
        {
            var quantity = new IngredientQuantity();
            string commandText = "SELECT INGREDIENT_AMOUNT_ID, MATRIX_ID, QUANTITY, QUANTITY_MINIMUM, QUANTITY_MAXIMUM, RATIO_NUMERATOR, RATIO_DENOMINATOR, DRIED_HERB_EQUIVALENT, POTENCY_AMOUNT, POTENCY_CONSTITUENT, ";
            if (lang.Equals("fr"))
            {
                commandText += "UOM_TYPE_DESC_AMT_QUANTITY_F as UOM_TYPE_DESC_AMT_QUANTITY, UOM_TYPE_DESC_DHE_F as UOM_TYPE_DESC_DHE, EXTRACT_TYPE_DESC_F as EXTRACT_TYPE_DESC, UOM_TYPE_DESC_POTENCY_F as UOM_TYPE_DESC_POTENCY ";
            }
            else {
                commandText += "UOM_TYPE_DESC_AMT_QUANTITY, UOM_TYPE_DESC_DHE, EXTRACT_TYPE_DESC, UOM_TYPE_DESC_POTENCY ";
            }
            commandText += "FROM NHPPLQ_OWNER.INGREDIENT_QUANTITY_ONLINE WHERE INGREDIENT_AMOUNT_ID = " + id;


            using (

                OracleConnection con = new OracleConnection(LnhpdDBConnection))
            {
                OracleCommand cmd = new OracleCommand(commandText, con);
                try
                {
                    con.Open();
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var item = new IngredientQuantity();

                                item.ingredient_amount_id = dr["INGREDIENT_AMOUNT_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["INGREDIENT_AMOUNT_ID"]);
                                item.matrix_id = dr["MATRIX_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["MATRIX_ID"]);
                                item.quantity = dr["QUANTITY"] == DBNull.Value ? 0 : Convert.ToDouble(dr["QUANTITY"]);
                                item.quantity_minimum = dr["QUANTITY_MINIMUM"] == DBNull.Value ? 0 : Convert.ToDouble(dr["QUANTITY_MINIMUM"]);
                                item.quantity_maximum = dr["QUANTITY_MAXIMUM"] == DBNull.Value ? 0 : Convert.ToDouble(dr["QUANTITY_MAXIMUM"]);
                                item.quantity_uom_type_desc = dr["UOM_TYPE_DESC_AMT_QUANTITY"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_AMT_QUANTITY"].ToString().Trim();
                                item.ratio_numerator = dr["RATIO_NUMERATOR"] == DBNull.Value ? string.Empty : dr["RATIO_NUMERATOR"].ToString().Trim();
                                item.ratio_denominator = dr["RATIO_DENOMINATOR"] == DBNull.Value ? string.Empty : dr["RATIO_DENOMINATOR"].ToString().Trim();
                                item.dried_herb_equivalent = dr["DRIED_HERB_EQUIVALENT"] == DBNull.Value ? string.Empty : dr["DRIED_HERB_EQUIVALENT"].ToString().Trim();
                                item.dried_herb_equivalent_uom_type_desc = dr["UOM_TYPE_DESC_DHE"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_DHE"].ToString().Trim();
                                item.extract_type_desc = dr["EXTRACT_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["EXTRACT_TYPE_DESC"].ToString().Trim();
                                item.potency = dr["POTENCY_AMOUNT"] == DBNull.Value ? 0 : Convert.ToDouble(dr["POTENCY_AMOUNT"]);
                                item.potency_uom_type_desc = dr["UOM_TYPE_DESC_POTENCY"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_POTENCY"].ToString().Trim();
                                item.potency_constituent = dr["POTENCY_CONSTITUENT"] == DBNull.Value ? string.Empty : dr["POTENCY_CONSTITUENT"].ToString().Trim();

                                quantity = item;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessages = string.Format("DbConnection.cs - GetIngredientQuantityById()");
                    ExceptionHelper.LogException(ex, errorMessages);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            return quantity;
        }

        public List<IngredientSource> GetAllIngredientSource(string lang)
        {
            var items = new List<IngredientSource>();
            string commandText = "SELECT MATERIAL_ID, MATRIX_ID, ";
            if (lang.Equals("fr"))
            {
                commandText += "MATERIAL_TYPE_DESC_F as MATERIAL_TYPE_DESC ";
            }
            else {
                commandText += "MATERIAL_TYPE_DESC ";
            }
            commandText += "FROM NHPPLQ_OWNER.INGREDIENT_SOURCE_ONLINE";

            using (OracleConnection con = new OracleConnection(LnhpdDBConnection))
            {
                OracleCommand cmd = new OracleCommand(commandText, con);
                try
                {
                    con.Open();
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var item = new IngredientSource();

                                item.material_id = dr["MATERIAL_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["MATERIAL_ID"]);
                                item.matrix_id = dr["MATRIX_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["MATRIX_ID"]);
                                item.material_type_desc = dr["MATERIAL_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["MATERIAL_TYPE_DESC"].ToString().Trim();

                                items.Add(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessages = string.Format("DbConnection.cs - GetAllIngredientSource()");
                    ExceptionHelper.LogException(ex, errorMessages);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            return items;
        }

        public IngredientSource GetIngredientSourceById(int id, string lang)
        {
            var source = new IngredientSource();
            string commandText = "SELECT MATERIAL_ID, MATRIX_ID, ";
            if (lang.Equals("fr"))
            {
                commandText += "MATERIAL_TYPE_DESC_F as MATERIAL_TYPE_DESC ";
            }
            else {
                commandText += "MATERIAL_TYPE_DESC ";
            }
            commandText += "FROM NHPPLQ_OWNER.INGREDIENT_SOURCE_ONLINE WHERE MATERIAL_ID = " + id;


            using (

                OracleConnection con = new OracleConnection(LnhpdDBConnection))
            {
                OracleCommand cmd = new OracleCommand(commandText, con);
                try
                {
                    con.Open();
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var item = new IngredientSource();

                                item.material_id = dr["MATERIAL_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["MATERIAL_ID"]);
                                item.matrix_id = dr["MATRIX_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["MATRIX_ID"]);
                                item.material_type_desc = dr["MATERIAL_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["MATERIAL_TYPE_DESC"].ToString().Trim();

                                source = item;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessages = string.Format("DbConnection.cs - GetIngredientSourceById()");
                    ExceptionHelper.LogException(ex, errorMessages);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            return source;
        }

        public List<IngredientSubmission> GetAllIngredientSubmission(string lang)
        {
            var items = new List<IngredientSubmission>();
            string commandText = "SELECT MATRIX_ID, SUBMISSION_ID, INGREDIENT_TYPE_CODE, ";
            if (lang.Equals("fr"))
            {
                commandText += "NAME_UPPER_F as NAME_UPPER ";
            }
            else {
                commandText += "NAME_UPPER ";
            }
            commandText += "FROM NHPPLQ_OWNER.INGREDIENT_SUBMISSION_ONLINE";

            using (OracleConnection con = new OracleConnection(LnhpdDBConnection))
            {
                OracleCommand cmd = new OracleCommand(commandText, con);
                try
                {
                    con.Open();
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var item = new IngredientSubmission();

                                item.matrix_id = dr["MATRIX_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["MATRIX_ID"]);
                                item.submission_id = dr["SUBMISSION_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUBMISSION_ID"]);
                                item.ingredient_type_code = dr["INGREDIENT_TYPE_CODE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["INGREDIENT_TYPE_CODE"]);
                                item.brand_name = dr["NAME_UPPER"] == DBNull.Value ? string.Empty : dr["NAME_UPPER"].ToString().Trim();

                                items.Add(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessages = string.Format("DbConnection.cs - GetAllIngredientSubmission()");
                    ExceptionHelper.LogException(ex, errorMessages);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            return items;
        }

        public IngredientSubmission GetIngredientSubmissionById(int id, string lang)
        {
            var submission = new IngredientSubmission();
            string commandText = "SELECT MATRIX_ID, SUBMISSION_ID, INGREDIENT_TYPE_CODE, ";
            if (lang.Equals("fr"))
            {
                commandText += "NAME_UPPER_F as NAME_UPPER ";
            }
            else {
                commandText += "NAME_UPPER ";
            }
            commandText += "FROM NHPPLQ_OWNER.INGREDIENT_SUBMISSION_ONLINE WHERE SUBMISSION_ID = " + id;


            using (

                OracleConnection con = new OracleConnection(LnhpdDBConnection))
            {
                OracleCommand cmd = new OracleCommand(commandText, con);
                try
                {
                    con.Open();
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var item = new IngredientSubmission();

                                item.matrix_id = dr["MATRIX_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["MATRIX_ID"]);
                                item.submission_id = dr["SUBMISSION_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUBMISSION_ID"]);
                                item.ingredient_type_code = dr["INGREDIENT_TYPE_CODE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["INGREDIENT_TYPE_CODE"]);
                                item.brand_name = dr["NAME_UPPER"] == DBNull.Value ? string.Empty : dr["NAME_UPPER"].ToString().Trim();

                                submission = item;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessages = string.Format("DbConnection.cs - GetIngredientSubmissionById()");
                    ExceptionHelper.LogException(ex, errorMessages);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            return submission;
        }

        public List<ProductDose> GetAllProductDose(string lang)
        {
            var items = new List<ProductDose>();
            string commandText = "SELECT SUBMISSION_ID, DOSE_ID, AGE, AGE_MINIMUM, AGE_MAXIMUM, QUANTITY_DOSE, QUANTITY_MINIMUM_DOSE, QUANTITY_MAXIMUM_DOSE, FREQUENCY, FREQUENCY_MINIMUM, FREQUENCY_MAXIMUM, ";
            if (lang.Equals("fr"))
            {
                commandText += "POPULATION_TYPE_DESC_F as POPULATION_TYPE_DESC, UOM_TYPE_DESC_AGE_F as UOM_TYPE_DESC_AGE, UOM_TYPE_DESC_QUANTITY_DOSE_F as UOM_TYPE_DESC_QUANTITY_DOSE, UOM_TYPE_DESC_FREQUENCY_F as UOM_TYPE_DESC_FREQUENCY ";
            }
            else {
                commandText += "POPULATION_TYPE_DESC, UOM_TYPE_DESC_AGE, UOM_TYPE_DESC_QUANTITY_DOSE, UOM_TYPE_DESC_FREQUENCY ";
            }
            commandText += "FROM NHPPLQ_OWNER.PRODUCT_DOSE_ONLINE";

            using (OracleConnection con = new OracleConnection(LnhpdDBConnection))
            {
                OracleCommand cmd = new OracleCommand(commandText, con);
                try
                {
                    con.Open();
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var item = new ProductDose();

                                item.submission_id = dr["SUBMISSION_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUBMISSION_ID"]);
                                item.dose_id = dr["DOSE_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["DOSE_ID"]);
                                item.population_type_desc = dr["POPULATION_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["POPULATION_TYPE_DESC"].ToString().Trim();
                                item.age = dr["AGE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AGE"]);
                                item.age_minimum = dr["AGE_MINIMUM"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AGE_MINIMUM"]);
                                item.age_maximum = dr["AGE_MAXIMUM"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AGE_MAXIMUM"]);
                                item.age_uom_type_desc = dr["UOM_TYPE_DESC_AGE"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_AGE"].ToString().Trim();
                                item.quantity_dose = dr["QUANTITY_DOSE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["QUANTITY_DOSE"]);
                                item.quantity_dose_minimum = dr["QUANTITY_MINIMUM_DOSE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["QUANTITY_MINIMUM_DOSE"]);
                                item.quantity_dose_maximum = dr["QUANTITY_MAXIMUM_DOSE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["QUANTITY_MAXIMUM_DOSE"]);
                                item.quantity_dose_uom_type_desc = dr["UOM_TYPE_DESC_QUANTITY_DOSE"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_QUANTITY_DOSE"].ToString().Trim();
                                item.frequency = dr["FREQUENCY"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FREQUENCY"]);
                                item.frequency_minimum = dr["FREQUENCY_MINIMUM"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FREQUENCY_MINIMUM"]);
                                item.frequency_maximum = dr["FREQUENCY_MAXIMUM"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FREQUENCY_MAXIMUM"]);
                                item.frequency_uom_type_desc = dr["UOM_TYPE_DESC_FREQUENCY"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_FREQUENCY"].ToString().Trim();

                                items.Add(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessages = string.Format("DbConnection.cs - GetAllProductDose()");
                    ExceptionHelper.LogException(ex, errorMessages);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            return items;
        }

        public ProductDose GetProductDoseById(int id, string lang)
        {
            var dose = new ProductDose();
            string commandText = "SELECT SUBMISSION_ID, DOSE_ID, AGE, AGE_MINIMUM, AGE_MAXIMUM, QUANTITY_DOSE, QUANTITY_MINIMUM_DOSE, QUANTITY_MAXIMUM_DOSE, FREQUENCY, FREQUENCY_MINIMUM, FREQUENCY_MAXIMUM, ";
            if (lang.Equals("fr"))
            {
                commandText += "POPULATION_TYPE_DESC_F as POPULATION_TYPE_DESC, UOM_TYPE_DESC_AGE_F as UOM_TYPE_DESC_AGE, UOM_TYPE_DESC_QUANTITY_DOSE_F as UOM_TYPE_DESC_QUANTITY_DOSE, UOM_TYPE_DESC_FREQUENCY_F as UOM_TYPE_DESC_FREQUENCY ";
            }
            else {
                commandText += "POPULATION_TYPE_DESC, UOM_TYPE_DESC_AGE, UOM_TYPE_DESC_QUANTITY_DOSE, UOM_TYPE_DESC_FREQUENCY ";
            }
            commandText += "FROM NHPPLQ_OWNER.PRODUCT_DOSE_ONLINE WHERE DOSE_ID = " + id;


            using (

                OracleConnection con = new OracleConnection(LnhpdDBConnection))
            {
                OracleCommand cmd = new OracleCommand(commandText, con);
                try
                {
                    con.Open();
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var item = new ProductDose();

                                item.submission_id = dr["SUBMISSION_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUBMISSION_ID"]);
                                item.dose_id = dr["DOSE_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["DOSE_ID"]);
                                item.population_type_desc = dr["POPULATION_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["POPULATION_TYPE_DESC"].ToString().Trim();
                                item.age = dr["AGE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AGE"]);
                                item.age_minimum = dr["AGE_MINIMUM"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AGE_MINIMUM"]);
                                item.age_maximum = dr["AGE_MAXIMUM"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AGE_MAXIMUM"]);
                                item.age_uom_type_desc = dr["UOM_TYPE_DESC_AGE"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_AGE"].ToString().Trim();
                                item.quantity_dose = dr["QUANTITY_DOSE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["QUANTITY_DOSE"]);
                                item.quantity_dose_minimum = dr["QUANTITY_MINIMUM_DOSE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["QUANTITY_MINIMUM_DOSE"]);
                                item.quantity_dose_maximum = dr["QUANTITY_MAXIMUM_DOSE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["QUANTITY_MAXIMUM_DOSE"]);
                                item.quantity_dose_uom_type_desc = dr["UOM_TYPE_DESC_QUANTITY_DOSE"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_QUANTITY_DOSE"].ToString().Trim();
                                item.frequency = dr["FREQUENCY"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FREQUENCY"]);
                                item.frequency_minimum = dr["FREQUENCY_MINIMUM"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FREQUENCY_MINIMUM"]);
                                item.frequency_maximum = dr["FREQUENCY_MAXIMUM"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FREQUENCY_MAXIMUM"]);
                                item.frequency_uom_type_desc = dr["UOM_TYPE_DESC_FREQUENCY"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_FREQUENCY"].ToString().Trim();

                                dose = item;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessages = string.Format("DbConnection.cs - GetProductDoseById()");
                    ExceptionHelper.LogException(ex, errorMessages);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            return dose;
        }

        public List<ProductDose> GetProductDoseByLicenceNumber(int licenceNumber, string lang)
        {
            var items = new List<ProductDose>();
            string commandText = "SELECT DISTINCT D.SUBMISSION_ID, D.DOSE_ID, D.AGE, D.AGE_MINIMUM, D.AGE_MAXIMUM, D.QUANTITY_DOSE, D.QUANTITY_MINIMUM_DOSE, D.QUANTITY_MAXIMUM_DOSE, D.FREQUENCY, D.FREQUENCY_MINIMUM, D.FREQUENCY_MAXIMUM, ";
            if (lang.Equals("fr"))
            {
                commandText += "D.POPULATION_TYPE_DESC_F as D.POPULATION_TYPE_DESC, D.UOM_TYPE_DESC_AGE_F as D.UOM_TYPE_DESC_AGE, D.UOM_TYPE_DESC_QUANTITY_DOSE_F as D.UOM_TYPE_DESC_QUANTITY_DOSE, D.UOM_TYPE_DESC_FREQUENCY_F as D.UOM_TYPE_DESC_FREQUENCY ";
            }
            else {
                commandText += "D.POPULATION_TYPE_DESC, D.UOM_TYPE_DESC_AGE, D.UOM_TYPE_DESC_QUANTITY_DOSE, D.UOM_TYPE_DESC_FREQUENCY ";
            }
            commandText += "FROM NHPPLQ_OWNER.PRODUCT_LICENCE_ONLINE L, NHPPLQ_OWNER.PRODUCT_DOSE_ONLINE D WHERE L.SUBMISSION_ID = D.SUBMISSION_ID AND L.LICENCE_NUMBER = " + licenceNumber;


            using (

                OracleConnection con = new OracleConnection(LnhpdDBConnection))
            {
                OracleCommand cmd = new OracleCommand(commandText, con);
                try
                {
                    con.Open();
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var item = new ProductDose();

                                item.submission_id = dr["SUBMISSION_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUBMISSION_ID"]);
                                item.dose_id = dr["DOSE_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["DOSE_ID"]);
                                item.population_type_desc = dr["POPULATION_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["POPULATION_TYPE_DESC"].ToString().Trim();
                                item.age = dr["AGE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AGE"]);
                                item.age_minimum = dr["AGE_MINIMUM"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AGE_MINIMUM"]);
                                item.age_maximum = dr["AGE_MAXIMUM"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AGE_MAXIMUM"]);
                                item.age_uom_type_desc = dr["UOM_TYPE_DESC_AGE"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_AGE"].ToString().Trim();
                                item.quantity_dose = dr["QUANTITY_DOSE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["QUANTITY_DOSE"]);
                                item.quantity_dose_minimum = dr["QUANTITY_MINIMUM_DOSE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["QUANTITY_MINIMUM_DOSE"]);
                                item.quantity_dose_maximum = dr["QUANTITY_MAXIMUM_DOSE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["QUANTITY_MAXIMUM_DOSE"]);
                                item.quantity_dose_uom_type_desc = dr["UOM_TYPE_DESC_QUANTITY_DOSE"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_QUANTITY_DOSE"].ToString().Trim();
                                item.frequency = dr["FREQUENCY"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FREQUENCY"]);
                                item.frequency_minimum = dr["FREQUENCY_MINIMUM"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FREQUENCY_MINIMUM"]);
                                item.frequency_maximum = dr["FREQUENCY_MAXIMUM"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FREQUENCY_MAXIMUM"]);
                                item.frequency_uom_type_desc = dr["UOM_TYPE_DESC_FREQUENCY"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_FREQUENCY"].ToString().Trim();

                                items.Add(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessages = string.Format("DbConnection.cs - GetProductDoseByLicenceNumber()");
                    ExceptionHelper.LogException(ex, errorMessages);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            return items;
        }

        public List<ProductPurpose> GetAllProductPurpose(string lang)
        {
            var items = new List<ProductPurpose>();
            string commandText = "SELECT TEXT_ID, SUBMISSION_ID, ";
            if (lang.Equals("fr"))
            {
                commandText += "PURPOSE_F as PURPOSE_E ";
            }
            else {
                commandText += "PURPOSE_E ";
            }
            commandText += "FROM NHPPLQ_OWNER.PRODUCT_PURPOSE_ONLINE";

            using (OracleConnection con = new OracleConnection(LnhpdDBConnection))
            {
                OracleCommand cmd = new OracleCommand(commandText, con);
                try
                {
                    con.Open();
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var item = new ProductPurpose();

                                item.text_id = dr["TEXT_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["TEXT_ID"]);
                                item.submission_id = dr["SUBMISSION_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUBMISSION_ID"]);
                                item.purpose = dr["PURPOSE_E"] == DBNull.Value ? string.Empty : dr["PURPOSE_E"].ToString().Trim();

                                items.Add(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessages = string.Format("DbConnection.cs - GetAllProductPurpose()");
                    ExceptionHelper.LogException(ex, errorMessages);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            return items;
        }

        public ProductPurpose GetProductPurposeById(int id, string lang)
        {
            var purpose = new ProductPurpose();
            string commandText = "SELECT TEXT_ID, SUBMISSION_ID, ";
            if (lang.Equals("fr"))
            {
                commandText += "PURPOSE_F as PURPOSE_E ";
            }
            else {
                commandText += "PURPOSE_E ";
            }
            commandText += "FROM NHPPLQ_OWNER.PRODUCT_PURPOSE_ONLINE WHERE SUBMISSION_ID = " + id;


            using (

                OracleConnection con = new OracleConnection(LnhpdDBConnection))
            {
                OracleCommand cmd = new OracleCommand(commandText, con);
                try
                {
                    con.Open();
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var item = new ProductPurpose();

                                item.text_id = dr["TEXT_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["TEXT_ID"]);
                                item.submission_id = dr["SUBMISSION_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUBMISSION_ID"]);
                                item.purpose = dr["PURPOSE_E"] == DBNull.Value ? string.Empty : dr["PURPOSE_E"].ToString().Trim();

                                purpose = item;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessages = string.Format("DbConnection.cs - GetProductPurposeById()");
                    ExceptionHelper.LogException(ex, errorMessages);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            return purpose;
        }

        public List<ProductPurpose> GetProductPurposeByLicenceNumber(int licenceNumber, string lang)
        {
            var items = new List<ProductPurpose>();
            string commandText = "SELECT DISTINCT P.TEXT_ID, P.SUBMISSION_ID, ";
            if (lang.Equals("fr"))
            {
                commandText += "P.PURPOSE_F as P.PURPOSE_E ";
            }
            else {
                commandText += "P.PURPOSE_E ";
            }
            commandText += "FROM NHPPLQ_OWNER.PRODUCT_LICENCE_ONLINE L, NHPPLQ_OWNER.PRODUCT_PURPOSE_ONLINE P WHERE L.SUBMISSION_ID = P.SUBMISSION_ID AND L.LICENCE_NUMBER = " + licenceNumber;


            using (

                OracleConnection con = new OracleConnection(LnhpdDBConnection))
            {
                OracleCommand cmd = new OracleCommand(commandText, con);
                try
                {
                    con.Open();
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var item = new ProductPurpose();

                                item.text_id = dr["TEXT_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["TEXT_ID"]);
                                item.submission_id = dr["SUBMISSION_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUBMISSION_ID"]);
                                item.purpose = dr["PURPOSE_E"] == DBNull.Value ? string.Empty : dr["PURPOSE_E"].ToString().Trim();

                                items.Add(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessages = string.Format("DbConnection.cs - GetProductPurposeByLicenceNumber()");
                    ExceptionHelper.LogException(ex, errorMessages);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            return items;
        }

        public List<ProductRisk> GetAllProductRisk(string lang)
        {
            var items = new List<ProductRisk>();
            string commandText = "SELECT SUBMISSION_ID, RISK_ID, ";
            if (lang.Equals("fr"))
            {
                commandText += "RISK_TYPE_DESC_F as RISK_TYPE_DESC, SUB_RISK_TYPE_DESC_F as SUB_RISK_TYPE_DESC ";
            }
            else {
                commandText += "RISK_TYPE_DESC, SUB_RISK_TYPE_DESC ";
            }
            commandText += "FROM NHPPLQ_OWNER.PRODUCT_RISK_ONLINE";

            using (OracleConnection con = new OracleConnection(LnhpdDBConnection))
            {
                OracleCommand cmd = new OracleCommand(commandText, con);
                try
                {
                    con.Open();
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var item = new ProductRisk();

                                item.submission_id = dr["SUBMISSION_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUBMISSION_ID"]);
                                item.risk_id = dr["RISK_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RISK_ID"]);
                                item.risk_type_desc = dr["RISK_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["RISK_TYPE_DESC"].ToString().Trim();
                                item.sub_risk_type_desc = dr["SUB_RISK_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["SUB_RISK_TYPE_DESC"].ToString().Trim();

                                items.Add(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessages = string.Format("DbConnection.cs - GetAllProductRisk()");
                    ExceptionHelper.LogException(ex, errorMessages);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            return items;
        }

        public ProductRisk GetProductRiskById(int id, string lang)
        {
            var risk = new ProductRisk();
            string commandText = "SELECT SUBMISSION_ID, RISK_ID, ";
            if (lang.Equals("fr"))
            {
                commandText += "RISK_TYPE_DESC_F as RISK_TYPE_DESC, SUB_RISK_TYPE_DESC_F as SUB_RISK_TYPE_DESC ";
            }
            else {
                commandText += "RISK_TYPE_DESC, SUB_RISK_TYPE_DESC ";
            }
            commandText += "FROM NHPPLQ_OWNER.PRODUCT_RISK_ONLINE WHERE RISK_ID = " + id;


            using (

                OracleConnection con = new OracleConnection(LnhpdDBConnection))
            {
                OracleCommand cmd = new OracleCommand(commandText, con);
                try
                {
                    con.Open();
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var item = new ProductRisk();

                                item.submission_id = dr["SUBMISSION_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUBMISSION_ID"]);
                                item.risk_id = dr["RISK_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RISK_ID"]);
                                item.risk_type_desc = dr["RISK_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["RISK_TYPE_DESC"].ToString().Trim();
                                item.sub_risk_type_desc = dr["SUB_RISK_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["SUB_RISK_TYPE_DESC"].ToString().Trim();

                                risk = item;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessages = string.Format("DbConnection.cs - GetProductRiskById()");
                    ExceptionHelper.LogException(ex, errorMessages);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            return risk;
        }

        public List<ProductRisk> GetRiskByLicenceNumber(int licenceNumber, string lang)
        {
            var items = new List<ProductRisk>();
            string commandText = "SELECT DISTINCT R.SUBMISSION_ID, R.RISK_ID, ";
            if (lang.Equals("fr"))
            {
                commandText += "R.RISK_TYPE_DESC_F as R.RISK_TYPE_DESC, R.SUB_RISK_TYPE_DESC_F as R.SUB_RISK_TYPE_DESC ";
            }
            else {
                commandText += "R.RISK_TYPE_DESC, R.SUB_RISK_TYPE_DESC ";
            }
            commandText += "FROM NHPPLQ_OWNER.PRODUCT_LICENCE_ONLINE P, NHPPLQ_OWNER.PRODUCT_RISK_ONLINE R WHERE P.SUBMISSION_ID = R.SUBMISSION_ID AND P.LICENCE_NUMBER = " + licenceNumber;


            using (

                OracleConnection con = new OracleConnection(LnhpdDBConnection))
            {
                OracleCommand cmd = new OracleCommand(commandText, con);
                try
                {
                    con.Open();
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var item = new ProductRisk();

                                item.submission_id = dr["SUBMISSION_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUBMISSION_ID"]);
                                item.risk_id = dr["RISK_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RISK_ID"]);
                                item.risk_type_desc = dr["RISK_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["RISK_TYPE_DESC"].ToString().Trim();
                                item.sub_risk_type_desc = dr["SUB_RISK_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["SUB_RISK_TYPE_DESC"].ToString().Trim();
                                item.risk_text_list = GetAllProductRiskTextByRiskId(item.risk_id, lang);

                                items.Add(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessages = string.Format("DbConnection.cs - GetRiskByLicenceNumber()");
                    ExceptionHelper.LogException(ex, errorMessages);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            return items;
        }

        public List<ProductRiskText> GetAllProductRiskText(string lang)
        {
            var items = new List<ProductRiskText>();
            string commandText = "SELECT TEXT_ID, RISK_ID, ";
            if (lang.Equals("fr"))
            {
                commandText += "RISK_TEXT_F as RISK_TEXT_E ";
            }
            else {
                commandText += "RISK_TEXT_E ";
            }
            commandText += "FROM NHPPLQ_OWNER.PRODUCT_RISK_TEXT_ONLINE";

            using (OracleConnection con = new OracleConnection(LnhpdDBConnection))
            {
                OracleCommand cmd = new OracleCommand(commandText, con);
                try
                {
                    con.Open();
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var item = new ProductRiskText();

                                item.text_id = dr["TEXT_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["TEXT_ID"]);
                                item.risk_id = dr["RISK_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RISK_ID"]);
                                item.risk_text = dr["RISK_TEXT_E"] == DBNull.Value ? string.Empty : dr["RISK_TEXT_E"].ToString().Trim();

                                items.Add(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessages = string.Format("DbConnection.cs - GetAllProductRiskText()");
                    ExceptionHelper.LogException(ex, errorMessages);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            return items;
        }

        public List<ProductRiskText> GetAllProductRiskTextByRiskId(int riskId, string lang)
        {
            var items = new List<ProductRiskText>();
            string commandText = "SELECT TEXT_ID, RISK_ID, ";
            if (lang.Equals("fr"))
            {
                commandText += "RISK_TEXT_F as RISK_TEXT_E ";
            }
            else {
                commandText += "RISK_TEXT_E ";
            }
            commandText += "FROM NHPPLQ_OWNER.PRODUCT_RISK_TEXT_ONLINE WHERE RISK_ID = " + riskId;

            using (OracleConnection con = new OracleConnection(LnhpdDBConnection))
            {
                OracleCommand cmd = new OracleCommand(commandText, con);
                try
                {
                    con.Open();
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var item = new ProductRiskText();

                                item.text_id = dr["TEXT_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["TEXT_ID"]);
                                item.risk_id = dr["RISK_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RISK_ID"]);
                                item.risk_text = dr["RISK_TEXT_E"] == DBNull.Value ? string.Empty : dr["RISK_TEXT_E"].ToString().Trim();

                                items.Add(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessages = string.Format("DbConnection.cs - GetAllProductRiskTextByRiskId()");
                    ExceptionHelper.LogException(ex, errorMessages);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            return items;
        }

        public ProductRiskText GetProductRiskTextById(int id, string lang)
        {
            var riskText = new ProductRiskText();
            string commandText = "SELECT TEXT_ID, RISK_ID, ";
            if (lang.Equals("fr"))
            {
                commandText += "RISK_TEXT_F as RISK_TEXT_E ";
            }
            else {
                commandText += "RISK_TEXT_E ";
            }
            commandText += "FROM NHPPLQ_OWNER.PRODUCT_RISK_TEXT_ONLINE WHERE TEXT_ID = " + id;


            using (

                OracleConnection con = new OracleConnection(LnhpdDBConnection))
            {
                OracleCommand cmd = new OracleCommand(commandText, con);
                try
                {
                    con.Open();
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var item = new ProductRiskText();

                                item.text_id = dr["TEXT_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["TEXT_ID"]);
                                item.risk_id = dr["RISK_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RISK_ID"]);
                                item.risk_text = dr["RISK_TEXT_E"] == DBNull.Value ? string.Empty : dr["RISK_TEXT_E"].ToString().Trim();

                                riskText = item;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessages = string.Format("DbConnection.cs - GetProductRiskTextById()");
                    ExceptionHelper.LogException(ex, errorMessages);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            return riskText;
        }

        public List<ProductRoute> GetAllProductRoute(string lang)
        {
            var items = new List<ProductRoute>();
            string commandText = "SELECT SUBMISSION_ID, ROUTE_ID, ";
            if (lang.Equals("fr"))
            {
                commandText += "ROUTE_TYPE_DESC_F as ROUTE_TYPE_DESC ";
            }
            else {
                commandText += "ROUTE_TYPE_DESC ";
            }
            commandText += "FROM NHPPLQ_OWNER.PRODUCT_ROUTE_ONLINE";

            using (OracleConnection con = new OracleConnection(LnhpdDBConnection))
            {
                OracleCommand cmd = new OracleCommand(commandText, con);
                try
                {
                    con.Open();
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var item = new ProductRoute();

                                item.submission_id = dr["SUBMISSION_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUBMISSION_ID"]);
                                item.route_id = dr["ROUTE_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ROUTE_ID"]);
                                item.route_type_desc = dr["ROUTE_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["ROUTE_TYPE_DESC"].ToString().Trim();

                                items.Add(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessages = string.Format("DbConnection.cs - GetAllProductRoute()");
                    ExceptionHelper.LogException(ex, errorMessages);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            return items;
        }

        public ProductRoute GetProductRouteById(int id, string lang)
        {
            var route = new ProductRoute();
            string commandText = "SELECT SUBMISSION_ID, ROUTE_ID, ";
            if (lang.Equals("fr"))
            {
                commandText += "ROUTE_TYPE_DESC_F as ROUTE_TYPE_DESC ";
            }
            else {
                commandText += "ROUTE_TYPE_DESC ";
            }
            commandText += "FROM NHPPLQ_OWNER.PRODUCT_ROUTE_ONLINE WHERE ROUTE_ID = " + id;


            using (

                OracleConnection con = new OracleConnection(LnhpdDBConnection))
            {
                OracleCommand cmd = new OracleCommand(commandText, con);
                try
                {
                    con.Open();
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var item = new ProductRoute();

                                item.submission_id = dr["SUBMISSION_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUBMISSION_ID"]);
                                item.route_id = dr["ROUTE_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ROUTE_ID"]);
                                item.route_type_desc = dr["ROUTE_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["ROUTE_TYPE_DESC"].ToString().Trim();

                                route = item;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessages = string.Format("DbConnection.cs - GetProductRouteById()");
                    ExceptionHelper.LogException(ex, errorMessages);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            return route;
        }

        public List<ProductRoute> GetProductRouteByLicenceNumber(int licenceNumber, string lang)
        {
            var items = new List<ProductRoute>();
            var route = new ProductRoute();
            string commandText = "SELECT DISTINCT R.SUBMISSION_ID, R.ROUTE_ID, ";
            if (lang.Equals("fr"))
            {
                commandText += "R.ROUTE_TYPE_DESC_F as R.ROUTE_TYPE_DESC ";
            }
            else {
                commandText += "R.ROUTE_TYPE_DESC ";
            }
            commandText += "FROM NHPPLQ_OWNER.PRODUCT_LICENCE_ONLINE P, NHPPLQ_OWNER.PRODUCT_ROUTE_ONLINE R WHERE P.SUBMISSION_ID = R.SUBMISSION_ID AND P.LICENCE_NUMBER = " + licenceNumber;


            using (

                OracleConnection con = new OracleConnection(LnhpdDBConnection))
            {
                OracleCommand cmd = new OracleCommand(commandText, con);
                try
                {
                    con.Open();
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var item = new ProductRoute();

                                item.submission_id = dr["SUBMISSION_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUBMISSION_ID"]);
                                item.route_id = dr["ROUTE_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ROUTE_ID"]);
                                item.route_type_desc = dr["ROUTE_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["ROUTE_TYPE_DESC"].ToString().Trim();

                                items.Add(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessages = string.Format("DbConnection.cs - GetProductRouteByLicenceNumber()");
                    ExceptionHelper.LogException(ex, errorMessages);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
            return items;
        }

        public List<string> GetSecondaryBrandNameList(int licenceNumber, string lang)
        {
            var items = new List<string>();
            string commandText = "SELECT DISTINCT PRODUCT_NAME FROM NHPPLQ_OWNER.PRODUCT_LICENCE_ONLINE WHERE FLAG_PRIMARY_NAME = 0 AND LICENCE_NUMBER = " + licenceNumber;
            commandText += " ORDER BY PRODUCT_NAME ASC";

            using (

                OracleConnection con = new OracleConnection(LnhpdDBConnection))
            {
                OracleCommand cmd = new OracleCommand(commandText, con);
                try
                {
                    con.Open();
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var item = "";

                                item = dr["PRODUCT_NAME"] == DBNull.Value ? string.Empty : dr["PRODUCT_NAME"].ToString().Trim();

                                items.Add(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessages = string.Format("DbConnection.cs - GetSecondaryBrandNameList()");
                    ExceptionHelper.LogException(ex, errorMessages);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }

            if (items.Count > 0)
            {
                return items;
            }
            return null;
        }
    }
}
