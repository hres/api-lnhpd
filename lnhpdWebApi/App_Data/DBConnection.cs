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


        public List<ProductLicence> GetAllProductLicence()
        {
            var items = new List<ProductLicence>();
            string commandText = "SELECT * FROM NHPPLQ_OWNER.PRODUCT_LICENCE_ONLINE";

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
                                item.Notes = dr["NOTES"] == DBNull.Value ? string.Empty : dr["NOTES"].ToString().Trim();;
                                item.ProductNameId = dr["PRODUCT_NAME_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["PRODUCT_NAME_ID"]);
                                item.ProductName = dr["PRODUCT_NAME"] == DBNull.Value ? string.Empty : dr["PRODUCT_NAME"].ToString().Trim();
                                item.DosageFormE = dr["DOSAGE_FORM"] == DBNull.Value ? string.Empty : dr["DOSAGE_FORM"].ToString().Trim();
                                item.DosageFormF = dr["DOSAGE_FORM_F"] == DBNull.Value ? string.Empty : dr["DOSAGE_FORM_F"].ToString().Trim();
                                item.CompanyId = dr["COMPANY_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["COMPANY_ID"]);
                                item.CompanyNameId = dr["COMPANY_NAME_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["COMPANY_NAME_ID"]);
                                item.CompanyName = dr["COMPANY_NAME"] == DBNull.Value ? string.Empty : dr["COMPANY_NAME"].ToString().Trim();
                                item.SubSubmissionTypeCode = dr["SUB_SUBMISSION_TYPE_CODE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUB_SUBMISSION_TYPE_CODE"]);
                                item.SubSubmissionTypeDescE = dr["SUB_SUBMISSION_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["SUB_SUBMISSION_TYPE_DESC"].ToString().Trim();
                                item.SubSubmissionTypeDescF = dr["SUB_SUBMISSION_TYPE_DESC_F"] == DBNull.Value ? string.Empty : dr["SUB_SUBMISSION_TYPE_DESC_F"].ToString().Trim();
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

        public ProductLicence GetProductLicenceById(int id)
        {
            var licence = new ProductLicence();
            string commandText = "SELECT * FROM NHPPLQ_OWNER.PRODUCT_LICENCE_ONLINE WHERE SUBMISSION_ID = " + id;

           
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
                                item.DosageFormE = dr["DOSAGE_FORM"] == DBNull.Value ? string.Empty : dr["DOSAGE_FORM"].ToString().Trim();
                                item.DosageFormF = dr["DOSAGE_FORM_F"] == DBNull.Value ? string.Empty : dr["DOSAGE_FORM_F"].ToString().Trim();
                                item.CompanyId = dr["COMPANY_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["COMPANY_ID"]);
                                item.CompanyNameId = dr["COMPANY_NAME_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["COMPANY_NAME_ID"]);
                                item.CompanyName = dr["COMPANY_NAME"] == DBNull.Value ? string.Empty : dr["COMPANY_NAME"].ToString().Trim();
                                item.SubSubmissionTypeCode = dr["SUB_SUBMISSION_TYPE_CODE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUB_SUBMISSION_TYPE_CODE"]);
                                item.SubSubmissionTypeDescE = dr["SUB_SUBMISSION_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["SUB_SUBMISSION_TYPE_DESC"].ToString().Trim();
                                item.SubSubmissionTypeDescF = dr["SUB_SUBMISSION_TYPE_DESC_F"] == DBNull.Value ? string.Empty : dr["SUB_SUBMISSION_TYPE_DESC_F"].ToString().Trim();
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

        public List<Ingredient> GetAllIngredient()
        {
            var items = new List<Ingredient>();
            string commandText = "SELECT * FROM NHPPLQ_OWNER.INGREDIENT_ONLINE";

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
                                item.IngredientNameOther = dr["INGREDIENT_NAME_OTHER"] == DBNull.Value ? string.Empty : dr["INGREDIENT_NAME_OTHER"].ToString().Trim();
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

        public Ingredient GetIngredientById(int id)
        {
            var ingredient = new Ingredient();
            string commandText = "SELECT * FROM NHPPLQ_OWNER.INGREDIENT_ONLINE WHERE SUBMISSION_ID = " + id;


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
                                item.IngredientNameOther = dr["INGREDIENT_NAME_OTHER"] == DBNull.Value ? string.Empty : dr["INGREDIENT_NAME_OTHER"].ToString().Trim();
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

        public List<IngredientQuantity> GetAllIngredientQuantity()
        {
            var items = new List<IngredientQuantity>();
            string commandText = "SELECT * FROM NHPPLQ_OWNER.INGREDIENT_QUANTITY_ONLINE";

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
                                item.UomTypeDescAmtQuantityE = dr["UOM_TYPE_DESC_AMT_QUANTITY"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_AMT_QUANTITY"].ToString().Trim();
                                item.UomTypeDescAmtQuantityF = dr["UOM_TYPE_DESC_AMT_QUANTITY_F"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_AMT_QUANTITY_F"].ToString().Trim();
                                item.RatioNumerator = dr["RATIO_NUMERATOR"] == DBNull.Value ? string.Empty : dr["RATIO_NUMERATOR"].ToString().Trim();
                                item.RatioDenominator = dr["RATIO_DENOMINATOR"] == DBNull.Value ? string.Empty : dr["RATIO_DENOMINATOR"].ToString().Trim();
                                item.DriedHerbEquivalent = dr["DRIED_HERB_EQUIVALENT"] == DBNull.Value ? string.Empty : dr["DRIED_HERB_EQUIVALENT"].ToString().Trim();
                                item.UomTypeDescDheE = dr["UOM_TYPE_DESC_DHE"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_DHE"].ToString().Trim();
                                item.UomTypeDescDheF = dr["UOM_TYPE_DESC_DHE_F"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_DHE_F"].ToString().Trim();
                                item.ExtractTypeDescE = dr["EXTRACT_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["EXTRACT_TYPE_DESC"].ToString().Trim();
                                item.ExtractTypeDescF = dr["EXTRACT_TYPE_DESC_F"] == DBNull.Value ? string.Empty : dr["EXTRACT_TYPE_DESC_F"].ToString().Trim();
                                item.PotencyAmount = dr["POTENCY_AMOUNT"] == DBNull.Value ? 0 : Convert.ToInt32(dr["POTENCY_AMOUNT"]);
                                item.UomTypeDescPotencyE = dr["UOM_TYPE_DESC_POTENCY"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_POTENCY"].ToString().Trim();
                                item.UomTypeDescPotencyF = dr["UOM_TYPE_DESC_POTENCY_F"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_POTENCY_F"].ToString().Trim();
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

        public IngredientQuantity GetIngredientQuantityById(int id)
        {
            var quantity = new IngredientQuantity();
            string commandText = "SELECT * FROM NHPPLQ_OWNER.INGREDIENT_QUANTITY_ONLINE WHERE INGREDIENT_AMOUNT_ID = " + id;


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
                                item.UomTypeDescAmtQuantityE = dr["UOM_TYPE_DESC_AMT_QUANTITY"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_AMT_QUANTITY"].ToString().Trim();
                                item.UomTypeDescAmtQuantityF = dr["UOM_TYPE_DESC_AMT_QUANTITY_F"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_AMT_QUANTITY_F"].ToString().Trim();
                                item.RatioNumerator = dr["RATIO_NUMERATOR"] == DBNull.Value ? string.Empty : dr["RATIO_NUMERATOR"].ToString().Trim();
                                item.RatioDenominator = dr["RATIO_DENOMINATOR"] == DBNull.Value ? string.Empty : dr["RATIO_DENOMINATOR"].ToString().Trim();
                                item.DriedHerbEquivalent = dr["DRIED_HERB_EQUIVALENT"] == DBNull.Value ? string.Empty : dr["DRIED_HERB_EQUIVALENT"].ToString().Trim();
                                item.UomTypeDescDheE = dr["UOM_TYPE_DESC_DHE"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_DHE"].ToString().Trim();
                                item.UomTypeDescDheF = dr["UOM_TYPE_DESC_DHE_F"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_DHE_F"].ToString().Trim();
                                item.ExtractTypeDescE = dr["EXTRACT_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["EXTRACT_TYPE_DESC"].ToString().Trim();
                                item.ExtractTypeDescF = dr["EXTRACT_TYPE_DESC_F"] == DBNull.Value ? string.Empty : dr["EXTRACT_TYPE_DESC_F"].ToString().Trim();
                                item.PotencyAmount = dr["POTENCY_AMOUNT"] == DBNull.Value ? 0 : Convert.ToInt32(dr["POTENCY_AMOUNT"]);
                                item.UomTypeDescPotencyE = dr["UOM_TYPE_DESC_POTENCY"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_POTENCY"].ToString().Trim();
                                item.UomTypeDescPotencyF = dr["UOM_TYPE_DESC_POTENCY_F"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_POTENCY_F"].ToString().Trim();
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

        public List<IngredientSource> GetAllIngredientSource()
        {
            var items = new List<IngredientSource>();
            string commandText = "SELECT * FROM NHPPLQ_OWNER.INGREDIENT_SOURCE_ONLINE";

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
                                item.MaterialTypeDescE = dr["MATERIAL_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["MATERIAL_TYPE_DESC"].ToString().Trim();
                                item.MaterialTypeDescF = dr["MATERIAL_TYPE_DESC_F"] == DBNull.Value ? string.Empty : dr["MATERIAL_TYPE_DESC_F"].ToString().Trim();

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

        public IngredientSource GetIngredientSourceById(int id)
        {
            var source = new IngredientSource();
            string commandText = "SELECT * FROM NHPPLQ_OWNER.INGREDIENT_SOURCE_ONLINE WHERE MATERIAL_ID = " + id;


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
                                item.MaterialTypeDescE = dr["MATERIAL_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["MATERIAL_TYPE_DESC"].ToString().Trim();
                                item.MaterialTypeDescF = dr["MATERIAL_TYPE_DESC_F"] == DBNull.Value ? string.Empty : dr["MATERIAL_TYPE_DESC_F"].ToString().Trim();

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

        public List<IngredientSubmission> GetAllIngredientSubmission()
        {
            var items = new List<IngredientSubmission>();
            string commandText = "SELECT * FROM NHPPLQ_OWNER.INGREDIENT_SUBMISSION_ONLINE";

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
                                item.NameUpperE = dr["NAME_UPPER"] == DBNull.Value ? string.Empty : dr["NAME_UPPER"].ToString().Trim();
                                item.NameUpperF = dr["NAME_UPPER_F"] == DBNull.Value ? string.Empty : dr["NAME_UPPER_F"].ToString().Trim();

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

        public IngredientSubmission GetIngredientSubmissionById(int id)
        {
            var submission = new IngredientSubmission();
            string commandText = "SELECT * FROM NHPPLQ_OWNER.INGREDIENT_SUBMISSION_ONLINE WHERE SUBMISSION_ID = " + id;


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
                                item.NameUpperE = dr["NAME_UPPER"] == DBNull.Value ? string.Empty : dr["NAME_UPPER"].ToString().Trim();
                                item.NameUpperF = dr["NAME_UPPER_F"] == DBNull.Value ? string.Empty : dr["NAME_UPPER_F"].ToString().Trim();

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

        public List<ProductDose> GetAllProductDose()
        {
            var items = new List<ProductDose>();
            string commandText = "SELECT * FROM NHPPLQ_OWNER.PRODUCT_DOSE_ONLINE";

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
                                item.PopulationTypeDescE = dr["POPULATION_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["POPULATION_TYPE_DESC"].ToString().Trim();
                                item.PopulationTypeDescF = dr["POPULATION_TYPE_DESC_F"] == DBNull.Value ? string.Empty : dr["POPULATION_TYPE_DESC_F"].ToString().Trim();
                                item.Age = dr["AGE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AGE"]);
                                item.AgeMinimum = dr["AGE_MINIMUM"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AGE_MINIMUM"]);
                                item.AgeMaximum = dr["AGE_MAXIMUM"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AGE_MAXIMUM"]);
                                item.UomTypeDescAgeE = dr["UOM_TYPE_DESC_AGE"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_AGE"].ToString().Trim();
                                item.UomTypeDescAgeF = dr["UOM_TYPE_DESC_AGE_F"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_AGE_F"].ToString().Trim();
                                item.QuantityDose = dr["QUANTITY_DOSE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["QUANTITY_DOSE"]);
                                item.QuantityMinimumDose = dr["QUANTITY_MINIMUM_DOSE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["QUANTITY_MINIMUM_DOSE"]);
                                item.QuantityMaximumDose = dr["QUANTITY_MAXIMUM_DOSE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["QUANTITY_MAXIMUM_DOSE"]);
                                item.UomTypeDescQuantityDoseE = dr["UOM_TYPE_DESC_QUANTITY_DOSE"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_QUANTITY_DOSE"].ToString().Trim();
                                item.UomTypeDescQuantityDoseF = dr["UOM_TYPE_DESC_QUANTITY_DOSE_F"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_QUANTITY_DOSE_F"].ToString().Trim();
                                item.Frequency = dr["FREQUENCY"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FREQUENCY"]);
                                item.FrequencyMinimum = dr["FREQUENCY_MINIMUM"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FREQUENCY_MINIMUM"]);
                                item.FrequencyMaximum = dr["FREQUENCY_MAXIMUM"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FREQUENCY_MAXIMUM"]);
                                item.UomTypeDescFrequencyE = dr["UOM_TYPE_DESC_FREQUENCY"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_FREQUENCY"].ToString().Trim();
                                item.UomTypeDescFrequencyF = dr["UOM_TYPE_DESC_FREQUENCY_F"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_FREQUENCY_F"].ToString().Trim();

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

        public ProductDose GetProductDoseById(int id)
        {
            var dose = new ProductDose  ();
            string commandText = "SELECT * FROM NHPPLQ_OWNER.PRODUCT_DOSE_ONLINE WHERE DOSE_ID = " + id;


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
                                item.PopulationTypeDescE = dr["POPULATION_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["POPULATION_TYPE_DESC"].ToString().Trim();
                                item.PopulationTypeDescF = dr["POPULATION_TYPE_DESC_F"] == DBNull.Value ? string.Empty : dr["POPULATION_TYPE_DESC_F"].ToString().Trim();
                                item.Age = dr["AGE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AGE"]);
                                item.AgeMinimum = dr["AGE_MINIMUM"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AGE_MINIMUM"]);
                                item.AgeMaximum = dr["AGE_MAXIMUM"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AGE_MAXIMUM"]);
                                item.UomTypeDescAgeE = dr["UOM_TYPE_DESC_AGE"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_AGE"].ToString().Trim();
                                item.UomTypeDescAgeF = dr["UOM_TYPE_DESC_AGE_F"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_AGE_F"].ToString().Trim();
                                item.QuantityDose = dr["QUANTITY_DOSE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["QUANTITY_DOSE"]);
                                item.QuantityMinimumDose = dr["QUANTITY_MINIMUM_DOSE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["QUANTITY_MINIMUM_DOSE"]);
                                item.QuantityMaximumDose = dr["QUANTITY_MAXIMUM_DOSE"] == DBNull.Value ? 0 : Convert.ToInt32(dr["QUANTITY_MAXIMUM_DOSE"]);
                                item.UomTypeDescQuantityDoseE = dr["UOM_TYPE_DESC_QUANTITY_DOSE"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_QUANTITY_DOSE"].ToString().Trim();
                                item.UomTypeDescQuantityDoseF = dr["UOM_TYPE_DESC_QUANTITY_DOSE_F"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_QUANTITY_DOSE_F"].ToString().Trim();
                                item.Frequency = dr["FREQUENCY"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FREQUENCY"]);
                                item.FrequencyMinimum = dr["FREQUENCY_MINIMUM"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FREQUENCY_MINIMUM"]);
                                item.FrequencyMaximum = dr["FREQUENCY_MAXIMUM"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FREQUENCY_MAXIMUM"]);
                                item.UomTypeDescFrequencyE = dr["UOM_TYPE_DESC_FREQUENCY"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_FREQUENCY"].ToString().Trim();
                                item.UomTypeDescFrequencyF = dr["UOM_TYPE_DESC_FREQUENCY_F"] == DBNull.Value ? string.Empty : dr["UOM_TYPE_DESC_FREQUENCY_F"].ToString().Trim();

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

        public List<ProductPurpose> GetAllProductPurpose()
        {
            var items = new List<ProductPurpose>();
            string commandText = "SELECT * FROM NHPPLQ_OWNER.PRODUCT_PURPOSE_ONLINE";

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
                                item.PurposeE = dr["PURPOSE_E"] == DBNull.Value ? string.Empty : dr["PURPOSE_E"].ToString().Trim();
                                item.PurposeF = dr["PURPOSE_F"] == DBNull.Value ? string.Empty : dr["PURPOSE_F"].ToString().Trim();

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

        public ProductPurpose GetProductPurposeById(int id)
        {
            var purpose = new ProductPurpose();
            string commandText = "SELECT * FROM NHPPLQ_OWNER.PRODUCT_PURPOSE_ONLINE WHERE SUBMISSION_ID = " + id;


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
                                item.PurposeE = dr["PURPOSE_E"] == DBNull.Value ? string.Empty : dr["PURPOSE_E"].ToString().Trim();
                                item.PurposeF = dr["PURPOSE_F"] == DBNull.Value ? string.Empty : dr["PURPOSE_F"].ToString().Trim();

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

        public List<ProductRisk> GetAllProductRisk()
        {
            var items = new List<ProductRisk>();
            string commandText = "SELECT * FROM NHPPLQ_OWNER.PRODUCT_RISK_ONLINE";

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
                                item.RiskTypeDescE = dr["RISK_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["RISK_TYPE_DESC"].ToString().Trim();
                                item.RiskTypeDescF = dr["RISK_TYPE_DESC_F"] == DBNull.Value ? string.Empty : dr["RISK_TYPE_DESC_F"].ToString().Trim();
                                item.SubRiskTypeDescE = dr["SUB_RISK_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["SUB_RISK_TYPE_DESC"].ToString().Trim();
                                item.SubRiskTypeDescF = dr["SUB_RISK_TYPE_DESC_F"] == DBNull.Value ? string.Empty : dr["SUB_RISK_TYPE_DESC_F"].ToString().Trim();

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

        public ProductRisk GetProductRiskById(int id)
        {
            var risk = new ProductRisk();
            string commandText = "SELECT * FROM NHPPLQ_OWNER.PRODUCT_RISK_ONLINE WHERE RISK_ID = " + id;


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
                                item.RiskTypeDescE = dr["RISK_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["RISK_TYPE_DESC"].ToString().Trim();
                                item.RiskTypeDescF = dr["RISK_TYPE_DESC_F"] == DBNull.Value ? string.Empty : dr["RISK_TYPE_DESC_F"].ToString().Trim();
                                item.SubRiskTypeDescE = dr["SUB_RISK_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["SUB_RISK_TYPE_DESC"].ToString().Trim();
                                item.SubRiskTypeDescF = dr["SUB_RISK_TYPE_DESC_F"] == DBNull.Value ? string.Empty : dr["SUB_RISK_TYPE_DESC_F"].ToString().Trim();

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

        public List<ProductRiskText> GetAllProductRiskText()
        {
            var items = new List<ProductRiskText>();
            string commandText = "SELECT * FROM NHPPLQ_OWNER.PRODUCT_RISK_TEXT_ONLINE";

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
                                item.RiskTypeE = dr["RISK_TEXT_E"] == DBNull.Value ? string.Empty : dr["RISK_TEXT_E"].ToString().Trim();
                                item.RiskTypeF = dr["RISK_TEXT_F"] == DBNull.Value ? string.Empty : dr["RISK_TEXT_F"].ToString().Trim();

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

        public ProductRiskText GetProductRiskTextById(int id)
        {
            var riskText = new ProductRiskText();
            string commandText = "SELECT * FROM NHPPLQ_OWNER.PRODUCT_RISK_TEXT_ONLINE WHERE TEXT_ID = " + id;


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
                                item.RiskTypeE = dr["RISK_TEXT_E"] == DBNull.Value ? string.Empty : dr["RISK_TEXT_E"].ToString().Trim();
                                item.RiskTypeF = dr["RISK_TEXT_F"] == DBNull.Value ? string.Empty : dr["RISK_TEXT_F"].ToString().Trim();

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

        public List<ProductRoute> GetAllProductRoute()
        {
            var items = new List<ProductRoute>();
            string commandText = "SELECT * FROM NHPPLQ_OWNER.PRODUCT_ROUTE_ONLINE";

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
                                item.RouteTypeDescE = dr["ROUTE_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["ROUTE_TYPE_DESC"].ToString().Trim();
                                item.RouteTypeDescF = dr["ROUTE_TYPE_DESC_F"] == DBNull.Value ? string.Empty : dr["ROUTE_TYPE_DESC_F"].ToString().Trim();

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

        public ProductRoute GetProductRouteById(int id)
        {
            var route = new ProductRoute();
            string commandText = "SELECT * FROM NHPPLQ_OWNER.PRODUCT_ROUTE_ONLINE WHERE ROUTE_ID = " + id;


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
                                item.RouteTypeDescE = dr["ROUTE_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["ROUTE_TYPE_DESC"].ToString().Trim();
                                item.RouteTypeDescF = dr["ROUTE_TYPE_DESC_F"] == DBNull.Value ? string.Empty : dr["ROUTE_TYPE_DESC_F"].ToString().Trim();

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
