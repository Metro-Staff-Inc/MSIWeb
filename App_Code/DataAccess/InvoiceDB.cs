using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Collections;
using System.Data.Common;
using System.Web.Security;
using System.Security.Principal;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.Practices.EnterpriseLibrary.Data;
using MSI.Web.MSINet.BusinessEntities;
using MSI.Web.MSINet.Common;
/// <summary>
/// Summary description for EmployeePunchDB
/// </summary>
namespace MSI.Web.MSINet.DataAccess
{
    public class InvoiceDB
    {
        public InvoiceDB()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        
        private DataAccessHelper _dbHelper = new DataAccessHelper();
        private HelperFunctions _helper = new HelperFunctions();

        public Invoice GetInvoiceHeader(Invoice invoiceIn, IPrincipal userPrincipal)
        {
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();

            Invoice invoiceOut = new Invoice();

            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetInvoiceHeader);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@clientID", DbType.Int32, invoiceIn.ClientID);
            dbSvc.AddInParameter(cw, "@weekEndDate", DbType.DateTime, invoiceIn.WeekEndDate);
            dbSvc.AddOutParameter(cw, "@clientHoursApprovalId", DbType.Int32, 4);
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        //a work summary = same shift same date
                        invoiceOut.InvoiceHeaderId = dr.GetInt32(dr.GetOrdinal("client_invoice_header_id"));
                        invoiceOut.ClientID = dr.GetInt32(dr.GetOrdinal("client_id"));
                        invoiceOut.InvoiceDateTime = dr.GetDateTime(dr.GetOrdinal("invoice_dt"));
                        invoiceOut.InvoiceNumber = dr.GetString(dr.GetOrdinal("invoice_number"));
                        invoiceOut.WeekEndDate = dr.GetDateTime(dr.GetOrdinal("week_end_dt"));
                        invoiceOut.ClientApprovalId = dr.GetInt32(dr.GetOrdinal("client_hours_approval_id"));
                        invoiceOut.TotalDollars = dr.GetDecimal(dr.GetOrdinal("total_dollars"));
                        invoiceOut.StatusId = dr.GetInt32(dr.GetOrdinal("status_id"));
                        invoiceOut.CreatedDateTime = dr.GetDateTime(dr.GetOrdinal("created_dt"));
                        invoiceOut.CreatedBy = dr.GetString(dr.GetOrdinal("created_by"));
                        invoiceOut.LastUpdatedDateTime = dr.GetDateTime(dr.GetOrdinal("last_updated_dt"));
                        invoiceOut.LastUpdatedBy = dr.GetString(dr.GetOrdinal("last_updated_by"));
                    }
                }

                catch (Exception drEx)
                {
                    
                    throw drEx;
                }
                finally
                {
                    if (dr != null && !dr.IsClosed)
                    {
                        dr.Close();
                        object val = cw.Parameters["@clientHoursApprovalId"].Value;
                        if (val != null && val != DBNull.Value)
                        {
                            invoiceOut.ClientApprovalId = int.Parse(cw.Parameters["@clientHoursApprovalId"].Value.ToString());
                        }
                    }

                    dr.Dispose();
                }
            }
            catch (Exception ex)
            {
                invoiceOut.ClientApprovalId = 0;
                
                throw ex;
            }
            finally
            {
                cw.Dispose();
            }

            return invoiceOut;
        }

        public ClientPayOverride UpdateClientPayOverride(ClientPayOverride clientPayOverride, IPrincipal userPrincipal, string user)
        {
            ClientPayOverride ret = clientPayOverride;

            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();

            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.UpdateClientPayOverride);
            cw.CommandTimeout = 120;    //10 minutes

            dbSvc.AddInParameter(cw, "@clientPayOverrideId", DbType.Int32, clientPayOverride.ClientPayOverrideId);
            dbSvc.AddInParameter(cw, "@payRate", DbType.Currency, clientPayOverride.PayRate);
            dbSvc.AddInParameter(cw, "@effectiveDate", DbType.DateTime, clientPayOverride.EffectiveDate);
            dbSvc.AddInParameter(cw, "@expirationDate", DbType.DateTime, clientPayOverride.ExpirationDate);
            dbSvc.AddInParameter(cw, "@prevExpirationDate", DbType.DateTime, clientPayOverride.EffectiveDate.AddDays(-6));
            if (userPrincipal != null)
                dbSvc.AddInParameter(cw, "@userName", DbType.String, userPrincipal.Identity.Name);
            else
                dbSvc.AddInParameter(cw, "@userName", DbType.String, user);

            dbSvc.AddOutParameter(cw, "@newClientPayOverrideId", DbType.Int32, 4);
            try
            {
                dbSvc.ExecuteNonQuery(cw);
                ret.ClientPayOverrideId = Convert.ToInt32((dbSvc.GetParameterValue(cw, "@newClientPayOverrideId")));
            }
            catch (Exception ex)
            {
                
                ret.ClientPayOverrideId = -1;
                //throw ex;
                //ret.ClientPayOverrideId = 0;
            }
            finally
            {
                cw.Dispose();
            }

            return ret;
        }

        public DepartmentPayRate UpdateDepartmentPayRate(DepartmentPayRate departmentPay, IPrincipal userPrincipal)
        {
            DepartmentPayRate ret = departmentPay;

            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            //IDataReader dr = null;

            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.UpdateDepartmentPay);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@clientPayID", DbType.Int32, departmentPay.ClientPayId);
            dbSvc.AddInParameter(cw, "@clientId", DbType.Int32, departmentPay.ClientId);
            dbSvc.AddInParameter(cw, "@shiftType", DbType.Int32, departmentPay.ShiftType);
            dbSvc.AddInParameter(cw, "@departmentId", DbType.Int32, departmentPay.DepartmentInfo.DepartmentID);
            dbSvc.AddInParameter(cw, "@payRate", DbType.Currency, departmentPay.PayRate);
            dbSvc.AddInParameter(cw, "@effectiveDate", DbType.DateTime, departmentPay.EffectiveDate);
            dbSvc.AddInParameter(cw, "@expirationDate", DbType.DateTime, departmentPay.ExpirationDate);
            dbSvc.AddInParameter(cw, "@userName", DbType.String, userPrincipal.Identity.Name);
            dbSvc.AddOutParameter(cw, "@newClientPayID", DbType.Int32, 4);

            try
            {
                dbSvc.ExecuteNonQuery(cw);
                //ret.ClientPayId = Convert.ToInt32((dbSvc.GetParameterValue(cw, "@newClientPayID")));
            }
            catch (Exception ex)
            {
                
                throw ex;
                //ret.ClientPayId = 0;
            }
            finally
            {
                cw.Dispose();
            }

            return ret;
        }

        public DepartmentJobCode UpdateDepartmentJobCode(DepartmentJobCode departmentJobCode, IPrincipal userPrincipal)
        {
            DepartmentJobCode ret = departmentJobCode;

            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();

            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.UpdateDepartmentJobCode);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@clientJobCodeID", DbType.Int32, departmentJobCode.ClientJobCodeId);
            dbSvc.AddInParameter(cw, "@clientId", DbType.Int32, departmentJobCode.ClientId);
            dbSvc.AddInParameter(cw, "@shiftType", DbType.Int32, departmentJobCode.ShiftType);
            dbSvc.AddInParameter(cw, "@departmentId", DbType.Int32, departmentJobCode.DepartmentInfo.DepartmentID);
            dbSvc.AddInParameter(cw, "@jobCode", DbType.Currency, departmentJobCode.JobCode);
            dbSvc.AddInParameter(cw, "@effectiveDate", DbType.DateTime, departmentJobCode.EffectiveDate);
            dbSvc.AddInParameter(cw, "@expirationDate", DbType.DateTime, departmentJobCode.ExpirationDate);
            dbSvc.AddInParameter(cw, "@userName", DbType.String, userPrincipal.Identity.Name);
            dbSvc.AddOutParameter(cw, "@newClientJobCodeID", DbType.Int32, 4);

            try
            {
                dbSvc.ExecuteNonQuery(cw);
                ret.ClientJobCodeId = Convert.ToInt32((dbSvc.GetParameterValue(cw, "@newClientJobCodeID")));
            }
            catch (Exception ex)
            {
                
                throw ex;
                //ret.ClientJobCodeId = 0;
            }
            finally
            {
                cw.Dispose();
            }

            return ret;
        }

        public ClientPayOverride AddClientPayOverride(ClientPayOverride clientPayOverride, IPrincipal userPrincipal, string user)
        {
            ClientPayOverride ret = clientPayOverride;

            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();

            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.AddClientPayOverride);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@employeeId", DbType.Int32, clientPayOverride.EmployeeId);
            if (clientPayOverride.AidentNumber != null && clientPayOverride.AidentNumber.Length > 0)
                dbSvc.AddInParameter(cw, "@aidentNumber", DbType.String, clientPayOverride.AidentNumber);
            dbSvc.AddInParameter(cw, "@clientId", DbType.Int32, clientPayOverride.ClientId);
            dbSvc.AddInParameter(cw, "@shiftType", DbType.Int32, clientPayOverride.ShiftType);
            dbSvc.AddInParameter(cw, "@departmentId", DbType.Int32, clientPayOverride.DepartmentId);
            dbSvc.AddInParameter(cw, "@payRate", DbType.Currency, clientPayOverride.PayRate);
            dbSvc.AddInParameter(cw, "@effectiveDate", DbType.DateTime, clientPayOverride.EffectiveDate);
            dbSvc.AddInParameter(cw, "@expirationDate", DbType.DateTime, clientPayOverride.ExpirationDate);
            dbSvc.AddInParameter(cw, "@firstPunch", DbType.DateTime, clientPayOverride.firstPunch);
            if (userPrincipal == null)
                dbSvc.AddInParameter(cw, "@userName", DbType.String, user);
            else
                dbSvc.AddInParameter(cw, "@userName", DbType.String, userPrincipal.Identity.Name);
            dbSvc.AddOutParameter(cw, "@clientPayOverrideId", DbType.Int32, 4);

            try
            {
                dbSvc.ExecuteNonQuery(cw);
                ret.ClientPayOverrideId = Convert.ToInt32((dbSvc.GetParameterValue(cw, "@clientPayOverrideId")));
            }
            catch (Exception ex)
            {
                
                ret.ClientPayOverrideId = -1;
                //ret.ClientPayOverrideId = 0;
            }
            finally
            {
                cw.Dispose();
            }

            return ret;
        }

        public Invoice GetInvoiceDetail(Invoice invoiceIn, IPrincipal userPrincipal)
        {
            DbCommand cw;

            Database dbSvc = DatabaseFactory.CreateDatabase();

            Invoice invoiceOut = invoiceIn;
            InvoiceDetail detail = new InvoiceDetail();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetInvoiceDetail);
            cw.CommandTimeout = 120;
            dbSvc.AddInParameter(cw, "@invoiceHeaderId", DbType.Int32, invoiceIn.InvoiceHeaderId);
            dbSvc.AddInParameter(cw, "@clientId", DbType.Int32, invoiceIn.ClientID);
            decimal totalBilling = 0;

            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        detail = new InvoiceDetail();
                        detail.BadgeNumber = dr.GetString(dr.GetOrdinal("badge_number"));
                        detail.ClientRosterID = dr.GetInt32(dr.GetOrdinal("client_roster_id"));
                        detail.CreatedBy = dr.GetString(dr.GetOrdinal("created_by"));
                        detail.CreatedDateTime = dr.GetDateTime(dr.GetOrdinal("created_dt"));
                        detail.DepartmentInfo.DepartmentID = dr.GetInt32(dr.GetOrdinal("department_id"));
                        detail.DepartmentInfo.DepartmentName = dr.GetString(dr.GetOrdinal("department_name"));
                        detail.InvoiceDetailId = dr.GetInt32(dr.GetOrdinal("client_invoice_detail_id"));
                        detail.InvoiceHeaderId = dr.GetInt32(dr.GetOrdinal("client_invoice_header_id"));
                        detail.LastUpdatedBy = dr.GetString(dr.GetOrdinal("last_updated_by"));
                        detail.LastUpdatedDateTime = dr.GetDateTime(dr.GetOrdinal("last_updated_dt"));
                        detail.OTMultiplier = dr.GetDecimal(dr.GetOrdinal("ot_multiplier"));
                        detail.PayRate = dr.GetDecimal(dr.GetOrdinal("pay_rate"));
                        detail.RegularMultiplier = dr.GetDecimal(dr.GetOrdinal("regular_multiplier"));
                        detail.ShiftTypeInfo.ShiftTypeId = dr.GetInt32(dr.GetOrdinal("shift_type"));
                        detail.TotalBilling = dr.GetDecimal(dr.GetOrdinal("total_billing"));
                        detail.TotalOTHours = dr.GetDecimal(dr.GetOrdinal("total_ot_hours"));
                        detail.TotalRegularHours = dr.GetDecimal(dr.GetOrdinal("total_regular_hours"));
                        detail.LastName = dr.GetString(dr.GetOrdinal("last_name"));
                        detail.FirstName = dr.GetString(dr.GetOrdinal("first_name"));
                        detail.JobCode = dr.GetString(dr.GetOrdinal("job_code"));
                        detail.ShiftInfo.ShiftID = dr.GetInt32(dr.GetOrdinal("shift_id"));
                        detail.ShiftInfo.TempWorksMappingId = dr.GetInt32(dr.GetOrdinal("tempworks_id"));
                        detail.CostCenter = dr.GetInt32(dr.GetOrdinal("cost_center_id"));
                        detail.Bonus = Convert.ToDecimal(dr.GetDouble(dr.GetOrdinal("bonus")));
                        detail.Office = dr.GetString(dr.GetOrdinal("office_cd"));
                        detail.Office = detail.BadgeNumber.Substring(1, 1);
                        //if (detail.DepartmentInfo.DepartmentID == 843) //.Contains("94379")
                        //{
                        //  detail.RegularMultiplier = (decimal)1.24;
                        //  detail.OTMultiplier = (decimal)1.24;
                        //  detail.TotalBilling = detail.PayRate * 
                        //          (detail.TotalRegularHours * (decimal)1.24 + detail.TotalOTHours * (decimal)1.24 * (decimal)1.5); 
                        //}
                        detail.TotalBilling = detail.CalculateTotalBilling();
                        totalBilling += detail.TotalBilling;
                        invoiceOut.DetailInfo.Add(detail);
                    }
                    invoiceOut.TotalDollars = totalBilling;
                }

                catch (Exception drEx)
                {
                    
                    throw drEx;
                }
                finally
                {
                    if (dr != null && !dr.IsClosed)
                    {
                        dr.Close();
                    }

                    dr.Dispose();
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            finally
            {
                cw.Dispose();
            }

            return invoiceOut;
        }

        public Invoice CreateInvoiceHeader(Invoice invoiceIn, Database dbSvc, DbConnection conn, DbTransaction transaction, IPrincipal userPrincipal)
        {
            Invoice invoiceOut = invoiceIn;

            DbCommand cw;

            //generate the invoice number
            invoiceIn.InvoiceNumber = "";

            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.CreateInvoiceHeader);
            cw.CommandTimeout = 120;
            dbSvc.AddInParameter(cw, "@clientID", DbType.Int32, invoiceIn.ClientID);
            dbSvc.AddInParameter(cw, "@invoiceNumber", DbType.String, invoiceIn.InvoiceNumber);
            dbSvc.AddInParameter(cw, "@weekEndDate", DbType.DateTime, invoiceIn.WeekEndDate);
            dbSvc.AddInParameter(cw, "@clientHoursApprovalId", DbType.Int32, invoiceIn.ClientApprovalId);
            dbSvc.AddInParameter(cw, "@userName", DbType.String, userPrincipal.Identity.Name);
            dbSvc.AddOutParameter(cw, "@invoiceHeaderId", DbType.Int32, 4);

            try
            {
                int rows = dbSvc.ExecuteNonQuery(cw, transaction);
                invoiceOut.InvoiceHeaderId = int.Parse(cw.Parameters["@invoiceHeaderId"].Value.ToString());
            }
            catch (Exception ex)
            {
                invoiceOut.InvoiceHeaderId = 0;
                
                throw ex;
            }
            finally
            {
                cw.Dispose();
            }

            return invoiceOut;
        }

        public InvoiceDetail CreateInvoiceDetail(InvoiceDetail detailIn, Database dbSvc, DbConnection conn, DbTransaction transaction, IPrincipal userPrincipal)
        {
            InvoiceDetail detailOut = detailIn;

            DbCommand cw;

            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.CreateInvoiceDetail);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@invoiceHeaderId", DbType.Int32, detailIn.InvoiceHeaderId);
            dbSvc.AddInParameter(cw, "@clientRosterId", DbType.Int32, detailIn.ClientRosterID);
            dbSvc.AddInParameter(cw, "@BadgeNumber", DbType.String, detailIn.BadgeNumber);
            dbSvc.AddInParameter(cw, "@jobCode", DbType.String, detailIn.JobCode);
            dbSvc.AddInParameter(cw, "@totalRegularHours", DbType.Decimal, detailIn.TotalRegularHours);
            dbSvc.AddInParameter(cw, "@totalOTHours", DbType.Decimal, detailIn.TotalOTHours);
            dbSvc.AddInParameter(cw, "@payRate", DbType.Decimal, detailIn.PayRate);
            dbSvc.AddInParameter(cw, "@regularMultiplier", DbType.Decimal, detailIn.RegularMultiplier);
            dbSvc.AddInParameter(cw, "@otMultiplier", DbType.Decimal, detailIn.OTMultiplier);
            dbSvc.AddInParameter(cw, "@totalBilling", DbType.Decimal, detailIn.CalculateTotalBilling());
            dbSvc.AddInParameter(cw, "@userName", DbType.String, userPrincipal.Identity.Name);
            dbSvc.AddInParameter(cw, "@departmentId", DbType.Int32, detailIn.DepartmentInfo.DepartmentID);
            dbSvc.AddInParameter(cw, "@shiftId", DbType.Int32, detailIn.ShiftInfo.ShiftID);
            dbSvc.AddInParameter(cw, "@shiftType", DbType.Int32, detailIn.ShiftTypeInfo.ShiftTypeId);
            dbSvc.AddInParameter(cw, "@clientId", DbType.Int32, detailIn.ClientId);
            dbSvc.AddOutParameter(cw, "@invoiceDetailId", DbType.Int32, 4);

            try
            {
                int rows = dbSvc.ExecuteNonQuery(cw, transaction);
                detailOut.InvoiceDetailId = int.Parse(cw.Parameters["@invoiceDetailId"].Value.ToString());
            }
            catch (Exception ex)
            {
                detailOut.InvoiceDetailId = 0;
                
                throw ex;
            }
            finally
            {
                cw.Dispose();
            }

            return detailOut;
        }
    }
}