using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using MSI.Web.MSINet.BusinessEntities;

/// <summary>
/// Summary description for ClientDB
/// </summary>
/// 
namespace MSI.Web.MSINet.DataAccess
{
	public class PhoneBlastDB
	{
        
        public PhoneBlastDB()
        {

        }
        public String UpdateEmployeeNotes(String aident, String notes, String userId)
        {
            String retStr = "Updated!";
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.UpdateEmployeeNotes);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@aidentNumber", DbType.String, aident);
            dbSvc.AddInParameter(cw, "@notes", DbType.String, notes);
            dbSvc.AddInParameter(cw, "@userId", DbType.String, userId);
            try
            {
                dbSvc.ExecuteNonQuery(cw);
            }
            catch (Exception ex)
            {
                
                retStr = ex.ToString();
                //throw (ex);
            }
            finally
            {
                cw.Dispose();
            }
            return retStr;
        }
        public PBEmployeeContact GetEmployeeContact(String from)
        {
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetEmployeeContact);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@employeeNum", DbType.String, from);
            PBEmployeeContact contact = null;
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    if (dr.Read())
                    {
                        String id = dr.GetString(dr.GetOrdinal("aident_number"));
                        String firstName = dr.GetString(dr.GetOrdinal("first_name"));
                        String lastName = dr.GetString(dr.GetOrdinal("last_name"));
                        String msiCallback = dr.GetString(dr.GetOrdinal("msi_callback"));
                        String msiTextback = dr.GetString(dr.GetOrdinal("msi_textback"));
                        String empNum = dr.GetString(dr.GetOrdinal("employee_num"));
                        DateTime dt = dr.GetDateTime(dr.GetOrdinal("last_contact"));
                        contact = new PBEmployeeContact();
                        contact.FirstName = firstName;
                        contact.LastName = lastName;
                        contact.Id = id;
                        contact.MSIPhoneNum = msiCallback;
                        contact.MSITextNum = msiTextback;
                        contact.EmployeePhoneNum = empNum;
                        contact.ContactDate = dt;
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
                
                throw (ex);
            }
            finally
            {
                cw.Dispose();
            }
            return contact;

        }
        public void UpdateEmployeeContact(String to, String textback, String callback, DateTime dt)
        {
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.UpdateEmployeeContact);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@phoneTo", DbType.String, to);
            dbSvc.AddInParameter(cw, "@phoneFrom", DbType.String, callback);
            dbSvc.AddInParameter(cw, "@textFrom", DbType.String, textback);
            dbSvc.AddInParameter(cw, "@date", DbType.DateTime, dt);
            try
            {
                dbSvc.ExecuteNonQuery(cw);
            }
            catch (Exception ex)
            {
                
                throw (ex);
            }
            finally
            {
                cw.Dispose();
            }
        }

        public String GetWorkSchedule(String id)
        {
            String response = "";
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetWorkSchedule);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@id", DbType.String, id);
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    DateTime curDate = DateTime.Now;
                    bool first = true;
                    while (dr.Read())
                    {
                        if (first)
                        {
                            String name = dr.GetString(dr.GetOrdinal("first_name")) + " " + dr.GetString(dr.GetOrdinal("last_name"));
                            response = name + " - ";
                            first = false;
                        }
                        String client = dr.GetString(dr.GetOrdinal("client_name"));
                        String shiftDesc = dr.GetString(dr.GetOrdinal("shift_desc"));
                        String start = dr.GetString(dr.GetOrdinal("shiftStart"));
                        int hr = Convert.ToInt32(start.Substring(0,2));
                        int min = Convert.ToInt32(start.Substring(3,2));
                        String hrStr, minStr;
                        String ampm = "AM";
                        if( hr >= 12 )
                        {
                            ampm = "PM";
                            if( hr > 12 )
                                hr -= 12;
                        }
                        if( hr == 0 )
                            hr = 12;
                        if (hr < 10)
                            hrStr = "0" + hr;
                        else
                            hrStr = "" + hr;
                        if (min < 10)
                            minStr = "0" + min;
                        else
                            minStr = "" + min;

                        String startOut = hrStr + ":" + minStr + " " + ampm;

                        String end = dr.GetString(dr.GetOrdinal("shiftEnd"));
                        hr = Convert.ToInt32(end.Substring(0, 2));
                        min = Convert.ToInt32(end.Substring(3, 2));
                        ampm = "AM";
                        if (hr >= 12)
                        {
                            ampm = "PM";
                            if (hr > 12)
                                hr -= 12;
                        }
                        if (hr == 0)
                            hr = 12;

                        if (hr < 10)
                            hrStr = "0" + hr;
                        else
                            hrStr = "" + hr;
                        if (min < 10)
                            minStr = "0" + min;
                        else
                            minStr = "" + min;

                        String endOut = hrStr + ":" + minStr + " " + ampm;

                        DateTime effectiveDt = dr.GetDateTime(dr.GetOrdinal("effective_dt"));
                        DateTime expirationDt = dr.GetDateTime(dr.GetOrdinal("expiration_dt"));
                        response += shiftDesc + "\nat " + client + " - \n";
                        curDate = DateTime.Now.AddDays(-1);
                        for (int d = 0; d < 3; d++)
                        {
                            curDate = curDate.AddDays(1);
                            if (curDate >= effectiveDt && curDate <= expirationDt)
                            {
                                response += curDate.ToString("MM/dd/yyyy") + " from " + startOut + " to " + endOut + "\n";
                            }
                        }
                    }
                    if (first)
                    {
                        response = id + "\nNo work scheduled.  Please contact MSI dispatch office if you have any questions.";
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
                
                throw (ex);
            }
            finally
            {
                cw.Dispose();
            }
            return response;
        }


        public void PBListUpdate(String aident, int customListID, int action, string userID)
        {
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.UpdatePhoneBlastList);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@aident", DbType.String, aident);
            dbSvc.AddInParameter(cw, "@userID", DbType.String, userID);
            dbSvc.AddInParameter(cw, "@customListID", DbType.String, customListID);
            dbSvc.AddInParameter(cw, "@action", DbType.Int32, action);
            try
            {
                dbSvc.ExecuteNonQuery(cw);
            }
            catch (Exception ex)
            {
                
                throw (ex);
            }
            finally
            {
                cw.Dispose();
            }
        }

        public void SetPhoneBlast(string num, int response, int lang)
        {
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.SetPhoneBlast);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@phoneNum", DbType.String, num);
            dbSvc.AddInParameter(cw, "@language", DbType.Int32, lang);
            dbSvc.AddInParameter(cw, "@response", DbType.Int32, response);
            try
            {
                dbSvc.ExecuteNonQuery(cw);
            }
            catch (Exception ex)
            {
                
                throw (ex);
            }
            finally
            {
                cw.Dispose();
            }
        }
        public int GetPhoneBlastLanguage(string num)
        {
            int response = 0;
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetPhoneBlast);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@phoneNum", DbType.String, num);
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        response = (dr.GetInt32(dr.GetOrdinal("language")));
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
                
                throw (ex);
            }
            finally
            {
                cw.Dispose();
            }
            return response;
        }
        public void SetSkillList(string id, string skillDescriptionId)
        {
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.SetSkillList);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@skillDescriptionId", DbType.Int32, Convert.ToInt32(skillDescriptionId));
            dbSvc.AddInParameter(cw, "@aident", DbType.String, id);
            try
            {
                dbSvc.ExecuteNonQuery(cw);
            }
            catch (Exception ex)
            {
                
                throw (ex);
            }
            finally
            {
                cw.Dispose();
            }
        }

        public RecruitPool GetSkillList(string id)
        {
            int skillDescriptionId = Convert.ToInt32(id);
            RecruitPool rp = new RecruitPool();
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetSkillList);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@skillDescriptionId", DbType.Int32, skillDescriptionId);

            try
            {
                rp.RecruitPoolCollection = new List<RecruitPoolItem>();
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        RecruitPoolItem rpi = new RecruitPoolItem();
                        rpi.BadgeNumber = dr.GetString(dr.GetOrdinal("aident_number"));
                        rpi.FirstName = dr.GetString(dr.GetOrdinal("first_name"));
                        rpi.LastName = dr.GetString(dr.GetOrdinal("last_name"));
                        rpi.PhoneNum = "(" + dr.GetString(dr.GetOrdinal("employee_phone_area")) + ")" +
                                dr.GetString(dr.GetOrdinal("employee_phone_prefix")) + "-" +
                                dr.GetString(dr.GetOrdinal("employee_phone_last4"));
                        rpi.DaysWorked = new List<int>();
                        rpi.DaysWorked.Add(dr.GetInt32(dr.GetOrdinal("punch_count")));
                        rpi.Depts = new List<String>();
                        rpi.Depts.Add(dr.GetString(dr.GetOrdinal("description")));
                        rpi.DnrReason = "active";
                        rpi.Notes = dr.GetString(dr.GetOrdinal("data"));
                        rp.RecruitPoolCollection.Add(rpi);
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
                
                throw (ex);
            }
            finally
            {
                cw.Dispose();
            }
            return rp;
        }

        public RecruitPool GetPhoneBlastList(string id)
        {
            int phoneBlastHeaderId = Convert.ToInt32(id);
            RecruitPool rp = new RecruitPool();
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetPhoneBlastList);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@phoneBlastListHeaderId", DbType.Int32, phoneBlastHeaderId);

            try
            {
                rp.RecruitPoolCollection = new List<RecruitPoolItem>();
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        RecruitPoolItem rpi = new RecruitPoolItem();
                        rpi.BadgeNumber = dr.GetString(dr.GetOrdinal("aident_number"));
                        rpi.FirstName = dr.GetString(dr.GetOrdinal("first_name"));
                        rpi.LastName = dr.GetString(dr.GetOrdinal("last_name"));
                        rpi.Notes = dr.GetString(dr.GetOrdinal("data"));
                        rpi.PhoneNum = "(" + dr.GetString(dr.GetOrdinal("employee_phone_area")) + ")" +
                                dr.GetString(dr.GetOrdinal("employee_phone_prefix")) + "-" +
                                dr.GetString(dr.GetOrdinal("employee_phone_last4"));
                        rpi.DnrReason = "active";
                        rp.RecruitPoolCollection.Add(rpi);
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
                
                throw (ex);
            }
            finally
            {
                cw.Dispose();
            }
            return rp;
        }


        public List<PhoneBlastList> GetPhoneBlastLists()
        {
            List<PhoneBlastList> pbl = new List<PhoneBlastList>();
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetPhoneBlastListNames);
            cw.CommandTimeout = 120;    //10 minutes
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        PhoneBlastList pb = new PhoneBlastList();
                        pb.Description = (dr.GetString(dr.GetOrdinal("description")));
                        pb.PhoneBlastListID = (dr.GetInt32(dr.GetOrdinal("phone_blast_list_header_id")));
                        pbl.Add(pb);
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
                
                throw (ex);
            }
            finally
            {
                cw.Dispose();
            }
            return pbl;
        }


        public PhoneBlast GetPhoneBlast(string num)
        {
            PhoneBlast pb = new PhoneBlast();
            pb.Response = 0;
            pb.Language = 0;
            pb.ResponseDate = new DateTime(2012, 1, 1);
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.GetPhoneBlast);
            cw.CommandTimeout = 120;    //10 minutes
            dbSvc.AddInParameter(cw, "@phoneNum", DbType.String, num);
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        pb.Response = (dr.GetInt32(dr.GetOrdinal("response")));
                        /*
                         * AND ABS(DATEDIFF(second, GETDATE(), pb.response_dt )) < 3600 * 3 -- less than 3 hours
                         */
                        pb.ResponseDate = (dr.GetDateTime(dr.GetOrdinal("response_dt")));
                        if (DateTime.Now.Ticks - pb.ResponseDate.Ticks > ((3 * 60 * 60) * TimeSpan.TicksPerSecond))
                            pb.Response = 0;
                        pb.Language = dr.GetInt32(dr.GetOrdinal("language"));
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
                
                throw (ex);
            }
            finally
            {
                cw.Dispose();
            }
            return pb;
        }
    }
}