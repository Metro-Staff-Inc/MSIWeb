using Microsoft.Practices.EnterpriseLibrary.Data;
using MSI.Web.MSINet.DataAccess;
using System;
using System.Data;
using System.Data.Common;
using ApiWebServices_PunchData;
using System.Data.SqlClient;
using System.Configuration;
using ApiWebServices_HoursData;
using ApiWebServices_EmployeeInfo;
using PunchClock;

/// <summary>
/// Summary description for ApiDA
/// </summary>
namespace MSI.Web.MSINet
{
    public class ApiDA
    {
        public static string connectionString = null;
        public ApiDA() 
        {
            ConnectionStringSettings mySetting = ConfigurationManager.ConnectionStrings["SqlServices_PROD"];
            if (mySetting == null || string.IsNullOrEmpty(mySetting.ConnectionString))
                throw new Exception("Fatal error: missing connecting string in web.config file");
            connectionString = mySetting.ConnectionString;
        }
        public EmployeeInfoResponse UpdateEmployeeInfo(EmployeeInfo ei)
        {
            EmployeeInfoResponse resp = new EmployeeInfoResponse();

            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(MSINetStoredProcs.SetEmployee);
            cw.CommandTimeout = 120;    //10 minutes

            try
            {

                if (ei.SSN == null || ei.SSN.Length == 0)
                {
                    ei.SSN = "9999";
                }
                dbSvc.AddInParameter(cw, "@ssn", DbType.String, ei.SSN);
                dbSvc.AddInParameter(cw, "@aident", DbType.String, ei.Aident);
                dbSvc.AddInParameter(cw, "@LastName", DbType.String, ei.LastName);
                dbSvc.AddInParameter(cw, "@FirstName", DbType.String, ei.FirstName);

                dbSvc.AddInParameter(cw, "@addr1", DbType.String, ei.Address);
                dbSvc.AddInParameter(cw, "@addr2", DbType.String, ei.Address2);

                dbSvc.AddInParameter(cw, "@city", DbType.String, ei.City);
                dbSvc.AddInParameter(cw, "@state", DbType.String, ei.State);
                dbSvc.AddInParameter(cw, "@middleName", DbType.String, ei.MiddleInitial);
                dbSvc.AddInParameter(cw, "@zip", DbType.String, ei.Zip);
                dbSvc.AddInParameter(cw, "@phoneArea", DbType.String, ei.Phone.Substring(0,3));
                dbSvc.AddInParameter(cw, "@phonePrefix", DbType.String, ei.Phone.Substring(3,3));
                dbSvc.AddInParameter(cw, "@phoneLast4", DbType.String, ei.Phone.Substring(6,4));
                dbSvc.AddInParameter(cw, "@roleID", DbType.Int32, 1);
                dbSvc.AddInParameter(cw, "@email", DbType.String, ei.Email);
                dbSvc.AddInParameter(cw, "@action", DbType.Int32, 2); //update
                dbSvc.AddInParameter(cw, "@birthDate", DbType.String, ei.Birthdate);

                dbSvc.ExecuteNonQuery(cw);
                resp.Success = true;
                resp.Msg = "Information Updated";
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Msg = ex.ToString();
            }
            finally
            {
                cw.Dispose();
            }
            return resp;
        }

