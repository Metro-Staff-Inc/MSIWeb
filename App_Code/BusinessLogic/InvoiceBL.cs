using System;
using System.Data.Common;
using System.Collections.Generic;
using MSI.Web.MSINet.BusinessEntities;
using MSI.Web.MSINet.Common;
using MSI.Web.MSINet.DataAccess;
using System.Security.Principal;
using Microsoft.Practices.EnterpriseLibrary.Data;

/// <summary>
/// Summary description for ClientBL
/// </summary>
namespace MSI.Web.MSINet.BusinessLogic
{
    public class InvoiceBL
    {
        public InvoiceBL()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        private InvoiceDB invoiceDB = new InvoiceDB();
        private HelperFunctions helperFunctions = new HelperFunctions();

        public Invoice GetInvoice(Invoice invoiceIn, IPrincipal userPrincipal)
        {
            Invoice invoiceOut = invoiceDB.GetInvoiceHeader(invoiceIn, userPrincipal);

            return invoiceDB.GetInvoiceDetail(invoiceOut, userPrincipal);
        }

        public ClientPayOverride UpdateClientPayOverride(ClientPayOverride clientPayOverride, IPrincipal userPrincipal, string user)
        {
            ClientPayOverride ret = clientPayOverride;

            return invoiceDB.UpdateClientPayOverride(clientPayOverride, userPrincipal, user);
        }

        public DepartmentPayRate UpdateDepartmentPayRate(DepartmentPayRate departmentPay, IPrincipal userPrincipal)
        {
            return invoiceDB.UpdateDepartmentPayRate(departmentPay, userPrincipal);
        }

        public DepartmentJobCode UpdateDepartmentJobCode(DepartmentJobCode departmentJobCode, IPrincipal userPrincipal)
        {
            return invoiceDB.UpdateDepartmentJobCode(departmentJobCode, userPrincipal);
        }

        public ClientPayOverride AddClientPayOverride(ClientPayOverride clientPayOverride, IPrincipal userPrincipal, string user)
        {
            ClientPayOverride ret = clientPayOverride;

            return invoiceDB.AddClientPayOverride(clientPayOverride, userPrincipal, user);
        }

        public InvoiceReturn CreateInvoice(Invoice invoiceIn, IPrincipal userPrincipal)
        {
            InvoiceReturn ret = new InvoiceReturn();
            Invoice invoiceOut = new Invoice();
            Database dbSvc = DatabaseFactory.CreateDatabase();            
                        
            //use one connection
            using (DbConnection conn = dbSvc.CreateConnection())
            {
                
                conn.Open();
                DbTransaction transaction = conn.BeginTransaction();

                try
                {
                    //create the header
                    invoiceOut = invoiceDB.CreateInvoiceHeader(invoiceIn, dbSvc, conn, transaction, userPrincipal);
                    //create the detail records
                    List<InvoiceDetail> detailOut = new List<InvoiceDetail>();
                    foreach (InvoiceDetail detail in invoiceOut.DetailInfo)
                    {
                        detail.InvoiceHeaderId = invoiceOut.InvoiceHeaderId;
                        detailOut.Add(invoiceDB.CreateInvoiceDetail(detail, dbSvc, conn, transaction, userPrincipal));
                    }

                    invoiceOut.DetailInfo = detailOut;

                    transaction.Commit();
                    ret.IsSuccess = true;
                    ret.ExceptionInfo = null;
                    ret.SystemErrorCode = string.Empty;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ret.IsSuccess = false;
                    ret.ExceptionInfo = ex;
                }
                finally
                {
                    ret.InvoiceInfo = invoiceOut;
                }
            }

            return ret;
        }
    }
}