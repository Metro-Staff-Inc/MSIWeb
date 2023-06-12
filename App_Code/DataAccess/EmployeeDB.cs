using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.Practices.EnterpriseLibrary.Data;
using MSI.Web.MSINet.BusinessEntities;
using System.Collections;
using WebServicesLocation;
using System.IO;

/// <summary>
/// Summary description for ClientDB
/// </summary>
namespace MSI.Web.MSINet.DataAccess
{
    public class EmployeeDB
    {
        public EmployeeDB()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private DataAccessHelper _dbHelper = new DataAccessHelper();

        public String ValidateUser(string userName, string pwd)
        {
            String retStr = "";
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.ValidateUser);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@user_name", DbType.String, userName);
            dbSvc.AddInParameter(cw, "@password", DbType.String, pwd);

            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    if (dr.Read())
                    {
                        retStr = dr.GetString(dr.GetOrdinal("user_id"));
                    }
                }
                catch (Exception drEx)
                {
                    throw (drEx);
                }
                finally
                {
                    if (dr != null && !dr.IsClosed)
                        dr.Close();
                    dr.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                cw.Dispose();
            }
            return retStr;
        }

        public int SetEmployee(EmployeeHistory empHist, int action)
        {
            DbCommand cw;
            int count = 1;

            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.SetEmployee);
            cw.CommandTimeout = 120;    //10 minutes

            if (empHist.SSN == null || empHist.SSN.Length == 0)
            {
                empHist.SSN = "899999999";
            }
            dbSvc.AddInParameter(cw, "@ssn", DbType.String, empHist.SSN);
            dbSvc.AddInParameter(cw, "@aident", DbType.String, empHist.AIdentNumber);
            dbSvc.AddInParameter(cw, "@LastName", DbType.String, empHist.LastName);
            dbSvc.AddInParameter(cw, "@FirstName", DbType.String, empHist.FirstName);
            
            dbSvc.AddInParameter(cw, "@addr1", DbType.String, empHist.Addr1);
            dbSvc.AddInParameter(cw, "@addr2", DbType.String, empHist.Addr2);
            dbSvc.AddInParameter(cw, "@city", DbType.String, empHist.City);
            dbSvc.AddInParameter(cw, "@state", DbType.String, empHist.State);
            dbSvc.AddInParameter(cw, "@middleName", DbType.String, empHist.MiddleName);
            dbSvc.AddInParameter(cw, "@zip", DbType.String, empHist.Zip);
            //dbSvc.AddInParameter(cw, "@zip4", DbType.String, empHist.Zip4);
            dbSvc.AddInParameter(cw, "@phoneArea", DbType.String, empHist.PhoneAreaCode);
            dbSvc.AddInParameter(cw, "@phonePrefix", DbType.String, empHist.PhonePrefix);
            dbSvc.AddInParameter(cw, "@phoneLast4", DbType.String, empHist.PhoneLast4);
            dbSvc.AddInParameter(cw, "@roleID", DbType.Int32, 1);
            dbSvc.AddInParameter(cw, "@email", DbType.String, empHist.Email);
            //dbSvc.AddInParameter(cw, "@created", DbType.String, empHist.Created.ToString());
            //dbSvc.AddInParameter(cw, "@lastUpdated", DbType.String, DateTime.Now.ToString());

            if (empHist.Update)
                dbSvc.AddInParameter(cw, "@action", DbType.Int32, 2); //update
            else
                dbSvc.AddInParameter(cw, "@action", DbType.Int32, 1); //create

            if (empHist.UpdatedBy.Length > 0)
                dbSvc.AddInParameter(cw, "@updatedBy", DbType.String, empHist.UpdatedBy);
            
            try
            {
                dbSvc.ExecuteNonQuery(cw);
            }
            catch (Exception ex)
            {
                count = -100;
                Console.WriteLine(ex.ToString());
                //throw ex;
            }
            finally
            {
                cw.Dispose();
            }
            return count;
        }

        public int UpdateEmployeeHours(int employeeHoursHeaderId, String aidentNumber, String userName, String notes, double hours1, double hours2, double hours3, double hours4,
                                            double hours5, double hours6, double hours7)
        {
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.UpdateEmployeeHours);
            cw.CommandTimeout = 120;    //10 minutes

            dbSvc.AddInParameter(cw, "@aidentNumber", DbType.String, aidentNumber);
            dbSvc.AddInParameter(cw, "@createdBy", DbType.String, userName);
            dbSvc.AddInParameter(cw, "@employeeHoursHeaderID", DbType.Int32, employeeHoursHeaderId);
            dbSvc.AddInParameter(cw, "@notes", DbType.String, notes);
            dbSvc.AddInParameter(cw, "@hours1", DbType.Decimal, hours1);
            dbSvc.AddInParameter(cw, "@hours2", DbType.Decimal, hours2);
            dbSvc.AddInParameter(cw, "@hours3", DbType.Decimal, hours3);
            dbSvc.AddInParameter(cw, "@hours4", DbType.Decimal, hours4);
            dbSvc.AddInParameter(cw, "@hours5", DbType.Decimal, hours5);
            dbSvc.AddInParameter(cw, "@hours6", DbType.Decimal, hours6);
            dbSvc.AddInParameter(cw, "@hours7", DbType.Decimal, hours7);
            try
            {
                dbSvc.ExecuteNonQuery(cw);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cw.Dispose();
            }
            return 0;
        }

        public int CreateETicketRoster(int clientID, int locID, int shiftType, int deptID, DateTime weekEnd)
        {
            int ETicketID = 0;
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.CreateEmployeeHoursHeader);
            cw.CommandTimeout = 120;    //10 minutes

            dbSvc.AddInParameter(cw, "@clientID", DbType.Int32, clientID);
            dbSvc.AddInParameter(cw, "@locationID", DbType.Int32, locID);
            dbSvc.AddInParameter(cw, "@departmentID", DbType.Int32, deptID);
            dbSvc.AddInParameter(cw, "@shiftType", DbType.Int32, shiftType);
            dbSvc.AddInParameter(cw, "@weekEnding", DbType.Date, weekEnd.Date);
            dbSvc.AddOutParameter(cw, "@employeeHoursHeaderID", DbType.Int32, 4);
            try
            {
                dbSvc.ExecuteNonQuery(cw);
                ETicketID = Convert.ToInt32(cw.Parameters["@employeeHoursHeaderID"].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cw.Dispose();
            }

            return ETicketID;
        }

        public List<String> GetFPTList()
        {
            string pathName = @"C:/inetpub/wwwroot/Dropbox/shiftdata/";
            string fileFPT = "*" + ".fpt";
            string[] fpt = Directory.GetFiles(pathName, fileFPT);
            for (int i = 0; i < fpt.Length; i++)
            {
                int st = fpt[i].LastIndexOf("/")+1;
                int end = Math.Max(fpt[i].LastIndexOf("L"), fpt[i].LastIndexOf("R"));
                if (end == -1)
                    end = fpt[i].LastIndexOf(".");
                fpt[i] = fpt[i].Substring(st, end - st); 
            }
            HashSet<String> hPrints = new HashSet<string>(fpt);

            List<String> fpts = new List<String>(hPrints);
            return fpts;
        }

        public List<String> GetXMLList()
        {
            string pathName = @"C:/inetpub/wwwroot/Dropbox/shiftdata/";
            string fileXML = "*" + ".xml";
            string[] fpt = Directory.GetFiles(pathName, fileXML);
            for (int i = 0; i < fpt.Length; i++)
            {
                int st = fpt[i].LastIndexOf("/") + 1;
                int end = fpt[i].LastIndexOf(".");
                fpt[i] = fpt[i].Substring(st, end - st);
            }
            HashSet<String> hPrints = new HashSet<string>(fpt);

            List<String> fpts = new List<String>(hPrints);
            return fpts;
        }

        public List<String> GetFSTList()
        {
            string pathName = @"C:/inetpub/wwwroot/Dropbox/shiftdata/";
            string fileFST = "*" + ".fst";
            string[] fpt = Directory.GetFiles(pathName, fileFST);
            for (int i = 0; i < fpt.Length; i++)
            {
                int st = fpt[i].LastIndexOf("/") + 1;
                int end = Math.Max(fpt[i].LastIndexOf("L"), fpt[i].LastIndexOf("R"));
                if (end == -1)
                    end = fpt[i].LastIndexOf(".");
                fpt[i] = fpt[i].Substring(st, end - st);
            }
            HashSet<String> hPrints = new HashSet<string>(fpt);

            List<String> fpts = new List<String>(hPrints);
            return fpts;
        }
        public FingerprintInfo GetFingerprints()
        {
            FingerprintInfo finfo = new FingerprintInfo();
            finfo.fingerprints = new List<Fingerprint>();

            string pathName = @"C:/inetpub/wwwroot/Dropbox/shiftdata/";
            
            string fileFST = "*" + ".fst";
            string fileFPT = "*" + ".fpt";
            string fileXML = "*" + ".xml";

            string[] fst = Directory.GetFiles(pathName, fileFST);
            string[] fpt = Directory.GetFiles(pathName, fileFPT);
            string[] xml = Directory.GetFiles(pathName, fileXML);
            List<string> filesXML = new List<String>();
            List<string> filesFST = new List<String>();
            List<string> filesFPT = new List<String>();
            
            for( int i=0; i<fst.Length; i++ )
            {
                int s = fst[i].LastIndexOf("/") + 1;
                int e = fst[i].LastIndexOf(".") - 2;
                string id = fst[i].Substring(s, e - s);
                if (!filesFST.Contains(id))
                    filesFST.Add(id);
            }
            for (int i = 0; i < fpt.Length; i++)
            {
                int s = fpt[i].LastIndexOf("/") + 1;
                int e = fpt[i].LastIndexOf(".") - 2;
                string id = fpt[i].Substring(s, e - s);
                if( !filesFPT.Contains(id) )
                    filesFPT.Add(id);
            }

            for (int i = 0; i < xml.Length; i++)
            {
                int s = xml[i].LastIndexOf("/") + 1;
                int e = xml[i].LastIndexOf(".");
                string id = xml[i].Substring(s, e - s);
                filesXML.Add(xml[i].Substring(s, e - s));
            }

            filesFST.Sort();
            filesFPT.Sort();
            filesXML.Sort();

            while (filesXML.Count > 0)
            {

                string id = filesXML[0];
                Fingerprint f;
                f = new Fingerprint();
                f.info = 100;
                f.aident = id;
                filesXML.RemoveAt(0);
                /* clear out matching fpt and fst members */
                for (int i = 0; i < filesFPT.Count; i++)
                {
                    if (filesFPT[i].Equals(id))
                    {
                        f.info += 100;
                        finfo.fptCount++;
                        filesFPT.RemoveAt(i);
                        i--;
                    }
                }
                for (int i = 0; i < filesFST.Count; i++)
                {
                    if (filesFST[i].Equals(id))
                    {
                        f.info ++;
                        finfo.fstCount++;
                        filesFST.RemoveAt(i);
                        i--;
                    }
                }
                finfo.fingerprints.Add(f);
            }
            return finfo;
        }

        public EmployeeHours GetETicketRoster(int clientID, int locID, DateTime weekEnd, int deptID, int shiftType )
        {
            //RosterInfo info = new RosterInfo();
            DateTime weekStart = weekEnd.Add(new TimeSpan(-6, 0, 0, 0));
            EmployeeHours info = new EmployeeHours();
            info.Employees = new List<EmployeeHoursItem>();
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetETicketRosters);
            cw.CommandTimeout = 120;    //10 minutes

            dbSvc.AddInParameter(cw, "@clientID", DbType.Int32, clientID);
            dbSvc.AddInParameter(cw, "@locationID", DbType.Int32, locID);

            weekEnd.Add(new TimeSpan(23, 59, 59));  /* put to the end of the day */
            dbSvc.AddInParameter(cw, "@weekEnding", DbType.DateTime, weekEnd);
            dbSvc.AddInParameter(cw, "@departmentID", DbType.Int32, deptID);
            dbSvc.AddInParameter(cw, "@shiftType", DbType.Int32, shiftType);

            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    int count = 0;
                    while (dr.Read())
                    {
                        if (count == 0)
                        {
                            info.EmployeeHoursHeaderId = dr.GetInt32(dr.GetOrdinal("employee_hours_header_id"));
                            info.ApprovalDate = dr.GetDateTime(dr.GetOrdinal("approval_dt"));
                            info.ApprovedBy = dr.GetString(dr.GetOrdinal("approved_by"));
                            info.Notes = dr.GetString(dr.GetOrdinal("ticket_notes"));
                            info.Submitted = !info.ApprovalDate.Equals(new DateTime(1999, 1, 1));
                            info.weekEnding = dr.GetDateTime(dr.GetOrdinal("week_ending"));
                            info.Supervisor = dr.GetString(dr.GetOrdinal("supervisor"));
                            info.DefaultStart = dr.GetString(dr.GetOrdinal("shift_start_time"));
                            info.DefaultEnd = dr.GetString(dr.GetOrdinal("shift_end_time"));
                            info.Multiplier = dr.GetDecimal(dr.GetOrdinal("multiplier"));
                            info.ShiftMapping = dr.GetInt32(dr.GetOrdinal("shift_mapping"));

                            count++;
                        }
                        
                        String aident = dr.GetString(dr.GetOrdinal("aident_number"));
                        String badge = dr.GetString(dr.GetOrdinal("badge_number"));
                        /* employee already here? */
                        EmployeeHoursItem r = null;
                        Boolean add = true;
                        foreach (EmployeeHoursItem e in info.Employees)
                        {
                            if (e.AidentNumber.Equals(aident))
                            {
                                r = e;
                                add = false;
                            }
                        }
                        if (r == null) // not in list...
                        {
                            r = new EmployeeHoursItem();
                            r.AidentNumber = aident;
                            r.BadgeNumber = badge;
                            r.PayRate = dr.GetDecimal(dr.GetOrdinal("pay_rate"));
                            r.DailyHours = new decimal[7]; // list of seven decimals for each day's hours worked
                            r.FirstName = dr.GetString(dr.GetOrdinal("first_name")).ToUpper();
                            r.LastName = dr.GetString(dr.GetOrdinal("last_name")).ToUpper();
                            r.Office = dr.GetString(dr.GetOrdinal("office_cd"));
                            r.StartDate = new List<DateTime>();
                            r.EndDate = new List<DateTime>();
                            /* now get the hours */
                            r.DailyHours[0] += dr.GetDecimal(dr.GetOrdinal("hours1"));
                            r.DailyHours[1] += dr.GetDecimal(dr.GetOrdinal("hours2"));
                            r.DailyHours[2] += dr.GetDecimal(dr.GetOrdinal("hours3"));
                            r.DailyHours[3] += dr.GetDecimal(dr.GetOrdinal("hours4"));
                            r.DailyHours[4] += dr.GetDecimal(dr.GetOrdinal("hours5"));
                            r.DailyHours[5] += dr.GetDecimal(dr.GetOrdinal("hours6"));
                            r.DailyHours[6] += dr.GetDecimal(dr.GetOrdinal("hours7"));
                        }
                        r.StartDate.Add(dr.GetDateTime(dr.GetOrdinal("effective_dt")).AddMinutes(1));
                        r.EndDate.Add(dr.GetDateTime(dr.GetOrdinal("expiration_dt")).AddMinutes(-1));
                        string start = dr.GetString(dr.GetOrdinal("start_time"));
                        if (start.Equals("-1") || start.Length == 0)
                            start = dr.GetString(dr.GetOrdinal("shift_start_time"));
                        string end = dr.GetString(dr.GetOrdinal("end_time"));
                        if (end.Equals("-1") || end.Length == 0)
                            end = dr.GetString(dr.GetOrdinal("shift_end_time"));
                        r.ShiftStart = start;
                        r.ShiftEnd = end;
                        r.RosterID = dr.GetInt32(dr.GetOrdinal("client_roster_id"));
                        r.Notes = dr.GetString(dr.GetOrdinal("notes"));
                        DateTime dStart = dr.GetDateTime(dr.GetOrdinal("effective_dt"));
                        DateTime dEnd = dr.GetDateTime(dr.GetOrdinal("expiration_dt"));
                        if (dEnd.Year - dStart.Year > 2)
                        {
                            r.Temp = false;
                        }
                        else
                        {
                            r.Temp = true;
                        }
                        if( add )
                            info.Employees.Add(r);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (dr != null)
                    {
                        if (!dr.IsClosed)
                            dr.Close();

                        dr.Dispose();
                    }
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
            return info;
        }

        public EmployeeHours GetEmployeeHours(int employeeHoursId)
        {
            EmployeeHours employeeHours = new EmployeeHours();
            Database dbSvc = DatabaseFactory.CreateDatabase();
            DbCommand cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetEmployeeHours);
            cw.CommandTimeout = 120;    //10 minutes
            //dbSvc.AddInParameter(cw, "@LastName", DbType.String, LastName);
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        EmployeeHoursItem ehi = new EmployeeHoursItem();
                    }
                }
                catch (Exception drEx)
                {
                    throw drEx;
                }
                finally
                {
                    if (dr != null && !dr.IsClosed)
                        dr.Close();

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
            return employeeHours;
        }

        public List<EmployeeStatus> GetEmployeeStatus(DateTime date, int clientID)
        {
            List<EmployeeStatus> list = new List<EmployeeStatus>();
            Database dbSvc = DatabaseFactory.CreateDatabase();
            DbCommand cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetEmployeeStatusReport);
            cw.CommandTimeout = 120;    //10 minutes
            date = date.Date;
            dbSvc.AddInParameter(cw, "@startDt", DbType.DateTime, date);
            dbSvc.AddInParameter(cw, "@endDt", DbType.DateTime, date.AddDays(1));
            dbSvc.AddInParameter(cw, "@clientId", DbType.Int32, clientID);
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        EmployeeStatus es = new EmployeeStatus();
                        es.EmployeeID = dr.GetString(dr.GetOrdinal("aident_number"));
                        es.LastName = dr.GetString(dr.GetOrdinal("last_name"));
                        es.FirstName = dr.GetString(dr.GetOrdinal("first_name"));
                        es.ShiftName = dr.GetString(dr.GetOrdinal("shift_name"));
                        es.EmployeeID = dr.GetString(dr.GetOrdinal("aident_number"));
                        es.DepartmentName = dr.GetString(dr.GetOrdinal("department_name"));
                        es.ClientName = dr.GetString(dr.GetOrdinal("client_name"));
                        es.Day = dr.GetDateTime(dr.GetOrdinal("work_day"));
                        list.Add(es);
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                    throw (e);
                }
            }
            catch(Exception e)
            {
                throw e;
            }
            return list;
        }
        public RecruitPool GetEmployeeByNamePhoneBlast(string lastName, string firstName)
        {
            RecruitPool rp = new RecruitPool();
            rp.RecruitPoolCollection = new List<RecruitPoolItem>();

            Database dbSvc = DatabaseFactory.CreateDatabase();
            DbCommand cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.RosterProgram);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@LastName", DbType.String, lastName);
            dbSvc.AddInParameter(cw, "@FirstName", DbType.String, firstName);
            dbSvc.AddInParameter(cw, "@action", DbType.Int32, 1);

            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        RecruitPoolItem rpi = new RecruitPoolItem();
                        //LOAD THE Employee
                        rpi.FirstName = dr.GetString(dr.GetOrdinal("first_name"));
                        rpi.LastName = dr.GetString(dr.GetOrdinal("last_name"));
                        rpi.BadgeNumber = dr.GetString(dr.GetOrdinal("aident_number"));
                        rpi.PhoneNum = "(" + dr.GetString(dr.GetOrdinal("employee_phone_area")) + ")" + dr.GetString(dr.GetOrdinal("employee_phone_prefix")) +
                                "-" + dr.GetString(dr.GetOrdinal("employee_phone_last4"));
                        rpi.Notes = dr.GetString(dr.GetOrdinal("data"));
                        rpi.DnrReason = "active";
                        rp.RecruitPoolCollection.Add(rpi);
                    }
                }
                catch (Exception drEx)
                {
                    throw drEx;
                }
                finally
                {
                    if (dr != null && !dr.IsClosed)
                        dr.Close();

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

            return rp;
        }

        public string GetEmployeeByName(string name)
        {
            string retStr = "<table id='tblAvailableEmployees'><thead><tr><th>ID</th><th>Name</th><th>Add</th></tr></thead>";
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.RosterProgram);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@LastName", DbType.String, name);
            dbSvc.AddInParameter(cw, "@action", DbType.Int32, 1);
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        string aident = dr.GetString(dr.GetOrdinal("aident_number"));
                        retStr = retStr + "<tr id='" + aident + "'><td>" + aident
                            + "</td><td>" + dr.GetString(dr.GetOrdinal("last_name")) + ", " + dr.GetString(dr.GetOrdinal("first_name")) + "</td>"
                            + "<td><input type='button' onclick='addToRoster(\"" + aident + "\")' value='Add'/></td></tr>";
                    }
                }
                catch (Exception drEx)
                {
                    Console.WriteLine(drEx);
                }
                finally
                {
                    if (dr != null && !dr.IsClosed)
                        dr.Close();
                    dr.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                cw.Dispose();
            }
            retStr = retStr + "</table>";
            return retStr;
        }
        public string GetEmployeeByDate_Pics(string date, string clientID)
        {
            string retStr = "<table id='tblAvailableEmployees'><thead><tr><th>ID</th><th>Name</th><th>View</th></tr></thead>";
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            string[] ids = clientID.Split('-');
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.RosterProgram);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@numberofdays", DbType.String, date);
            dbSvc.AddInParameter(cw, "@clientid", DbType.String, ids[0]);
            if (ids.Length > 1)
                dbSvc.AddInParameter(cw, "@clientid2", DbType.String, ids[1]);
            else
                dbSvc.AddInParameter(cw, "@clientid2", DbType.String, "0");
            dbSvc.AddInParameter(cw, "@action", DbType.Int32, 1);
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        string aident = dr.GetString(dr.GetOrdinal("aident_number"));
                        retStr = retStr + "<tr id='" + aident + "'><td>" + aident
                            + "</td><td>" + dr.GetString(dr.GetOrdinal("last_name")) + ", " + dr.GetString(dr.GetOrdinal("first_name")) + "</td>"
                            + "<td><input type='button' onclick='getPics(\"" + aident + "\")' value='View'/></td></tr>";
                    }
                }
                catch (Exception drEx)
                {
                    Console.WriteLine(drEx);
                }
                finally
                {
                    if (dr != null && !dr.IsClosed)
                        dr.Close();
                    dr.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                cw.Dispose();
            }
            return retStr;
        }
        public string GetEmployeeByName_Pics(string name)
        {
            string retStr = "<table id='tblAvailableEmployees'><thead><tr><th>ID</th><th>Name</th><th>View</th></tr></thead>";
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.RosterProgram);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@LastName", DbType.String, name);
            dbSvc.AddInParameter(cw, "@action", DbType.Int32, 1);
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        string aident = dr.GetString(dr.GetOrdinal("aident_number"));
                        retStr = retStr + "<tr id='" + aident + "'><td>" + aident
                            + "</td><td>" + dr.GetString(dr.GetOrdinal("last_name")) + ", " + dr.GetString(dr.GetOrdinal("first_name")) + "</td>"
                            + "<td><input type='button' onclick='getPics(\"" + aident + "\")' value='View'/></td></tr>";
                    }
                }
                catch (Exception drEx)
                {
                    Console.WriteLine(drEx);
                }
                finally
                {
                    if (dr != null && !dr.IsClosed)
                        dr.Close();
                    dr.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                cw.Dispose();
            }
            retStr = retStr + "</table>";
            return retStr;
        }
        public EmployeeHistory GetEmployeeByAident(EmployeeLookup lookup)
        {
            EmployeeHistory returnEmp = new EmployeeHistory();
            returnEmp.AIdentNumber = lookup.AidentNumber;
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetEmployeeByAident);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@aidentNumber", DbType.String, lookup.AidentNumber);
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        //LOAD THE Employee
                        returnEmp.FirstName = dr.GetString(dr.GetOrdinal("first_name"));
                        returnEmp.LastName = dr.GetString(dr.GetOrdinal("last_name"));
                        returnEmp.EmployeeID = dr.GetInt32(dr.GetOrdinal("employee_id"));
                        /* extra info returned for fingerprint enrollment */
                        int i;
                        i = dr.GetOrdinal("employee_phone_area");
                        if (i >= 0)
                            returnEmp.PhoneAreaCode = dr.GetString(i);
                        i = dr.GetOrdinal("employee_phone_prefix");
                        if (i >= 0)
                            returnEmp.PhonePrefix = dr.GetString(i);
                        i = dr.GetOrdinal("employee_phone_last4");
                        if (i >= 0)
                            returnEmp.PhoneLast4 = dr.GetString(i);
                        returnEmp.Addr1 = dr.GetString(dr.GetOrdinal("employee_address_1"));
                        returnEmp.Addr2 = dr.GetString(dr.GetOrdinal("employee_address_2"));
                        returnEmp.City = dr.GetString(dr.GetOrdinal("employee_city"));
                        returnEmp.State = dr.GetString(dr.GetOrdinal("employee_state"));
                        returnEmp.Zip = dr.GetString(dr.GetOrdinal("employee_zip"));
                    }
                }
                catch (Exception drEx)
                {
                    throw drEx;
                }
                finally
                {
                    if (dr != null && !dr.IsClosed)
                        dr.Close();

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

            return returnEmp;
        }
    }
}