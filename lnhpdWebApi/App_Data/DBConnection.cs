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
                                item.SubmissionId = dr["SUBMISSION_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUBMISSION_ID"]);
                                item.CompanyNameId = dr["COMPANY_NAME_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["COMPANY_NAME_ID"]);
                                item.CompanyName = dr["COMPANY_NAME"] == DBNull.Value ? string.Empty : dr["COMPANY_NAME"].ToString().Trim();
                                item.DosageFormE = dr["DOSAGE_FORM"] == DBNull.Value ? string.Empty : dr["DOSAGE_FORM"].ToString().Trim();
                                item.DosageFormF = dr["DOSAGE_FORM_F"] == DBNull.Value ? string.Empty : dr["DOSAGE_FORM_F"].ToString().Trim();
                                item.LicenceNumber = dr["LICENCE_NUMBER"] == DBNull.Value ? string.Empty : dr["LICENCE_NUMBER"].ToString().Trim();
                                item.ProductName = dr["PRODUCT_NAME"] == DBNull.Value ? string.Empty : dr["PRODUCT_NAME"].ToString().Trim();
                                item.SubSubmissionTypeDescE = dr["SUB_SUBMISSION_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["SUB_SUBMISSION_TYPE_DESC"].ToString().Trim();
                                item.SubSubmissionTypeDescF = dr["SUB_SUBMISSION_TYPE_DESC_F"] == DBNull.Value ? string.Empty : dr["SUB_SUBMISSION_TYPE_DESC_F"].ToString().Trim();
                                item.FlagAttestedMonograph = dr["FLAG_ATTESTED_MONOGRAPH"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FLAG_ATTESTED_MONOGRAPH"]);
                                item.FlagPrimaryName = dr["FLAG_PRIMARY_NAME"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FLAG_PRIMARY_NAME"]);
                                item.FlagProductStatus = dr["FLAG_PRODUCT_STATUS"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FLAG_PRODUCT_STATUS"]);

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
                                item.SubmissionId = dr["SUBMISSION_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SUBMISSION_ID"]);
                                item.CompanyNameId = dr["COMPANY_NAME_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["COMPANY_NAME_ID"]);
                                item.CompanyName = dr["COMPANY_NAME"] == DBNull.Value ? string.Empty : dr["COMPANY_NAME"].ToString().Trim();
                                item.DosageFormE = dr["DOSAGE_FORM"] == DBNull.Value ? string.Empty : dr["DOSAGE_FORM"].ToString().Trim();
                                item.DosageFormF = dr["DOSAGE_FORM_F"] == DBNull.Value ? string.Empty : dr["DOSAGE_FORM_F"].ToString().Trim();
                                item.LicenceNumber = dr["LICENCE_NUMBER"] == DBNull.Value ? string.Empty : dr["LICENCE_NUMBER"].ToString().Trim();
                                item.ProductName = dr["PRODUCT_NAME"] == DBNull.Value ? string.Empty : dr["PRODUCT_NAME"].ToString().Trim();
                                item.SubSubmissionTypeDescE = dr["SUB_SUBMISSION_TYPE_DESC"] == DBNull.Value ? string.Empty : dr["SUB_SUBMISSION_TYPE_DESC"].ToString().Trim();
                                item.SubSubmissionTypeDescF = dr["SUB_SUBMISSION_TYPE_DESC_F"] == DBNull.Value ? string.Empty : dr["SUB_SUBMISSION_TYPE_DESC_F"].ToString().Trim();
                                item.FlagAttestedMonograph = dr["FLAG_ATTESTED_MONOGRAPH"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FLAG_ATTESTED_MONOGRAPH"]);
                                item.FlagPrimaryName = dr["FLAG_PRIMARY_NAME"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FLAG_PRIMARY_NAME"]);
                                item.FlagProductStatus = dr["FLAG_PRODUCT_STATUS"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FLAG_PRODUCT_STATUS"]);

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
    }
}
