using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Collections;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using MSI.Web.MSINet.BusinessEntities;
using MSI.Web.MSINet.Common;
using MSI.Web.MSINet.DataAccess;
using System.Security.Principal;

/// <summary>
/// Summary description for ClientBL
/// </summary>
namespace MSI.Web.MSINet.BusinessLogic
{
    public class TicketTrackerBL
    {
        public TicketTrackerBL()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TicketTrackerDB ticketTrackerDB = new TicketTrackerDB();
        private HelperFunctions helperFunctions = new HelperFunctions();

        public TicketTrackerException GetTicketTrackingExceptions(TicketTrackerException ticketTracker, IPrincipal userPrincipal, String aident)
        {
            return ticketTrackerDB.GetTicketTrackingExceptions(ticketTracker, userPrincipal, aident);
        }
        public DailyTracker GetDailyInfo(DailyTracker dailyTracker, String badgeNum)
        {
            //get the shift start and end times using shift type and department
            ClientBL clientBL = new ClientBL();
            Client client = new Client();
            client.ClientID = dailyTracker.ClientId;
            client.Departments.Add(dailyTracker.Dept);
            client.ShiftTypes.Add(dailyTracker.ShiftType);
            Client clientWithShifts = clientBL.GetClientShifts(client);

            //format the period start and period end to coincide with
            //the shift atart and end times
            DateTime shiftStart;
            DateTime shiftEnd;
            TimeSpan difference = new TimeSpan();
            DailyTracker dailyTrackerIn = new DailyTracker();
            DailyTracker dailyTrackerOut = new DailyTracker();
            DailyTracker dailyTrackerTmp = new DailyTracker();
            DateTime todayShiftStart = new DateTime(1, 1, 1);
            string currentTime = "";


            foreach (Shift shift in clientWithShifts.Shifts)
            {
                if (shift.TrackingStartTime.Trim().Length == 0)
                {
                    shiftStart = DateTime.Parse(dailyTracker.PeriodStart.ToString("MM/dd/yyyy " + shift.DefaultStartTime));
                    shiftStart = shiftStart.AddHours(-2);
                    shiftEnd = DateTime.Parse(dailyTracker.PeriodEnd.ToString("MM/dd/yyyy " + shift.DefaultEndTime));
                    shiftEnd = shiftEnd.AddHours(4.5);
                    if (shiftEnd < shiftStart)
                    {
                        //shift spans multiple days
                        //When current date + shift start time > current date/time
                        currentTime = helperFunctions.GetCSTCurrentDateTime().ToString("HH:mm:ss");
                        todayShiftStart = DateTime.Parse(dailyTracker.PeriodStart.ToString("MM/dd/yyyy " + currentTime));
                        if (shiftStart > todayShiftStart)
                        {
                            difference = shiftStart - todayShiftStart;
                            shiftEnd = shiftEnd.AddDays(1);
                        }
                        else
                        {
                            //when current date + shift start time < current date/time
                            //leave the shift start date = current date
                            //make shift end date = current date + 1
                            shiftEnd = shiftEnd.AddDays(1);
                        }
                    }
                }
                else
                {
                    shiftStart = DateTime.Parse(dailyTracker.PeriodStart.ToString("MM/dd/yyyy " + shift.TrackingStartTime));
                    shiftEnd = DateTime.Parse(dailyTracker.PeriodEnd.ToString("MM/dd/yyyy " + shift.TrackingEndTime));
                    if (shift.TrackingMultiDay)
                    {
                        shiftEnd = shiftEnd.AddDays(1);
                    }
                }
                //dailyTrackerIn. = ticketTracker.ClientRosterID;
                //dailyTrackerIn. = ticketTracker.TrackingType;
                dailyTrackerIn.ClientId = dailyTracker.ClientId;
                dailyTrackerIn.PeriodStart = shiftStart;
                dailyTrackerIn.PeriodEnd = shiftEnd;
                dailyTrackerIn.Dept = dailyTracker.Dept;
                dailyTrackerIn.ShiftType = dailyTracker.ShiftType;

                dailyTrackerTmp = ticketTrackerDB.GetDailyTracking(dailyTrackerIn);
                
                dailyTracker.Employees.AddRange(dailyTrackerTmp.Employees);
                dailyTracker.PeriodStart = shiftStart;
                dailyTracker.PeriodEnd = shiftEnd;
                //ticketTracker.OverrideRoleName = ticketTrackerTmp.OverrideRoleName;
            }
            return dailyTracker;
        }
        public TicketTracker GetTicketTracking(TicketTracker ticketTracker, IPrincipal userPrincipal, String badgeNum, Boolean temps, Boolean hideEmpty)
        {
            //get the shift start and end times using shift type and department
            ClientBL clientBL = new ClientBL();
            Client client = new Client();
            client.ClientID = ticketTracker.ClientID;
            client.Departments.Add(ticketTracker.DepartmentInfo);
            client.ShiftTypes.Add(ticketTracker.ShiftTypeInfo);
            Client clientWithShifts = clientBL.GetClientShifts(client, ticketTracker.Location);
            //format the period start and period end to coincide with
            //the shift start and end times
            DateTime shiftStart;
            DateTime shiftEnd;
            TimeSpan difference =  new TimeSpan();
            TicketTracker ticketTrackerIn = new TicketTracker();
            TicketTracker ticketTrackerOut = new TicketTracker();
            TicketTracker ticketTrackerTmp = new TicketTracker();
            DateTime todayShiftStart = new DateTime(1, 1, 1);
            string currentTime = "";
            foreach (Shift shift in clientWithShifts.Shifts)
            {
                if (shift.TrackingStartTime.Trim().Length == 0)
                {
                    shiftStart = DateTime.Parse(ticketTracker.PeriodStartDateTime.ToString("MM/dd/yyyy " + shift.DefaultStartTime));
                    //shiftStart = shiftStart.AddMinutes(-15);
                    shiftStart = shiftStart.AddHours(-2);
                    shiftEnd = DateTime.Parse(ticketTracker.PeriodEndDateTime.ToString("MM/dd/yyyy " + shift.DefaultEndTime));
                    shiftEnd = shiftEnd.AddHours(4.5);
                    if (shiftEnd < shiftStart)
                    {
                        //shiftStart = shiftStart.AddDays(1);
                        //shift spans multiple days
                        //When current date + shift start time > current date/time
                        currentTime = helperFunctions.GetCSTCurrentDateTime().ToString("HH:mm:ss");
                        todayShiftStart = DateTime.Parse(ticketTracker.PeriodStartDateTime.ToString("MM/dd/yyyy " + currentTime));
                        if (shiftStart > todayShiftStart)
                        {
                            difference = shiftStart - todayShiftStart;
                            shiftEnd = shiftEnd.AddDays(1);
                        }
                        else
                        {
                            //when current date + shift start time < current date/time
                            //leave the shift start date = current date
                            //make shift end date = current date + 1
                            shiftEnd = shiftEnd.AddDays(1);
                        }
                    }
                }
                else
                {
                    shiftStart = DateTime.Parse(ticketTracker.PeriodStartDateTime.ToString("MM/dd/yyyy " + shift.TrackingStartTime));
                    shiftEnd = DateTime.Parse(ticketTracker.PeriodEndDateTime.ToString("MM/dd/yyyy " + shift.TrackingEndTime));
                    if (shift.TrackingMultiDay)
                    {
                        shiftEnd = shiftEnd.AddDays(1);
                    }
                }
                //ticketTrackerIn.RosterEmployeeFlag = ticketTracker.RosterEmployeeFlag;
                ticketTrackerIn.ClientRosterID = ticketTracker.ClientRosterID;
                ticketTrackerIn.TrackingType = ticketTracker.TrackingType;
                ticketTrackerIn.ClientID = ticketTracker.ClientID;
                ticketTrackerIn.Location = ticketTracker.Location;
                ticketTrackerIn.PeriodStartDateTime = shiftStart;
                ticketTrackerIn.PeriodEndDateTime = shiftEnd;
                ticketTrackerIn.DepartmentInfo = ticketTracker.DepartmentInfo;
                ticketTrackerIn.ShiftTypeInfo = ticketTracker.ShiftTypeInfo;

                ticketTrackerTmp = ticketTrackerDB.GetTicketTracking(ticketTrackerIn, userPrincipal);

                for (int i = 0; i < ticketTrackerTmp.Employees.Count; i++ )
                {
                    EmployeeTracker et = (EmployeeTracker)ticketTrackerTmp.Employees[i];
                    if ((et.Temp == true && !temps) || (hideEmpty && et.Punches.Count == 0)) 
                    {
                        ticketTrackerTmp.Employees.Remove(et);
                        i--;
                    }
                }
                ticketTracker.Employees.AddRange(ticketTrackerTmp.Employees);
                ticketTracker.PeriodStartDateTime = shiftStart;
                ticketTracker.PeriodEndDateTime = shiftEnd;
                ticketTracker.OverrideRoleName = ticketTrackerTmp.OverrideRoleName;
            }
            //return the ticket tracking
            return ticketTracker;
        }

        public TicketTracker GetTicketTrackingAlerts(TicketTracker ticketTracker)
        {
            return this.GetTicketTracking(ticketTracker, null, null, true, false);
        }

        public bool ApproveTrackingHours(TicketTrackerApproval approvalInfo, IPrincipal userPrincipal)
        {
            return ticketTrackerDB.ApproveTrackingHours(approvalInfo, userPrincipal);
        }

        public bool UnlockTrackingHours(TicketTrackerUnlock unlockInfo, IPrincipal userPrincipal)
        {
            return ticketTrackerDB.UnlockTrackingHours(unlockInfo, userPrincipal);
        }
    }
}