        public HoursResponseFlat RetrieveHoursFlat(HoursRequest hrq)
        {
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(ApiStoredProcs.GetHours);

            if( hrq._weekEndDate != null )
            {
                dbSvc.AddInParameter(cw, "@weekEndDate", DbType.DateTime, hrq._weekEndDate);
            }
            dbSvc.AddInParameter(cw, "@clientId", DbType.Int32, hrq.ClientID);
            dbSvc.AddInParameter(cw, "@password", DbType.String, hrq.Password);
            dbSvc.AddInParameter(cw, "@userName", DbType.String, hrq.UserName);

            HoursResponseFlat hr = new HoursResponseFlat();
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                HoursDataFlat hd = new HoursDataFlat();
                try
                {
                    hr.Msg = "Query Made";
                    hr.Success = true;

                    while (dr.Read())
                    {
                        if (dr.GetName(0).Equals("authorization", StringComparison.InvariantCultureIgnoreCase))
                        {
                            hr.Msg = dr.GetString(dr.GetOrdinal("authorization"));
                        }
                        else
                        {
                            hd = new HoursDataFlat();
                            hd.Aident = dr.GetString(dr.GetOrdinal("aident_number"));
                            hd.ClientName = dr.GetString(dr.GetOrdinal("client_name"));
                            hd.FirstName = dr.GetString(dr.GetOrdinal("first_name"));
                            hd.LastName = dr.GetString(dr.GetOrdinal("last_name"));
                            hd.DepartmentName = dr.GetString(dr.GetOrdinal("department_name"));
                            hd.LocationName = dr.GetString(dr.GetOrdinal("location_name"));
                            hd.RegularHours = dr.GetDecimal(dr.GetOrdinal("total_regular_hours"));
                            hd.OTHours = dr.GetDecimal(dr.GetOrdinal("total_ot_hours"));
                            hd.ShiftType = dr.GetInt32(dr.GetOrdinal("shift_type"));
                            hd.WeekEndDate = dr.GetDateTime(dr.GetOrdinal("week_end_dt")).ToString("yyyy-MM-dd");
                            hd.StartDate = dr.GetDateTime(dr.GetOrdinal("start_dt")).ToString("yyyy-MM-dd");
                            if (!dr.IsDBNull(dr.GetOrdinal("dnr_dt")))
                            {
                                hd.DNRDate = dr.GetDateTime(dr.GetOrdinal("dnr_dt")).ToString("yyyy-MM-dd");
                                hd.DNRReason = dr.GetString(dr.GetOrdinal("dnr_reason"));
                            }

                            if (!dr.IsDBNull(dr.GetOrdinal("job_title")))
                            {
                                hd.JobTitle = dr.GetString(dr.GetOrdinal("job_title"));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("termination_reason")))
                            {
                                hd.TerminationReason = dr.GetString(dr.GetOrdinal("termination_reason"));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("primary_department")))
                            {
                                hd.PrimaryDepartment = dr.GetString(dr.GetOrdinal("primary_department"));
                            }
                            hr.Data.Add(hd);
                        }
                    }
                }
                catch (Exception ex)
                {
                    hr.Msg = "EXCEPTION OCCURRED: " + ex.ToString() + "Last Line Read: " + hd.ToString();
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
                hr.Msg = ex.ToString();
                hr.Success = false;
            }
            finally
            {
                cw.Dispose();
            }
            return hr;
        }

        internal BridgfordOut BridgfordEmployeeData(BridgfordIn brIn)
        {
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(ApiStoredProcs.GetBridgfordEmployeeInfo);
            BridgfordOut bfo = new BridgfordOut();
            bfo.Msg = "Success";
            try
            {
                dbSvc.AddInParameter(cw, "@endDate", DbType.String, brIn.Date);
                DateTime date = new DateTime(
                            Convert.ToInt32(brIn.Date.Substring(0, 4)),
                            Convert.ToInt32(brIn.Date.Substring(5, 2)),
                            Convert.ToInt32(brIn.Date.Substring(8, 2))
                    );
                dbSvc.AddInParameter(cw, "@monThr", DbType.Boolean, 
                    date.DayOfWeek == DayOfWeek.Thursday);

                IDataReader dr = dbSvc.ExecuteReader(cw);
                try
                {
                    while (dr.Read())
                    {
                        BridgfordEmployeeData employeeData = new BridgfordEmployeeData();
                        employeeData.Aident = dr.GetString(dr.GetOrdinal("aident_number"));
                        employeeData.LastName = dr.GetString(dr.GetOrdinal("last_name"));
                        employeeData.FirstName = dr.GetString(dr.GetOrdinal("first_name"));
                        employeeData.Department = dr.GetString(dr.GetOrdinal("department_name"));
                        employeeData.Shift = "" + dr.GetInt32(dr.GetOrdinal("shift"));
                        employeeData.TotalHours = dr.GetDecimal(dr.GetOrdinal("Total_Hours"));

                        bfo.EmployeeData.Add(employeeData);
                    }
                }
                catch (Exception ex)
                {
                    bfo.Msg = ex.ToString();
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
                bfo.Msg = ex.ToString();
            }
            finally
            {
                cw.Dispose();
            }
            return bfo;
        }

        public MobileDataOut MobilePunch(MobileDataIn mdi)
        {
            MobileDataOut mdo = new MobileDataOut();

            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(ApiStoredProcs.MobilPunch);

            dbSvc.AddInParameter(cw, "@clientId", DbType.Int32, Convert.ToInt32(mdi.ClientId));
            dbSvc.AddInParameter(cw, "@aidentNumber", DbType.String, mdi.Id);
            dbSvc.AddInParameter(cw, "@phoneDateTime", DbType.String, mdi._phoneDateTime.ToString("yyyy/MM/dd HH:mm:ss"));
            dbSvc.AddInParameter(cw, "@clientDateTime", DbType.String, mdi._clientDateTime.ToString("yyyy/MM/dd HH:mm:ss"));
            dbSvc.AddInParameter(cw, "@phoneLatitude", DbType.Double, Convert.ToDouble(mdi.PhoneLatitude));
            dbSvc.AddInParameter(cw, "@phoneLongitude", DbType.Double, Convert.ToDouble(mdi.PhoneLongitude));
            dbSvc.AddInParameter(cw, "@punchClockId", DbType.String, mdi.PunchClockId);
            if( mdi.DepartmentId != null && mdi.DepartmentId.Length > 0 )
            {
                dbSvc.AddInParameter(cw, "@departmentId", DbType.Int32, Convert.ToInt32(mdi.DepartmentId));
            }

            dbSvc.AddOutParameter(cw, "@clientName", DbType.String, 64);
            dbSvc.AddOutParameter(cw, "@departmentName", DbType.String, 64);
            dbSvc.AddOutParameter(cw, "@employeeName", DbType.String, 32);
            dbSvc.AddOutParameter(cw, "@response", DbType.String, 256);
            dbSvc.AddOutParameter(cw, "@responseId", DbType.Int32, 4);
            try
            {
                int rows = dbSvc.ExecuteNonQuery(cw);
                mdo.Success = Convert.ToInt32(cw.Parameters["@responseId"].Value);
                mdo.Message = Convert.ToString(cw.Parameters["@response"].Value);
                if(mdo.Success >= 0 )
                {
                    mdo.Name = Convert.ToString(cw.Parameters["@employeeName"].Value);
                    mdo.ClientName = Convert.ToString(cw.Parameters["@clientName"].Value);
                    mdo.DepartmentName = Convert.ToString(cw.Parameters["@departmentName"].Value);
                    mdo.Day = mdi._phoneDateTime.ToString("dddd");
                    mdo.Id = mdi.Id;
                }
            }
            catch(Exception ex)
            {
                mdo.Success = -10;
                mdo.Message = ex.ToString();
            }
            finally
            {
                cw.Dispose();
            }
            return mdo;
        }

        //aident_number last_name   first_name employee_address_1  employee_address_2 employee_city   employee_state employee_phone  birthdate employee_zip    email
        //00700	Murfey Jonathan	3637 N.Springfield Ave.Apt 2	Chicago IL  (773)773-2056	2020-10-22	60618	jmurfey @msistaff.com
        public EmployeeInfoResponse GetEmployeeInfo(EmployeeInfo e)
        {
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(ApiStoredProcs.GetEmployeeACAInfo);

            /* need employee id and last 4 of social security # */
            dbSvc.AddInParameter(cw, "@ssnLast4", DbType.String, e.SSN);
            dbSvc.AddInParameter(cw, "@aidentNumber", DbType.String, e.Aident);

            EmployeeInfoResponse resp = new EmployeeInfoResponse();
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                EmployeeInfo ei = new EmployeeInfo();
                try
                {
                    resp.Msg = "Query Made";
                    resp.Success = true;

                    while (dr.Read())
                    {
                        if (dr.GetName(0).Equals("qualification", StringComparison.InvariantCultureIgnoreCase))
                        {
                            resp.Msg = dr.GetString(dr.GetOrdinal("qualification"));
                        }
                        else
                        {
                            ei = new EmployeeInfo();
                            ei.Aident = dr.GetString(dr.GetOrdinal("aident_number"));
                            ei.FirstName = dr.GetString(dr.GetOrdinal("first_name"));
                            ei.LastName = dr.GetString(dr.GetOrdinal("last_name"));
                            ei.Address = dr.GetString(dr.GetOrdinal("employee_address_1"));
                            ei.Address2 = dr.GetString(dr.GetOrdinal("employee_address_2"));
                            ei.Birthdate = dr.GetString(dr.GetOrdinal("birthdate"));
                            ei.City = dr.GetString(dr.GetOrdinal("employee_city"));
                            ei.State = dr.GetString(dr.GetOrdinal("employee_state"));
                            ei.Zip = dr.GetString(dr.GetOrdinal("employee_zip"));
                            ei.Email = dr.GetString(dr.GetOrdinal("email"));
                            ei.Phone = dr.GetString(dr.GetOrdinal("phone"));
                            ei.SSN = e.SSN;
                            resp.Data = ei;
                        }
                    }
                }
                catch (Exception ex)
                {
                    resp.Msg = "EXCEPTION OCCURRED: " + ex.ToString() + 
                        "Last Line Read: " + ei.ToString();
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
                resp.Msg = ex.ToString();
                resp.Success = false;
            }
            finally
            {
                cw.Dispose();
            }
            return resp;
        }

        public PunchResponseFlat RetrievePunchesFlat(PunchRequest prq)
        {
            DbCommand cw;
            Database dbSvc = DatabaseFactory.CreateDatabase();
            cw = dbSvc.GetStoredProcCommand(ApiStoredProcs.GetPunches);
            //cw.CommandTimeout = 120;

            dbSvc.AddInParameter(cw, "@endDate", DbType.DateTime, prq.end);
            dbSvc.AddInParameter(cw, "@startDate", DbType.DateTime, prq.start);
            dbSvc.AddInParameter(cw, "@clientId", DbType.Int32, prq.ClientID);
            dbSvc.AddInParameter(cw, "@password", DbType.String, prq.Password);
            dbSvc.AddInParameter(cw, "@userName", DbType.String, prq.UserName);

            PunchResponseFlat prs = new PunchResponseFlat();
            try
            {
                IDataReader dr = dbSvc.ExecuteReader(cw);
                PunchData pd = new PunchData();
                try
                {
                    prs.Msg = "Query Made";
                    prs.Success = true;

                    while (dr.Read())
                    {
                        if (dr.GetName(0).Equals("authorization", StringComparison.InvariantCultureIgnoreCase))
                        {
                            prs.Msg = dr.GetString(dr.GetOrdinal("authorization"));
                        }
                        else
                        {
                            pd = new PunchData();
                            pd.ClientID = dr.GetInt32(dr.GetOrdinal("client_id"));
                            pd.Aident = dr.GetString(dr.GetOrdinal("aident"));
                            pd.ClientName = dr.GetString(dr.GetOrdinal("client_name"));
                            pd.FirstName = dr.GetString(dr.GetOrdinal("first_name"));
                            pd.LastName = dr.GetString(dr.GetOrdinal("last_name"));

                            if( !dr.IsDBNull(dr.GetOrdinal("punch_dt")) )
                            {
                                pd.PunchDate = dr.GetDateTime(dr.GetOrdinal("punch_dt"));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("rounded_punch_dt")))
                            {
                                pd.RoundedPunchDate = dr.GetDateTime(dr.GetOrdinal("rounded_punch_dt"));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("created_dt")))
                            {
                                pd.CreatedDate = dr.GetDateTime(dr.GetOrdinal("created_dt"));
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("created_by")))
                            {
                                pd.CreatedBy = dr.GetString(dr.GetOrdinal("created_by"));
                            }
                            if( !dr.IsDBNull(dr.GetOrdinal("department_id")))
                            {
                                pd.DepartmentId = dr.GetInt32(dr.GetOrdinal("department_id"));
                            }
                            else
                            {
                                continue;       /* should not have NULL department_id values */
                            }
                            if( !dr.IsDBNull(dr.GetOrdinal("department_name")))
                            {
                                pd.DepartmentName = dr.GetString(dr.GetOrdinal("department_name"));
                            }
                            //pd.ShiftDesc = dr.GetString(dr.GetOrdinal("shift_desc"));
                            pd.ShiftStart = dr.GetString(dr.GetOrdinal("shift_start"));
                            pd.ShiftEnd = dr.GetString(dr.GetOrdinal("shift_end"));
                            pd.BreakTime = dr.GetDecimal(dr.GetOrdinal("break_time"));
                            pd.PayRate = dr.GetDecimal(dr.GetOrdinal("pay_rate"));
                            pd.LocationID = dr.GetInt32(dr.GetOrdinal("location_id"));
                            pd.LocationName = dr.GetString(dr.GetOrdinal("location_name"));
                            pd.ShiftType = dr.GetInt32(dr.GetOrdinal("shift_type"));
                            prs.Data.Add(pd);
                        }
                    }
                }
                catch (Exception ex)
                {
                    prs.Msg = "EXCEPTION OCCURRED: " + ex.ToString() + "Last Line Read: " + pd.ToString();
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
                prs.Msg = ex.ToString();
                prs.Success = false;
            }
            finally
            {
                cw.Dispose();
            }
            return prs;
        }
        public PunchResponse RetrievePunches(PunchRequest prq)
        {
            PunchResponse pr = new PunchResponse();
            pr.Client = new Client();
            pr.Client.ID = prq.ClientID;
            using (var conn = new SqlConnection(connectionString))
            using (var command = new SqlCommand(ApiStoredProcs.GetPunches, conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                command.Parameters.AddWithValue("@endDate", prq.end);
                command.Parameters.AddWithValue("@startDate", prq.start);
                command.Parameters.AddWithValue("@clientId", prq.ClientID);
                command.Parameters.AddWithValue("@password", prq.Password);
                command.Parameters.AddWithValue("@userName", prq.UserName);
                conn.Open();
                // execute the command
                using (SqlDataReader rdr = command.ExecuteReader())
                {
                    pr.Msg = "Query Made";
                    pr.Success = true;

                    while (rdr.Read())
                    {
                        if (rdr.GetName(0).Equals("authorization", StringComparison.InvariantCultureIgnoreCase))
                        {
                            pr.Msg = "UserName / Password Failure";
                        }
                        else
                        {
                            if (pr.Client.Name == null)
                            {
                                pr.Client.Name = rdr.GetString(rdr.GetOrdinal("client_name"));
                            }
                            do
                            {

                            } while (rdr.Read());
                        }
                    }
                }
            }
            return pr;
        }
    }
}