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
using System.IO;
using System.Text;
using System.Net.Mime;
using System.Net.Mail;

/// <summary>
/// Summary description for ClientDB
/// </summary>
/// 
namespace MSI.Web.MSINet.DataAccess
{
    public class RosterDB
    {
        public RosterDB()
        {
        }
        
        public String SetGMPInfo(String csvStr)
        {
            String retStr = "";
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();

            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.SetGMPInfo);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@inpStr", DbType.String, csvStr);
            try
            {
                int rows = dbSvc.ExecuteNonQuery(cw);
                retStr = rows + " records added/updated.";
            }
            catch (IOException ioe)
            {
                throw (ioe);
            }
            finally
            {
                cw.Dispose();
            }
            return retStr;
        }

        public List<GMPInfo> GetGMPInfo(DateTime start, DateTime end, int clientId)
        {
            List<GMPInfo> list = new List<GMPInfo>();
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetGMPInfo);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@startDate", DbType.DateTime,start);
            dbSvc.AddInParameter(cw, "@endDate", DbType.DateTime, end);
            dbSvc.AddInParameter(cw, "@clientId", DbType.Int32, clientId);
            
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        GMPInfo gmp = new GMPInfo();
                        gmp.Aident = dr.GetString(dr.GetOrdinal("aident_number"));
                        gmp.Name = dr.GetString(dr.GetOrdinal("last_name")) + "," + 
                                            dr.GetString(dr.GetOrdinal("first_name"));
                        gmp.HireDate = dr.GetDateTime(dr.GetOrdinal("hire_date"));
                        gmp.GMPDate = dr.GetDateTime(dr.GetOrdinal("gmp_date"));
                        gmp.ShiftType = dr.GetInt32(dr.GetOrdinal("shift"));
                        int pos = dr.GetOrdinal("nla");
                        if( !dr.IsDBNull(pos) )
                            gmp.NLA = dr.GetBoolean(dr.GetOrdinal("nla"));
                        else
                            gmp.NLA = false;
                        if (gmp.ShiftType == 1) gmp.ShiftType = 100;
                        else if (gmp.ShiftType == 2) gmp.ShiftType = 200;
                        else gmp.ShiftType = 300;
                        list.Add(gmp);
                    }
                }
                catch (IOException ioe)
                {
                    throw (ioe);
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
            catch (IOException ioe)
            {
                throw (ioe);
            }
            finally
            {
                cw.Dispose();
            }
            return list;
        }

        public List<SkillDescription> GetSkillDescriptions()
        {
            List<SkillDescription> description = new List<SkillDescription>();
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetSkillDescriptions);
            cw.CommandTimeout = 120;    //10 minutes
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        SkillDescription sd = new SkillDescription();
                        sd.Description = dr.GetString(dr.GetOrdinal("description"));
                        sd.Id = dr.GetInt32(dr.GetOrdinal("skill_description_id"));
                        description.Add(sd);
                    }
                }
                catch (Exception ex)
                {
                    throw (ex);
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
                
            }
            finally
            {
                cw.Dispose();
            }
            return description;
        }

        public List<LocationDDL> GetClientLocations(string userID, string clientID)
        {
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.ClientLocations);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@clientID", DbType.Int32, Convert.ToInt32(clientID));
            List<LocationDDL> locations = new List<LocationDDL>();
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        LocationDDL location = new LocationDDL();
                        location.LocationID = (dr.GetInt32(dr.GetOrdinal("location_id")));
                        location.LocationName = (dr.GetString(dr.GetOrdinal("location_name")));
                        location.Address1 = (dr.GetString(dr.GetOrdinal("location_address_1")));
                        location.Address2 = (dr.GetString(dr.GetOrdinal("location_address_2")));
                        location.City = (dr.GetString(dr.GetOrdinal("location_city")));
                        location.State = (dr.GetString(dr.GetOrdinal("location_state")));
                        location.Zip = (dr.GetString(dr.GetOrdinal("location_zip")));
                        location.PhonePrefix = (dr.GetString(dr.GetOrdinal("location_phone_prefix")));
                        location.PhoneAreaCode = (dr.GetString(dr.GetOrdinal("location_phone_area")));
                        location.PhoneLast4 = (dr.GetString(dr.GetOrdinal("location_phone_last4")));
                        locations.Add(location);
                    }
                }
                catch( Exception ex )
                {
                    
                    throw (ex);
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
            catch( Exception ex )
            {
                
                throw (ex);
            }
            finally
            {
                cw.Dispose();
            }
            return locations;
        }

        public List<String> GetClientEmails(int clientID)
        {
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.ClientEmails);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@clientId", DbType.Int32, clientID);
            List<String> emailList = new List<String>();
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        emailList.Add(dr.GetString(dr.GetOrdinal("email")));
                    }
                }
                catch( Exception ex )
                {
                    
                    throw (ex);
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
            catch( Exception ex )
            {
                
                throw (ex);
            }
            finally
            {
                cw.Dispose();
            }
            return emailList;
        }

        public String EmailSuncast(int clientID, int tempsOnly, int deptID, int shiftType, int shiftID, DateTime date)
        {
            DbCommand cw;
            int employeeCount = 0;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetEmployeeRostersByShift);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@clientID", DbType.Int32, clientID);
            dbSvc.AddInParameter(cw, "@tempsOnly", DbType.Int32, tempsOnly);
            dbSvc.AddInParameter(cw, "@departmentID", DbType.Int32, deptID);
            dbSvc.AddInParameter(cw, "@shiftType", DbType.Int32, shiftType);
            dbSvc.AddInParameter(cw, "@date", DbType.DateTime, date);
            string emailBody = "";
            string dateTime = date.ToString("yyyyMMdd");
            string missingIDs = "";
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        String eB = "";
                        eB += dateTime + "\t";
                        eB += shiftType + "\t";
                        eB += dr.GetDecimal(dr.GetOrdinal("temp_number")) + "\t";
                        eB += dr.GetString(dr.GetOrdinal("badge_number")) + "\t";
                        eB += dr.GetString(dr.GetOrdinal("last_name")) + "\t";
                        eB += dr.GetString(dr.GetOrdinal("first_name")) + "\t";
                        eB += dr.GetString(dr.GetOrdinal("paycode")) + "\t";
                        eB += dr.GetInt32(dr.GetOrdinal("substitute_for")) + "\r\n";
                        if( dr.GetDecimal(dr.GetOrdinal("temp_number") ) > 0)
                        {
                            emailBody += eB;
                            employeeCount++;
                        }
                        else
                        {
                            missingIDs += dr.GetString(dr.GetOrdinal("last_name")) + ", " +
                                    dr.GetString(dr.GetOrdinal("first_name")) + ", " +
                                        dr.GetString(dr.GetOrdinal("badge_number")) + "   </br>";
                        }
                    }
                }
                catch (Exception drEx)
                {
                    Console.WriteLine(drEx.Message.ToString());
                }
                finally
                {
                    if (emailBody.Length > 0)
                    {
                        DateTime dt = DateTime.Now;
                        List<String> list = GetClientEmails(8);
                        SendEMail999(clientID, "LaborRoster-MSI-" + date.ToString("yyyyMMdd") + "-" + dt.ToString("hhmmss") + "", emailBody, list);
                    }
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
                
            }
            finally
            {
                cw.Dispose();
            }
            if (missingIDs.Length > 0)
                return employeeCount + " employees on roster.<br/>Removed (no suncast id): " + missingIDs;
            else return employeeCount + " employees on roster.";
        }

        public List<EmployeeNotes> GetEmployeeNotes(String id)
        {
            List<EmployeeNotes> employeeNotes = new List<EmployeeNotes>();
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetEmployeeNotes);
            cw.CommandTimeout = 120;    //10 minutes

            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        EmployeeNotes en = new EmployeeNotes();
                        /*
                        office.OfficeName = dr.GetString(dr.GetOrdinal("office_name"));
                        office.OfficeID = dr.GetInt32(dr.GetOrdinal("office_id"));
                        office.PhoneArea = dr.GetString(dr.GetOrdinal("office_phone_area"));
                        office.PhoneLast4 = dr.GetString(dr.GetOrdinal("office_phone_last4"));
                        office.PhonePrefix = dr.GetString(dr.GetOrdinal("office_phone_prefix"));
                        office.AddressLine1 = dr.GetString(dr.GetOrdinal("office_address_1"));
                        office.AddressLine2 = dr.GetString(dr.GetOrdinal("office_address_2"));
                        office.City = dr.GetString(dr.GetOrdinal("office_city"));
                        office.State = dr.GetString(dr.GetOrdinal("office_state"));
                        office.Zip = dr.GetString(dr.GetOrdinal("office_zip"));
                        office.ZipPlus4 = dr.GetString(dr.GetOrdinal("office_zip_4"));
                        office.OfficeCode = dr.GetString(dr.GetOrdinal("office_cd"));
                        offices.Add(office);*/
                    }
                }
                catch (Exception drEx)
                {
                    
                    throw drEx;
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
            return employeeNotes;
        }

        public List<Office> GetFullDispatchOffices()
        {
            List<Office> offices = new List<Office>();
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetDispatchOffices);
            cw.CommandTimeout = 120;    //10 minutes

            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        Office office = new Office();
                        office.OfficeName = dr.GetString(dr.GetOrdinal("office_name"));
                        office.OfficeID = dr.GetInt32(dr.GetOrdinal("office_id"));
                        office.PhoneArea = dr.GetString(dr.GetOrdinal("office_phone_area"));
                        office.PhoneLast4 = dr.GetString(dr.GetOrdinal("office_phone_last4"));
                        office.PhonePrefix = dr.GetString(dr.GetOrdinal("office_phone_prefix"));
                        office.AddressLine1 = dr.GetString(dr.GetOrdinal("office_address_1"));
                        office.AddressLine2 = dr.GetString(dr.GetOrdinal("office_address_2"));
                        office.City = dr.GetString(dr.GetOrdinal("office_city"));
                        office.State = dr.GetString(dr.GetOrdinal("office_state"));
                        office.Zip = dr.GetString(dr.GetOrdinal("office_zip"));
                        office.ZipPlus4 = dr.GetString(dr.GetOrdinal("office_zip_4"));
                        office.OfficeCode = dr.GetString(dr.GetOrdinal("office_cd"));
                        offices.Add(office);
                    }
                }
                catch (Exception drEx)
                {
                    
                    throw drEx;
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
            return offices;
        }
        public string GetDispatchOffices()
        {
            string options = "<option value='-1'>Dispatch Office</option>";
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.RosterProgram);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@action", DbType.Int32, 10); // 10 means - get list of dispatch offices
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        string office_name = dr.GetString(dr.GetOrdinal("office_name"));
                        string office_id = dr.GetString(dr.GetOrdinal("office_id"));
                        options += "<option value='" + office_id + "'>" + office_name + "</option>";
                    }
                }
                catch (Exception drEx)
                {
                    
                    options += "<option>" + drEx.ToString() + "</option>";
                    //throw ex;
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
                
                options += "<option>" + ex.ToString() + "</option>";
                //table += ex.ToString();
                //throw ex;
            }
            finally
            {
                cw.Dispose();
            }
            return options;
        }
        public String checkClientTempNumber(int clientID, string aident)
        {
            String retStr = "";
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetClientTempNumberForAident);
            cw.CommandTimeout = 60;    // 1 minute
            dbSvc.AddInParameter(cw, "@clientID", DbType.Int32, clientID);
            dbSvc.AddInParameter(cw, "@aidentID", DbType.String, aident);
            dbSvc.AddInParameter(cw, "@lastUpdatedBy", DbType.String, "ClientRoster");
            dbSvc.AddOutParameter(cw, "@clientTempNumber", DbType.String, 80);
            try
            {
                dbSvc.ExecuteNonQuery(cw);
                retStr = (String)dbSvc.GetParameterValue(cw, "@clientTempNumber");
            }
            catch(Exception e)
            {
                retStr = "Error with Client ID: " + e.ToString();
            }
            finally
            {
                cw.Dispose();
            }
            return retStr;
        }

        public string CreateRoster(int clientID, int locationID, string aident, int shiftType, int dept, string office, DateTime startDate, DateTime endDate)
        {
            String retStr = "";
            if ( clientID == 8 )  //suncast
            {
                retStr = checkClientTempNumber(clientID, aident);
                if( retStr.StartsWith("Error"))
                {
                    return retStr;
                }
            }
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.RosterProgram);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@clientid", DbType.Int32, clientID);
            dbSvc.AddInParameter(cw, "@locationid", DbType.Int32, locationID);
            dbSvc.AddInParameter(cw, "@aident", DbType.String, aident);
            dbSvc.AddInParameter(cw, "@shifttype", DbType.Int32, shiftType);
            dbSvc.AddInParameter(cw, "@departmentid", DbType.Int32, dept);
            dbSvc.AddInParameter(cw, "@officeid", DbType.String, office);
            dbSvc.AddInParameter(cw, "@startdate", DbType.DateTime, startDate);
            dbSvc.AddInParameter(cw, "@enddate", DbType.DateTime, endDate);
            dbSvc.AddInParameter(cw, "@action", DbType.Int32, 8);

            dbSvc.AddOutParameter(cw, "@clientrosterid", DbType.Int32, 4);
            try
            {
                dbSvc.ExecuteNonQuery(cw);
                int rs = (int)dbSvc.GetParameterValue(cw, "@clientrosterid");
                retStr = "" + rs;
            }
            catch (Exception ex)
            {
                
                retStr = ex.ToString();
            }
            finally
            {
                cw.Dispose();
            }

            return retStr;
        }

        public string GetAvailableEmployees( int clientID, int numDays, int dpt, int shift, int loc  )
        {
            string table = "<table id='tblAvailableEmployees'><thead><tr><th>ID#</th><th>Name</th><th>Add</th></tr></thead>";
            DbCommand cw;
            int count = 0;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.RosterProgram);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@clientid", DbType.Int32, clientID);
            dbSvc.AddInParameter(cw, "@numberofdays", DbType.Int32, numDays );
            dbSvc.AddInParameter(cw, "@departmentid", DbType.Int32, dpt);
            dbSvc.AddInParameter(cw, "@shifttype", DbType.Int32, shift);
            dbSvc.AddInParameter(cw, "@locationid", DbType.Int32, loc);
            dbSvc.AddInParameter(cw, "@action", DbType.Int32, 199); // 2 means - get rosters of working employees
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        string addClass="";
                        string aident = dr.GetString(dr.GetOrdinal("aident_number"));
                        if (aident.Length == 0)
                            addClass = "class='noID'";
                        count++;
                        table += "<tr id='" + aident + "'" + addClass + "><td>" + aident + "</td>";
                        table += "<td>" + dr.GetString(dr.GetOrdinal("last_name")) + ", " +
                            dr.GetString(dr.GetOrdinal("first_name")) + "</td>" + 
                        "<td><input type='button' onclick='addToRoster(\"" + aident + "\")' value='Add'/></td></tr>";
                    }
                    if (count == 0)
                        table += "<tr><td>No Available Employees!</td></tr>";
                }
                catch (Exception drEx)
                {
                    
                    table += drEx.ToString();
                    //throw ex;
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
                
                table += ex.ToString();
                //throw ex;
            }
            finally
            {
                cw.Dispose();
            }
            table += "</table>";
            return table;
        }
        public String PunchesOutside(int clientRosterID, DateTime startDate, DateTime endDate, string startTime, string endTime)
        {
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();

            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.PunchesOutside);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@client_roster_id", DbType.Int32, clientRosterID);
            dbSvc.AddInParameter(cw, "@startDate", DbType.DateTime, startDate);
            dbSvc.AddInParameter(cw, "@endDate", DbType.DateTime, endDate);
            dbSvc.AddInParameter(cw, "@shiftStartTime", DbType.String, startTime);
            dbSvc.AddInParameter(cw, "@shiftEndTime", DbType.String, startTime);
            dbSvc.AddInParameter(cw, "@action", DbType.Int32, 71); //get punches outside of date range
            int count = 0;
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    if (dr.Read())
                    {
                        count++;
                    }
                }
                catch (Exception ex)
                {
                    
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
                
                //if (ex.ToString().Contains("overflow"))
                //    count = 1;
            }
            finally
            {
                cw.Dispose();
            }
            return count.ToString();
        }

        public String PunchesExist(string clientRosterID)
        {
            //Boolean exist = false;

            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.RosterProgram);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@client_roster_id", DbType.Int32, Convert.ToInt32(clientRosterID));
            dbSvc.AddInParameter(cw, "@action", DbType.Int32, 7); // 7 = get punches;
            String punchDate = "";
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    if (dr.Read())
                    {
                        bool deleted = dr.GetBoolean(dr.GetOrdinal("deleted"));
                        if( !deleted )
                        {
                            punchDate = dr.GetDateTime(dr.GetOrdinal("punch_dt")).ToString();
                        }
                        //exist = true;
                    }
                }
                catch (Exception ex)
                {
                    
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
                
            }
            finally
            {
                cw.Dispose();
            }
            return punchDate;
        }

        string[] shiftName = { "0", "1st shift", "2nd shift", "3rd shift", "Shift A", "Shift B", "Shift C", "Shift D" };
        public string GetRosters(int clientID, string aident, DateTime startDate, DateTime endDate)
        {
            //int count = 0;
            string rosters = "";
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.RosterProgram);
            cw.CommandTimeout = 120;    //10 minutes

            dbSvc.AddInParameter(cw, "@aident", DbType.String, aident);
            dbSvc.AddInParameter(cw, "@clientid", DbType.Int32, clientID);
            dbSvc.AddInParameter(cw, "@action", DbType.Int32, 20);
            dbSvc.AddInParameter(cw, "@startDate", DbType.DateTime, startDate);
            dbSvc.AddInParameter(cw, "@endDate", DbType.DateTime, endDate);
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        bool deleted = dr.GetBoolean(dr.GetOrdinal("deleted"));
                        if( !deleted )
                            rosters = dr.GetString(dr.GetOrdinal("department_name")) + ", " + shiftName[dr.GetInt32(dr.GetOrdinal("shift_type"))];
                    }
                }
                catch (Exception drEx)
                {
                    
                    throw drEx;
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
                
            }
            finally
            {
                cw.Dispose();
            }
            return rosters;
        }


        public string GetRosters(int clientRosterID)
        {
            int count = 0;
            List<String> crIDs = new List<String>();
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.RosterProgram);
            cw.CommandTimeout = 120;    //10 minutes

            dbSvc.AddInParameter(cw, "@client_roster_id", DbType.Int32, clientRosterID);
            dbSvc.AddInParameter(cw, "@action", DbType.Int32, 2);

            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        //String r = dr.GetString(dr.GetOrdinal("client_roster_id"));
                        //crIDs.Add(r);
                        count++;
                    }
                }
                catch (Exception drEx)
                {
                    
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
                
            }
            finally
            {
                cw.Dispose();
            }
            return count.ToString();
        }

        public List<String> GetAllRosterIDs(string badge, string clientID, string shiftID)
        {
            List<String> crIDs = new List<String>();
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.RosterProgram);
            cw.CommandTimeout = 120;    //10 minutes

            dbSvc.AddInParameter(cw, "@aident", DbType.String, badge);
            dbSvc.AddInParameter(cw, "@action", DbType.Int32, 28);
            dbSvc.AddInParameter(cw, "@clientid", DbType.Int32, Convert.ToInt32(clientID));
            dbSvc.AddInParameter(cw, "@shiftid", DbType.Int32, Convert.ToInt32(shiftID));
            dbSvc.AddInParameter(cw, "@startdate", DbType.DateTime, DateTime.Now);
            dbSvc.AddInParameter(cw, "@enddate", DbType.DateTime, DateTime.Now);


            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        String r = dr.GetInt32(dr.GetOrdinal("client_roster_id")).ToString();
                        crIDs.Add(r);
                    }
                }
                catch (Exception drEx)
                {
                    
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
                
            }
            finally
            {
                cw.Dispose();
            }
            return crIDs;
        }


        public string DnrTrim(string id, string clientID, string shiftID)
        {
            String retString = "TRIM THE ROSTER!!!";
            List<String> rosters = GetAllRosterIDs(id, clientID, shiftID);
            foreach (string roster in rosters)
            {
                TrimRoster(Convert.ToInt32(roster));
            }
            return retString;
        }


        public string TrimRoster(int rosterID)
        {
            string retStr = "";
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.RosterProgram);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@client_roster_id", DbType.Int32, rosterID);
            //dbSvc.AddInParameter(cw, "@enddate", DbType.DateTime, dt);
            dbSvc.AddInParameter(cw, "@action", DbType.Int32, 91); // 9_1 means - update endpoint of a particular roster 
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        retStr += dr.GetString(dr.GetOrdinal("return_dates"));
                    }
                }
                catch (Exception drEx)
                {
                    retStr += drEx.ToString();
                    
                }
                finally
                {
                    if( dr != null )
                    {
                        if (!dr.IsClosed)
                            dr.Close();

                        dr.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                retStr = ex.ToString();
                
            }
            finally
            {
                cw.Dispose();
            }
            return retStr;
        }
        public List<Roster> GetEmployeeRosters(String id, DateTime startDate, DateTime endDate, String clientID)
        {
            List<Roster> list = new List<Roster>();
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetEmployeeRosters);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@id", DbType.String, id);
            dbSvc.AddInParameter(cw, "@startDate", DbType.DateTime, startDate);
            dbSvc.AddInParameter(cw, "@endDate", DbType.DateTime, endDate);
            dbSvc.AddInParameter(cw, "@clientId", DbType.String, clientID);
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        Roster r = new Roster();
                        r.ID = dr.GetInt32(dr.GetOrdinal("client_roster_id")).ToString();
                        r.StartDate = dr.GetDateTime(dr.GetOrdinal("effective_dt"));
                        r.EndDate = dr.GetDateTime(dr.GetOrdinal("expiration_dt"));
                        r.ClientID = dr.GetInt32(dr.GetOrdinal("client_id")).ToString();
                        r.ClientName = dr.GetString(dr.GetOrdinal("client_name"));
                        r.Shift = dr.GetInt32(dr.GetOrdinal("shift_type"));
                        r.ShiftStart = dr.GetString(dr.GetOrdinal("shift_start_time"));
                        r.ShiftEnd = dr.GetString(dr.GetOrdinal("shift_end_time"));
                        r.SubstituteFor = 0;// dr.GetInt32(dr.GetOrdinal("substitute_for"));
                        r.Dept = dr.GetInt32(dr.GetOrdinal("department_id"));
                        list.Add(r);
                    }
                }
                catch (Exception e)
                {
                    throw (e);
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
            return list;
        }

        public List<Roster> GetPrintRosters(int clientID, DateTime startDate, DateTime endDate, int dept, int shift)
        {
            int count = 0;
            List<Roster> list = new List<Roster>();
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.RosterProgram);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@clientid", DbType.Int32, clientID);
            dbSvc.AddInParameter(cw, "@startdate", DbType.Date, startDate.Date);
            dbSvc.AddInParameter(cw, "@enddate", DbType.Date, endDate.Date);
            dbSvc.AddInParameter(cw, "@departmentid", DbType.Int32, dept);
            dbSvc.AddInParameter(cw, "@shifttype", DbType.Int32, shift);
            dbSvc.AddInParameter(cw, "@action", DbType.Int32, 2); // 2 means - get rosters of working employees
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        count++;
                        Roster r = new Roster();
                        r.ShiftStart = dr.GetString(dr.GetOrdinal("start_time"));
                        if (r.ShiftStart.Equals("-1") || r.ShiftStart.Length == 0)
                            r.ShiftStart = dr.GetString(dr.GetOrdinal("shift_start_time"));

                        r.ShiftEnd = dr.GetString(dr.GetOrdinal("end_time"));
                        if (r.ShiftEnd.Equals("-1") || r.ShiftEnd.Length == 0)
                            r.ShiftEnd = dr.GetString(dr.GetOrdinal("shift_end_time"));
                        
                        r.StartDate = dr.GetDateTime(dr.GetOrdinal("effective_dt")).AddMinutes(1);
                        r.EndDate = dr.GetDateTime(dr.GetOrdinal("expiration_dt")).AddMinutes(-1);
                        
                        r.ID = dr.GetString(dr.GetOrdinal("office_cd")) + 
                        dr.GetString(dr.GetOrdinal("aident_number"));

                        r.LastName = dr.GetString(dr.GetOrdinal("last_name"));
                        r.FirstName = dr.GetString(dr.GetOrdinal("first_name"));
                        list.Add(r);
                    }
                }
                catch (Exception drEx)
                {
                    
                    throw drEx;
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
            return list;
        }

        public RosterInfo GetRostersWithLocation(int clientID, DateTime startDate, int loc, int dept, int shift)
        {
            RosterInfo info = new RosterInfo();
            info.rosters = new List<Roster>();
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.RosterProgram);
            cw.CommandTimeout = 120;    //10 minutes

            dbSvc.AddInParameter(cw, "@clientid", DbType.Int32, clientID);
            dbSvc.AddInParameter(cw, "@locationid", DbType.Int32, loc);
            dbSvc.AddInParameter(cw, "@startdate", DbType.Date, startDate.Date);
            dbSvc.AddInParameter(cw, "@enddate", DbType.Date, startDate.Date);
            dbSvc.AddInParameter(cw, "@departmentid", DbType.Int32, dept);
            dbSvc.AddInParameter(cw, "@shifttype", DbType.Int32, shift);
            dbSvc.AddInParameter(cw, "@action", DbType.Int32, 27); // 27 means - get rosters of working employees
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
                            info.ShiftStart = dr.GetString(dr.GetOrdinal("shift_start_time"));
                            info.ShiftEnd = dr.GetString(dr.GetOrdinal("shift_end_time"));
                            info.TrackingStart = dr.GetString(dr.GetOrdinal("tracking_start"));
                            info.TrackingEnd = dr.GetString(dr.GetOrdinal("tracking_end"));
                            info.ClientName = dr.GetString(dr.GetOrdinal("client_name"));
                            info.DeptName = dr.GetString(dr.GetOrdinal("department_name"));
                            count++;
                        }
                        Roster r = new Roster();
                        r.ClientID = "" + clientID;
                        r.ID = dr.GetString(dr.GetOrdinal("aident_number"));
                        r.SubID = "" + dr.GetDecimal(dr.GetOrdinal("sub_id"));
                        r.FirstName = dr.GetString(dr.GetOrdinal("first_name")).ToUpper();
                        r.LastName = dr.GetString(dr.GetOrdinal("last_name")).ToUpper();
                        r.Office = dr.GetString(dr.GetOrdinal("office_cd"));
                        r.StartDate = dr.GetDateTime(dr.GetOrdinal("effective_dt")).AddMinutes(1);
                        r.EndDate = dr.GetDateTime(dr.GetOrdinal("expiration_dt")).AddMinutes(-1);
                        string start = dr.GetString(dr.GetOrdinal("start_time"));
                        if (start.Equals("-1") || start.Length == 0)
                            start = dr.GetString(dr.GetOrdinal("shift_start_time"));
                        string end = dr.GetString(dr.GetOrdinal("end_time"));
                        if (end.Equals("-1") || end.Length == 0)
                            end = dr.GetString(dr.GetOrdinal("shift_end_time"));
                        r.ShiftStart = start;
                        r.ShiftEnd = end;
                        r.RosterID = dr.GetInt32(dr.GetOrdinal("client_roster_id"));
                        info.rosters.Add(r);
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
            catch(Exception ex)
            {
                
                throw ex;
            }
            finally
            {
                cw.Dispose();
            }
            return info;
        }

        public string GetRosters(int clientID, int loc, DateTime startDate, DateTime endDate, int dept, int shift)
        {
            string table = "<table id='tblCurRosters'><thead><tr><th>ID</th><th>Name</th><th>Start</th><th>End</th><th>Time</th><th>Subs</th><th>Upd</th><th>Del</th></tr></thead>";
            if( clientID != 8 )
                table = "<table id='tblCurRosters'><thead><tr><th>ID</th><th>Name</th><th>Start</th><th>End</th><th>Time</th><th>Upd</th><th>Del</th></tr></thead>";
            DbCommand cw;
            int count = 0;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.RosterProgram);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@clientid", DbType.Int32, clientID);
            dbSvc.AddInParameter(cw, "@startdate", DbType.Date, startDate.Date);
            dbSvc.AddInParameter(cw, "@enddate", DbType.Date, endDate.Date);
            dbSvc.AddInParameter(cw, "@departmentid", DbType.Int32, dept);
            dbSvc.AddInParameter(cw, "@shifttype", DbType.Int32, shift);
            dbSvc.AddInParameter(cw, "@locationid", DbType.Int32, loc);
            dbSvc.AddInParameter(cw, "@action", DbType.Int32, 2); // 2 means - get rosters of working employees
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        count++;
                        Roster r = new Roster();
                        string start = dr.GetString(dr.GetOrdinal("start_time"));
                        if (start.Equals("-1") || start.Length == 0 )
                            start = dr.GetString(dr.GetOrdinal("shift_start_time"));
                        string end = dr.GetString(dr.GetOrdinal("end_time"));
                        if (end.Equals("-1") || end.Length == 0 )
                            end = dr.GetString(dr.GetOrdinal("shift_end_time"));
                        DateTime rosterStart = dr.GetDateTime(dr.GetOrdinal("effective_dt"));
                        DateTime rosterEnd = dr.GetDateTime(dr.GetOrdinal("expiration_dt"));
                        rosterStart = rosterStart.AddMinutes(1);
                        rosterEnd = rosterEnd.AddMinutes(-1);
                        table += "<tr id='" + dr.GetInt32(dr.GetOrdinal("client_roster_id")) + "'><td>T" + dr.GetString(dr.GetOrdinal("office_cd")) + dr.GetString(dr.GetOrdinal("aident_number")) + "</td>";
                        table += "<td>" + dr.GetString(dr.GetOrdinal("last_name")) + ",<br/>" + 
                            dr.GetString(dr.GetOrdinal("first_name")) + "</td>";
                        table += "<td><input class='stDt' size='10' type='text' value='" + rosterStart.Date.ToString("MM/dd/yyyy") +  "'/></td>";
                        table += "<td><input class='endDt' size='10' type='text' value='" + rosterEnd.Date.ToString("MM/dd/yyyy") + "'</td>";
                        //table += "<td> s:<input type='text' size='4' value='" + start + "'/><br/>e:<input type='text' size='4' value='" + end + "'/></td>";
                        string startHours = start.Substring(0, 2);
                        string startMins = start.Substring(3, 2);
                        string startAMPM = "AM";
                        if (Convert.ToInt32(startHours) >= 12)
                        {
                            if( Convert.ToInt32(startHours) > 12 )
                                startHours = Convert.ToString(Convert.ToInt32(startHours) - 12);
                            if( startHours.Length < 2 )
                                startHours = '0' + startHours;
                            startAMPM = "PM";
                        }
                        else if (Convert.ToInt32(startHours) == 0)
                        {
                            startHours = "12";
                        }
                        string endHours = end.Substring(0, 2);
                        string endMins = end.Substring(3, 2);
                        string endAMPM = "AM";
                        if (Convert.ToInt32(endHours) >= 12)
                        {
                            if( Convert.ToInt32(endHours) > 12 )
                                endHours = Convert.ToString(Convert.ToInt32(endHours) - 12);
                            if (endHours.Length < 2)
                                endHours = '0' + endHours;
                            endAMPM = "PM";
                        }
                        else if (Convert.ToInt32(endHours) == 0)
                        {
                            endHours = "12";
                        }


                        table += "<td>s:<select></select>:";
                        table += "<select></select>";
                        table += "<select></select><br/>";
                        table += "e:<select></select>:";
                        table += "<select></select>";
                        table += "<select></select>";
                        table += "<input type='hidden' value='" + startHours+startMins+startAMPM + "'></input><input type='hidden' value='" + endHours+endMins+endAMPM + "'></input></td>";
                        if (clientID == 8)
                            table += "<td><input type='text' id='sub_" + dr.GetInt32(dr.GetOrdinal("client_roster_id")).ToString() + "' value='" + dr.GetString(dr.GetOrdinal("substitute_for")) +"' size='6' ></td>";
                        table += "<td><input type='button' id='upd_" + dr.GetInt32(dr.GetOrdinal("client_roster_id")).ToString() + "' value='Upd' onclick='updateLine(" + dr.GetInt32(dr.GetOrdinal("client_roster_id")).ToString() + ")'></td>";
                        table += "<td><input type='button' id='del_" + dr.GetInt32(dr.GetOrdinal("client_roster_id")).ToString() + "' value='Del' onclick='deleteLine(" + dr.GetInt32(dr.GetOrdinal("client_roster_id")).ToString() + ")'></td></tr>";
                    }
                    if (count == 0)
                    {
                        table += "<tbody></tbody>";
                        //table += "<tr><td>No Rosters for given time period!!</td></tr>";
                    }
                }
                catch (Exception drEx)
                {
                    table += drEx.ToString();
                    
                    //throw ex;
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
                
                table += ex.ToString();
                //throw ex;
            }
            finally
            {
                cw.Dispose();
            }
            table += "</table>";
            return table;
        }

        private void SendEMail999(int clientID, string title, string body, List<String> emailAddrs)
        {
            System.Net.Mail.MailMessage message = null;
            try
            {
                System.Net.Mail.SmtpClient mailClient = new System.Net.Mail.SmtpClient();
                mailClient.UseDefaultCredentials = false;
                //System.Net.ICredentialsByHost credentials = new System.Net.NetworkCredential("msibilling", "metroStaffInc60!23");
                System.Net.ICredentialsByHost credentials = new System.Net.NetworkCredential("MetroStaffInc@gmail.com", /*"Metro2019"*/ "isouhrfseigmwnzs");
                mailClient.Credentials = credentials;
                mailClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                //mailClient.Host = "mail.msistaff.com";
                //mailClient.Host = "smtp.msistaff.com";
                //mailClient.Port = 5190; //587
                mailClient.Port = 587;
                message = new System.Net.Mail.MailMessage();
                //message.From = new System.Net.Mail.MailAddress("eticket@msistaff.com");
                message.From = new System.Net.Mail.MailAddress("MetroStaffInc@gmail.com");
                mailClient.Host = "smtp.gmail.com";
                mailClient.EnableSsl = true;

                foreach (String addr in emailAddrs)
                {
                    switch (clientID)
                    {
                        case 8:
                            message.To.Add(addr);
                            break;
                    }
                }
                message.Subject = "[Labor Roster] [MSI]";
                message.Body = "";
                message.IsBodyHtml = false;
                using (System.IO.MemoryStream memoryStream = new MemoryStream())
                {
                    byte[] contentAsBytes = Encoding.UTF8.GetBytes(body);
                    memoryStream.Write(contentAsBytes, 0, contentAsBytes.Length);

                    // Set the position to the beginning of the stream.
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    // Create attachment
                    ContentType contentType = new ContentType();
                    contentType.MediaType = MediaTypeNames.Application.Octet;
                    contentType.Name = title + ".txt";
                    Attachment attachment = new Attachment(memoryStream, contentType);

                    // Add the attachment
                    message.Attachments.Add(attachment);

                    // Send Mail via SmtpClient
                    mailClient.Send(message);
                }
                //mailClient.Send(message);
            }
            catch (Exception ex)
            {
                
                Console.WriteLine(ex);
            }
            finally
            {
                if (message != null)
                {
                    message.Dispose();
                }
            }
        }
    }
}
//ex = {"The SMTP server requires a secure connection or the client was not authenticated. The server response was: 5.5.1 Authentication Required. Learn more at"}
