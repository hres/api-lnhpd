using System;
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
            } else {
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
                                
                                item.FileNumber = dr["FILE_NUMBER"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FILE_NUMBER"]);
                                item.SubmissionId = dr["SUBMISSION_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUBMISSION_ID"]);
                                item.LicenceNumber = dr["LICENCE_NUMBER"] == DBNull.Value ? string.Empty : dr["LICENCE_NUMBER"].ToString().Trim();
                                item.LicenceDate = dr["LICENCE_DATE"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["LICENCE_DATE"]);
                                item.RevisedDate = dr["REVISED_DATE"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["REVISED_DATE"]);
                                item.TimeReceipt = dr["TIME_RECEIPT"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["TIME_RECEIPT"]);
                                item.DateStart = dr["DATE_START"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["DATE_START"]);
                                item.Notes = dr["NOTES"] == DBNull.Value ? string.Empty : dr["NOTES"].ToString().Trim(); ;
                                item.ProductNameId = dr["PRODUCT_NAME_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["PRODUCT_NAME_ID"]);
                                item.ProductName = dr["PRODUCT_NAME"] == DBNull.Value ? string.Empty : dr["PRODUCT_NAME"].ToString().Trim();
                                item.DosageForm = dr["DOSAGE_FORM"] == DBNull.Value ? string.Empty : dr["DOSAGE_FORM"].ToString().Trim();
                                item.CompanyId = dr["COMPANY_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["COMPANY_ID"]);
                                item.CompanyNameId = dr["COMPANY_NAME_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["COMPANY_NAME_ID"]);
                                item.CompanyName = dr["COMPANY_NAME"] == DBNull.Value ? string.Empty : dr["COMPANY_NAME"].ToString().Trim();
                                item.SubSubmissionTypeCode = dr["SUB_SUBMISSION_TYPE_CODE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUB_SUBMISSION_TYPE_CODE"]);
                                item.SubSubmissionTypeDesc = dr["SUB_SUBMISSION_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["SUB_SUBMISSION_TYPE_DESC"].ToString().Trim();
                                item.FlagPrimaryName = dr["FLAG_PRIMARY_NAME"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FLAG_PRIMARY_NAME"]);
                                item.FlagProductStatus = dr["FLAG_PRODUCT_STATUS"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FLAG_PRODUCT_STATUS"]);
                                item.FlagAttestedMonograph = dr["FLAG_ATTESTED_MONOGRAPH"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FLAG_ATTESTED_MONOGRAPH"]);

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

                                item.FileNumber = dr["FILE_NUMBER"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FILE_NUMBER"]);
                                item.SubmissionId = dr["SUBMISSION_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUBMISSION_ID"]);
                                item.LicenceNumber = dr["LICENCE_NUMBER"] == DBNull.Value ? string.Empty : dr["LICENCE_NUMBER"].ToString().Trim();
                                item.LicenceDate = dr["LICENCE_DATE"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["LICENCE_DATE"]);
                                item.RevisedDate = dr["REVISED_DATE"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["REVISED_DATE"]);
                                item.TimeReceipt = dr["TIME_RECEIPT"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["TIME_RECEIPT"]);
                                item.DateStart = dr["DATE_START"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["DATE_START"]);
                                item.Notes = dr["NOTES"] == DBNull.Value ? string.Empty : dr["NOTES"].ToString().Trim(); ;
                                item.ProductNameId = dr["PRODUCT_NAME_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["PRODUCT_NAME_ID"]);
                                item.ProductName = dr["PRODUCT_NAME"] == DBNull.Value ? string.Empty : dr["PRODUCT_NAME"].ToString().Trim();
                                item.DosageForm = dr["DOSAGE_FORM"] == DBNull.Value ? string.Empty : dr["DOSAGE_FORM"].ToString().Trim();
                                item.CompanyId = dr["COMPANY_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["COMPANY_ID"]);
                                item.CompanyNameId = dr["COMPANY_NAME_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["COMPANY_NAME_ID"]);
                                item.CompanyName = dr["COMPANY_NAME"] == DBNull.Value ? string.Empty : dr["COMPANY_NAME"].ToString().Trim();
                                item.SubSubmissionTypeCode = dr["SUB_SUBMISSION_TYPE_CODE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUB_SUBMISSION_TYPE_CODE"]);
                                item.SubSubmissionTypeDesc = dr["SUB_SUBMISSION_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["SUB_SUBMISSION_TYPE_DESC"].ToString().Trim();
                                item.FlagPrimaryName = dr["FLAG_PRIMARY_NAME"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FLAG_PRIMARY_NAME"]);
                                item.FlagProductStatus = dr["FLAG_PRODUCT_STATUS"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FLAG_PRODUCT_STATUS"]);
                                item.FlagAttestedMonograph = dr["FLAG_ATTESTED_MONOGRAPH"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FLAG_ATTESTED_MONOGRAPH"]);

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

        public ProductLicence GetProductLicenceById(int id, string lang)
        {
            var licence = new ProductLicence();
            string commandText = "SELECT FILE_NUMBER, SUBMISSION_ID, LICENCE_NUMBER, LICENCE_DATE, REVISED_DATE, TIME_RECEIPT, DATE_START, NOTES, PRODUCT_NAME_ID, PRODUCT_NAME, COMPANY_ID, COMPANY_NAME_ID, COMPANY_NAME, SUB_SUBMISSION_TYPE_CODE, FLAG_PRIMARY_NAME, FLAG_PRODUCT_STATUS, FLAG_ATTESTED_MONOGRAPH, ";
            if (lang.Equals("fr"))
            {
                commandText += "DOSAGE_FORM_F as DOSAGE_FORM, SUB_SUBMISSION_TYPE_DESC_F as SUB_SUBMISSION_TYPE_DESC ";
            } else {
                commandText += "DOSAGE_FORM, SUB_SUBMISSION_TYPE_DESC ";
            }
            commandText += "FROM NHPPLQ_OWNER.PRODUCT_LICENCE_ONLINE WHERE LICENCE_NUMBER = " + id;


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
                                
                                item.FileNumber = dr["FILE_NUMBER"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FILE_NUMBER"]);
                                item.SubmissionId = dr["SUBMISSION_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUBMISSION_ID"]);
                                item.LicenceNumber = dr["LICENCE_NUMBER"] == DBNull.Value ? string.Empty : dr["LICENCE_NUMBER"].ToString().Trim();
                                item.LicenceDate = dr["LICENCE_DATE"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["LICENCE_DATE"]);
                                item.RevisedDate = dr["REVISED_DATE"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["REVISED_DATE"]);
                                item.TimeReceipt = dr["TIME_RECEIPT"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["TIME_RECEIPT"]);
                                item.DateStart = dr["DATE_START"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["DATE_START"]);
                                item.Notes = dr["NOTES"] == DBNull.Value ? string.Empty : dr["NOTES"].ToString().Trim(); ;
                                item.ProductNameId = dr["PRODUCT_NAME_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["PRODUCT_NAME_ID"]);
                                item.ProductName = dr["PRODUCT_NAME"] == DBNull.Value ? string.Empty : dr["PRODUCT_NAME"].ToString().Trim();
                                item.DosageForm = dr["DOSAGE_FORM"] == DBNull.Value ? string.Empty : dr["DOSAGE_FORM"].ToString().Trim();
                                item.CompanyId = dr["COMPANY_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["COMPANY_ID"]);
                                item.CompanyNameId = dr["COMPANY_NAME_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["COMPANY_NAME_ID"]);
                                item.CompanyName = dr["COMPANY_NAME"] == DBNull.Value ? string.Empty : dr["COMPANY_NAME"].ToString().Trim();
                                item.SubSubmissionTypeCode = dr["SUB_SUBMISSION_TYPE_CODE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUB_SUBMISSION_TYPE_CODE"]);
                                item.SubSubmissionTypeDesc = dr["SUB_SUBMISSION_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["SUB_SUBMISSION_TYPE_DESC"].ToString().Trim();
                                item.FlagPrimaryName = dr["FLAG_PRIMARY_NAME"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FLAG_PRIMARY_NAME"]);
                                item.FlagProductStatus = dr["FLAG_PRODUCT_STATUS"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FLAG_PRODUCT_STATUS"]);
                                item.FlagAttestedMonograph = dr["FLAG_ATTESTED_MONOGRAPH"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FLAG_ATTESTED_MONOGRAPH"]);
                                
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
            } else {
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
                                
                                item.SubmissionId = dr["SUBMISSION_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUBMISSION_ID"]);
                                item.IngredientName = dr["INGREDIENT_NAME"] == DBNull.Value ? string.Empty : dr["INGREDIENT_NAME"].ToString().Trim();
                                item.MatrixId = dr["MATRIX_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["MATRIX_ID"]);
                                item.MatrixTypeCode = dr["MATRIX_TYPE_CODE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["MATRIX_TYPE_CODE"]);

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
                                
                                item.SubmissionId = dr["SUBMISSION_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUBMISSION_ID"]);
                                item.IngredientName = dr["INGREDIENT_NAME"] == DBNull.Value ? string.Empty : dr["INGREDIENT_NAME"].ToString().Trim();
                                item.MatrixId = dr["MATRIX_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["MATRIX_ID"]);
                                item.MatrixTypeCode = dr["MATRIX_TYPE_CODE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["MATRIX_TYPE_CODE"]);

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

        public List<IngredientQuantity> GetAllIngredientQuantity(string lang)
        {
            var items = new List<IngredientQuantity>();
            string commandText = "SELECT INGREDIENT_AMOUNT_ID, MATRIX_ID, QUANTITY, QUANTITY_MINIMUM, QUANTITY_MAXIMUM, RATIO_NUMERATOR, RATIO_DENOMINATOR, DRIED_HERB_EQUIVALENT, POTENCY_AMOUNT, POTENCY_CONSTITUENT, ";
            if (lang.Equals("fr"))
            {
                commandText += "UOM_TYPE_DESC_AMT_QUANTITY_F as UOM_TYPE_DESC_AMT_QUANTITY, UOM_TYPE_DESC_DHE_F as UOM_TYPE_DESC_DHE, EXTRACT_TYPE_DESC_F as EXTRACT_TYPE_DESC, UOM_TYPE_DESC_POTENCY_F as UOM_TYPE_DESC_POTENCY ";
            } else {
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
                                
                                item.IngredientAmountId = dr["INGREDIENT_AMOUNT_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["INGREDIENT_AMOUNT_ID"]);
                                item.MatrixId = dr["MATRIX_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["MATRIX_ID"]);
                                item.Quantity = dr["QUANTITY"] == DBNull.Value ? 0 : Convert.ToInt32(dr["QUANTITY"]);
                                item.QuantityMinimum = dr["QUANTITY_MINIMUM"] == DBNull.Value ? 0 : Convert.ToInt32(dr["QUANTITY_MINIMUM"]);
                                item.QuantityMaximum = dr["QUANTITY_MAXIMUM"] == DBNull.Value ? 0 : Convert.ToInt32(dr["QUANTITY_MAXIMUM"]);
                                item.UomTypeDescAmtQuantity = dr["UOM_TYPE_DESC_AMT_QUANTITY"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_AMT_QUANTITY"].ToString().Trim();
                                item.RatioNumerator = dr["RATIO_NUMERATOR"] == DBNull.Value ? string.Empty : dr["RATIO_NUMERATOR"].ToString().Trim();
                                item.RatioDenominator = dr["RATIO_DENOMINATOR"] == DBNull.Value ? string.Empty : dr["RATIO_DENOMINATOR"].ToString().Trim();
                                item.DriedHerbEquivalent = dr["DRIED_HERB_EQUIVALENT"] == DBNull.Value ? string.Empty : dr["DRIED_HERB_EQUIVALENT"].ToString().Trim();
                                item.UomTypeDescDhe = dr["UOM_TYPE_DESC_DHE"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_DHE"].ToString().Trim();
                                item.ExtractTypeDesc = dr["EXTRACT_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["EXTRACT_TYPE_DESC"].ToString().Trim();
                                item.PotencyAmount = dr["POTENCY_AMOUNT"] == DBNull.Value ? 0 : Convert.ToInt32(dr["POTENCY_AMOUNT"]);
                                item.UomTypeDescPotency = dr["UOM_TYPE_DESC_POTENCY"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_POTENCY"].ToString().Trim();
                                item.PotencyConstituent = dr["POTENCY_CONSTITUENT"] == DBNull.Value ? string.Empty : dr["POTENCY_CONSTITUENT"].ToString().Trim();

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

                                item.IngredientAmountId = dr["INGREDIENT_AMOUNT_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["INGREDIENT_AMOUNT_ID"]);
                                item.MatrixId = dr["MATRIX_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["MATRIX_ID"]);
                                item.Quantity = dr["QUANTITY"] == DBNull.Value ? 0 : Convert.ToInt32(dr["QUANTITY"]);
                                item.QuantityMinimum = dr["QUANTITY_MINIMUM"] == DBNull.Value ? 0 : Convert.ToInt32(dr["QUANTITY_MINIMUM"]);
                                item.QuantityMaximum = dr["QUANTITY_MAXIMUM"] == DBNull.Value ? 0 : Convert.ToInt32(dr["QUANTITY_MAXIMUM"]);
                                item.UomTypeDescAmtQuantity = dr["UOM_TYPE_DESC_AMT_QUANTITY"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_AMT_QUANTITY"].ToString().Trim();
                                item.RatioNumerator = dr["RATIO_NUMERATOR"] == DBNull.Value ? string.Empty : dr["RATIO_NUMERATOR"].ToString().Trim();
                                item.RatioDenominator = dr["RATIO_DENOMINATOR"] == DBNull.Value ? string.Empty : dr["RATIO_DENOMINATOR"].ToString().Trim();
                                item.DriedHerbEquivalent = dr["DRIED_HERB_EQUIVALENT"] == DBNull.Value ? string.Empty : dr["DRIED_HERB_EQUIVALENT"].ToString().Trim();
                                item.UomTypeDescDhe = dr["UOM_TYPE_DESC_DHE"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_DHE"].ToString().Trim();
                                item.ExtractTypeDesc = dr["EXTRACT_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["EXTRACT_TYPE_DESC"].ToString().Trim();
                                item.PotencyAmount = dr["POTENCY_AMOUNT"] == DBNull.Value ? 0 : Convert.ToInt32(dr["POTENCY_AMOUNT"]);
                                item.UomTypeDescPotency = dr["UOM_TYPE_DESC_POTENCY"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_POTENCY"].ToString().Trim();
                                item.PotencyConstituent = dr["POTENCY_CONSTITUENT"] == DBNull.Value ? string.Empty : dr["POTENCY_CONSTITUENT"].ToString().Trim();

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
                                
                                item.MaterialId = dr["MATERIAL_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["MATERIAL_ID"]);
                                item.MatrixId = dr["MATRIX_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["MATRIX_ID"]);
                                item.MaterialTypeDesc = dr["MATERIAL_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["MATERIAL_TYPE_DESC"].ToString().Trim();

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

                                item.MaterialId = dr["MATERIAL_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["MATERIAL_ID"]);
                                item.MatrixId = dr["MATRIX_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["MATRIX_ID"]);
                                item.MaterialTypeDesc = dr["MATERIAL_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["MATERIAL_TYPE_DESC"].ToString().Trim();

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
                commandText += "NAME_UPPER_F as NAME_UPPER";
            }
            else {
                commandText += "NAME_UPPER";
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
                                
                                item.MatrixId = dr["MATRIX_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["MATRIX_ID"]);
                                item.SubmissionId = dr["SUBMISSION_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUBMISSION_ID"]);
                                item.IngredientTypeCode = dr["INGREDIENT_TYPE_CODE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["INGREDIENT_TYPE_CODE"]);
                                item.NameUpper = dr["NAME_UPPER"] == DBNull.Value ? string.Empty : dr["NAME_UPPER"].ToString().Trim();

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
                commandText += "NAME_UPPER_F as NAME_UPPER";
            }
            else {
                commandText += "NAME_UPPER";
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

                                item.MatrixId = dr["MATRIX_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["MATRIX_ID"]);
                                item.SubmissionId = dr["SUBMISSION_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUBMISSION_ID"]);
                                item.IngredientTypeCode = dr["INGREDIENT_TYPE_CODE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["INGREDIENT_TYPE_CODE"]);
                                item.NameUpper = dr["NAME_UPPER"] == DBNull.Value ? string.Empty : dr["NAME_UPPER"].ToString().Trim();

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
                                
                                item.SubmissionId = dr["SUBMISSION_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUBMISSION_ID"]);
                                item.DoseId = dr["DOSE_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["DOSE_ID"]);
                                item.PopulationTypeDesc = dr["POPULATION_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["POPULATION_TYPE_DESC"].ToString().Trim();
                                item.Age = dr["AGE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AGE"]);
                                item.AgeMinimum = dr["AGE_MINIMUM"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AGE_MINIMUM"]);
                                item.AgeMaximum = dr["AGE_MAXIMUM"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AGE_MAXIMUM"]);
                                item.UomTypeDescAge = dr["UOM_TYPE_DESC_AGE"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_AGE"].ToString().Trim();
                                item.QuantityDose = dr["QUANTITY_DOSE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["QUANTITY_DOSE"]);
                                item.QuantityMinimumDose = dr["QUANTITY_MINIMUM_DOSE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["QUANTITY_MINIMUM_DOSE"]);
                                item.QuantityMaximumDose = dr["QUANTITY_MAXIMUM_DOSE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["QUANTITY_MAXIMUM_DOSE"]);
                                item.UomTypeDescQuantityDose = dr["UOM_TYPE_DESC_QUANTITY_DOSE"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_QUANTITY_DOSE"].ToString().Trim();
                                item.Frequency = dr["FREQUENCY"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FREQUENCY"]);
                                item.FrequencyMinimum = dr["FREQUENCY_MINIMUM"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FREQUENCY_MINIMUM"]);
                                item.FrequencyMaximum = dr["FREQUENCY_MAXIMUM"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FREQUENCY_MAXIMUM"]);
                                item.UomTypeDescFrequency = dr["UOM_TYPE_DESC_FREQUENCY"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_FREQUENCY"].ToString().Trim();

                                items.Add(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessages = string.Format("DbConnection.cs - GetAllProduceDose()");
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
            var dose = new ProductDose  ();
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

                                item.SubmissionId = dr["SUBMISSION_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUBMISSION_ID"]);
                                item.DoseId = dr["DOSE_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["DOSE_ID"]);
                                item.PopulationTypeDesc = dr["POPULATION_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["POPULATION_TYPE_DESC"].ToString().Trim();
                                item.Age = dr["AGE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AGE"]);
                                item.AgeMinimum = dr["AGE_MINIMUM"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AGE_MINIMUM"]);
                                item.AgeMaximum = dr["AGE_MAXIMUM"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AGE_MAXIMUM"]);
                                item.UomTypeDescAge = dr["UOM_TYPE_DESC_AGE"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_AGE"].ToString().Trim();
                                item.QuantityDose = dr["QUANTITY_DOSE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["QUANTITY_DOSE"]);
                                item.QuantityMinimumDose = dr["QUANTITY_MINIMUM_DOSE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["QUANTITY_MINIMUM_DOSE"]);
                                item.QuantityMaximumDose = dr["QUANTITY_MAXIMUM_DOSE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["QUANTITY_MAXIMUM_DOSE"]);
                                item.UomTypeDescQuantityDose = dr["UOM_TYPE_DESC_QUANTITY_DOSE"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_QUANTITY_DOSE"].ToString().Trim();
                                item.Frequency = dr["FREQUENCY"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FREQUENCY"]);
                                item.FrequencyMinimum = dr["FREQUENCY_MINIMUM"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FREQUENCY_MINIMUM"]);
                                item.FrequencyMaximum = dr["FREQUENCY_MAXIMUM"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FREQUENCY_MAXIMUM"]);
                                item.UomTypeDescFrequency = dr["UOM_TYPE_DESC_FREQUENCY"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_FREQUENCY"].ToString().Trim();

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
                                
                                item.TextId = dr["TEXT_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["TEXT_ID"]);
                                item.SubmissionId = dr["SUBMISSION_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUBMISSION_ID"]);
                                item.Purpose = dr["PURPOSE_E"] == DBNull.Value ? string.Empty : dr["PURPOSE_E"].ToString().Trim();

                                items.Add(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessages = string.Format("DbConnection.cs - GetAllProducePurpose()");
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
                                
                                item.TextId = dr["TEXT_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["TEXT_ID"]);
                                item.SubmissionId = dr["SUBMISSION_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUBMISSION_ID"]);
                                item.Purpose = dr["PURPOSE_E"] == DBNull.Value ? string.Empty : dr["PURPOSE_E"].ToString().Trim();

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
                                
                                item.SubmissionId = dr["SUBMISSION_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUBMISSION_ID"]);
                                item.RiskId = dr["RISK_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RISK_ID"]);
                                item.RiskTypeDesc = dr["RISK_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["RISK_TYPE_DESC"].ToString().Trim();
                                item.SubRiskTypeDesc = dr["SUB_RISK_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["SUB_RISK_TYPE_DESC"].ToString().Trim();

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

                                item.SubmissionId = dr["SUBMISSION_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUBMISSION_ID"]);
                                item.RiskId = dr["RISK_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RISK_ID"]);
                                item.RiskTypeDesc = dr["RISK_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["RISK_TYPE_DESC"].ToString().Trim();
                                item.SubRiskTypeDesc = dr["SUB_RISK_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["SUB_RISK_TYPE_DESC"].ToString().Trim();

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
                                
                                item.TextId = dr["TEXT_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["TEXT_ID"]);
                                item.RiskId = dr["RISK_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RISK_ID"]);
                                item.RiskType = dr["RISK_TEXT_E"] == DBNull.Value ? string.Empty : dr["RISK_TEXT_E"].ToString().Trim();

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

                                item.TextId = dr["TEXT_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["TEXT_ID"]);
                                item.RiskId = dr["RISK_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RISK_ID"]);
                                item.RiskType = dr["RISK_TEXT_E"] == DBNull.Value ? string.Empty : dr["RISK_TEXT_E"].ToString().Trim();

                                riskText = item;
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
            return riskText;
        }

        public List<ProductRoute> GetAllProductRoute(string lang)
        {
            var items = new List<ProductRoute>();
            string commandText = "SELECT SUBMISSION_ID, ROUTE_ID, ";
            if (lang.Equals("fr"))
            {
                commandText += "ROUTE_TYPE_DESC_F as ROUTE_TYPE_DESC";
            }
            else {
                commandText += "ROUTE_TYPE_DESC";
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
                                
                                item.SubmissionId = dr["SUBMISSION_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUBMISSION_ID"]);
                                item.RouteId = dr["ROUTE_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ROUTE_ID"]);
                                item.RouteTypeDesc = dr["ROUTE_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["ROUTE_TYPE_DESC"].ToString().Trim();

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

        public ProductRoute GetProductRouteById(int id, string lang)
        {
            var route = new ProductRoute();
            string commandText = "SELECT SUBMISSION_ID, ROUTE_ID, ";
            if (lang.Equals("fr"))
            {
                commandText += "ROUTE_TYPE_DESC_F as ROUTE_TYPE_DESC";
            }
            else {
                commandText += "ROUTE_TYPE_DESC";
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

                                item.SubmissionId = dr["SUBMISSION_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUBMISSION_ID"]);
                                item.RouteId = dr["ROUTE_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ROUTE_ID"]);
                                item.RouteTypeDesc = dr["ROUTE_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["ROUTE_TYPE_DESC"].ToString().Trim();

                                route = item;
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
            return route;
        }
    }
}
