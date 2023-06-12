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
using System.Collections.Generic;

/// <summary>
/// Summary description for ClientBL
/// </summary>
namespace MSI.Web.MSINet.BusinessLogic
{
    public class EmployeePunchBL
    {
        public EmployeePunchBL()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        //private EmployeePunchDB employeePunchDB = new EmployeePunchDB();
        private HelperFunctions helperFunctions = new HelperFunctions();

        public string LineApprove(List<string> punchIds)
        {
            string userName = punchIds[0];
            string input = "";
            for( int i=1; i<punchIds.Count; i++ )
            {
                input += punchIds[i];
                if( i<punchIds.Count-1 )
                {
                    input += ",";
                }
            }
            EmployeePunchDB epdb = new EmployeePunchDB();
            string updateCount = epdb.LineApprove(userName, input);
            return updateCount;
        }
        public string ClearClientRoster(string clientRosterId)
        {
            EmployeePunchDB epdb = new EmployeePunchDB();
            string retMsg = epdb.ClearClientRoster(Convert.ToInt32(clientRosterId));
            return retMsg;
        }
        public ArrayList GetEmployeeTicketForPunch(EmployeePunchSummary employeePunchSummary, IPrincipal userPrincipal)
        {
            EmployeePunchDB epdb = new EmployeePunchDB();
            return epdb.GetEmployeeTicketForPunch(employeePunchSummary, userPrincipal);
        }

        public ArrayList GetEmployeeRosterScheduleForPunch(EmployeePunchSummary employeePunchSummary, IPrincipal userPrincipal)
        {
            EmployeePunchDB epdb = new EmployeePunchDB();
            return epdb.GetEmployeeRosterScheduleForPunch(employeePunchSummary, userPrincipal);
        }

        public EmployeePunchResult ValidateShiftStart(EmployeePunchSummary employeePunchSummary)
        {
            //check if there is already a check in without any previous checkouts.
            //if so then we do not need to validate the start.
            EmployeePunchResult returnResult = new EmployeePunchResult();
            returnResult.EmployeePunchSummaryInfo = employeePunchSummary;

            //   punchInfo.RoundedPunchDateTime = helper.GetRoundedPunchTime(punchInfo.PunchDateTime);
            bool swipeRounded = employeePunchSummary.UseExactTimes;

            if (returnResult.EmployeePunchSummaryInfo.ManualOverride)
            {
                returnResult.PunchSuccess = true;
            }
            else
            {
                returnResult.PunchSuccess = true;

                //2. check if the current time is 15 min less than ticket start time 
                //the times are stored in cst
                DateTime shiftEndDateTime = DateTime.Parse(employeePunchSummary.PunchDateTime.Date.ToShortDateString() + " " + returnResult.EmployeePunchSummaryInfo.TicketInfo.ShiftEndTime);
                DateTime shiftStartDateTime = DateTime.Parse(employeePunchSummary.PunchDateTime.Date.ToShortDateString() + " " + returnResult.EmployeePunchSummaryInfo.TicketInfo.ShiftStartTime);

                //DateTime cstNow = helperFunctions.GetCSTCurrentDateTime();

                DateTime alternateShiftStartDateTime = new DateTime(1, 1, 1);
                DateTime employeePunchDateTime = new DateTime(1, 1, 1);

                //if the shift start time is midnight then add one to the day to account for 
                //check ins done 30 min before midnight
                if (returnResult.EmployeePunchSummaryInfo.TicketInfo.ShiftStartTime == "00:00" || returnResult.EmployeePunchSummaryInfo.TicketInfo.ShiftStartTime == "0:00" || returnResult.EmployeePunchSummaryInfo.TicketInfo.ShiftStartTime == "00:00:00" || returnResult.EmployeePunchSummaryInfo.TicketInfo.ShiftStartTime == "0:00:00")
                {
                    if (employeePunchSummary.PunchDateTime.Hour > 16)
                    {
                        shiftStartDateTime = shiftStartDateTime.AddDays(1);
                        shiftEndDateTime = shiftEndDateTime.AddDays(1);
                    }
                }

                DateTime adjustedShiftStartDateTime = shiftStartDateTime;

                //adjust the start time for shifts with overlapping start and end times
                if (shiftEndDateTime < shiftStartDateTime)
                {
                    //if (cstNow < shiftStartDateTime && cstNow < shiftEndDateTime)
                    if( shiftStartDateTime > shiftEndDateTime )
                    {
                        //adjustedShiftStartDateTime = shiftStartDateTime.AddDays(-1);
                        shiftEndDateTime = shiftEndDateTime.AddDays(1);
                    }
                }

                if (returnResult.EmployeePunchSummaryInfo.PunchDateTime > adjustedShiftStartDateTime)
                {
                    //use actual time because it is a punch in
                    employeePunchDateTime = returnResult.EmployeePunchSummaryInfo.PunchDateTime;
                    if (returnResult.EmployeePunchSummaryInfo.ClientID == 381)
                    {
                        returnResult.EmployeePunchSummaryInfo.RoundedPunchDateTime = returnResult.EmployeePunchSummaryInfo.PunchDateTime;
                        swipeRounded = true;
                    }
                }


                // Difference in minutes between rounded punch and shift start
                TimeSpan ts = adjustedShiftStartDateTime - returnResult.EmployeePunchSummaryInfo.RoundedPunchDateTime;
                // Difference in minutes must be less than 15. AND same day
                if (ts.Hours <= 0)
                {
                    if (ts.Hours == 0)
                    {
                        if (ts.Minutes >= 0 && ts.Minutes <= 30 && ts.Seconds >= 0)
                        {
                            //set the punch date/time to the shift start
                            //**JHM**returnResult.EmployeePunchSummaryInfo.PunchDateTime = shiftStartDateTime;
                            /* no round for Arthur Schumann */
                            if (returnResult.EmployeePunchSummaryInfo.ClientID != 113 &&
                                (returnResult.EmployeePunchSummaryInfo.ClientID < 325 || returnResult.EmployeePunchSummaryInfo.ClientID > 327))
                                returnResult.EmployeePunchSummaryInfo.RoundedPunchDateTime = helperFunctions.GetRoundedPunchTime(shiftStartDateTime);
                            swipeRounded = true;
                            //success record punch
                            //returnResult.PunchSuccess = true;
                        }
                    }
                    else
                    {
                        //success record an employee punch
                        //returnResult.PunchSuccess = true;
                        //round up if alternate time
                        if (returnResult.EmployeePunchSummaryInfo.TicketInfo.AlternateShiftStartTime.Length > 0)
                        {
                            alternateShiftStartDateTime = DateTime.Parse(/* employeePunchSummary.PunchDateTime.Date.ToShortDateString() + " " + */returnResult.EmployeePunchSummaryInfo.TicketInfo.AlternateShiftStartTime);
                            if (returnResult.EmployeePunchSummaryInfo.PunchDateTime > alternateShiftStartDateTime)
                            {
                                //use actual time
                                employeePunchDateTime = returnResult.EmployeePunchSummaryInfo.PunchDateTime;
                            }
                            else
                            {
                                //use rounded time
                                //employeePunchDateTime = returnResult.EmployeePunchSummaryInfo.RoundedPunchDateTime;
                            }

                            TimeSpan alternateDif = alternateShiftStartDateTime - employeePunchDateTime;
                            if (alternateDif.Hours <= 0)
                            {
                                if (alternateDif.Hours == 0)
                                {
                                    if (alternateDif.Minutes == 0 || alternateDif.Minutes == 15 || alternateDif.Minutes == 30)
                                    {
                                        returnResult.EmployeePunchSummaryInfo.PunchDateTime = alternateShiftStartDateTime;
                                        returnResult.EmployeePunchSummaryInfo.RoundedPunchDateTime = alternateShiftStartDateTime;
                                        swipeRounded = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (!returnResult.EmployeePunchSummaryInfo.ManualOverride && !swipeRounded
                && returnResult.EmployeePunchSummaryInfo.PreviousPunches.Count == 0
                && returnResult.EmployeePunchSummaryInfo.ClientID != 381 
                && (returnResult.EmployeePunchSummaryInfo.ClientID < 325 || returnResult.EmployeePunchSummaryInfo.ClientID > 327)
            )
            {
                //round for the start of the shift to the nearest quarter
                returnResult.EmployeePunchSummaryInfo.RoundedPunchDateTime = helperFunctions.GetShiftStartRoundedPunchTime(returnResult.EmployeePunchSummaryInfo.PunchDateTime);
            }
            return returnResult;
        }

        public EmployeePunchResult ValidateShiftEnd(EmployeePunchSummary employeePunchSummary)
        {
            //check if there is already a check in without any previous checkouts.
            //if so then we do not need to validate the start.
            EmployeePunchResult returnResult = new EmployeePunchResult();
            returnResult.EmployeePunchSummaryInfo = employeePunchSummary;
            returnResult.PunchSuccess = true;

            if (returnResult.EmployeePunchSummaryInfo.ManualOverride)
            {
                returnResult.PunchSuccess = true;
            }
            else
            {
                //2. check if the current time is past the ticket end time 
                //the times are stored in cst
                DateTime shiftEndDateTime = DateTime.Parse(returnResult.EmployeePunchSummaryInfo.TicketInfo.ShiftEndTime);
                DateTime shiftStartTime = DateTime.Parse(returnResult.EmployeePunchSummaryInfo.TicketInfo.ShiftStartTime);

                if (shiftEndDateTime < shiftStartTime)
                    shiftEndDateTime = shiftEndDateTime.AddDays(1);

                //DateTime cstNow = helperFunctions.GetCSTCurrentDateTime();

                //Removed the validation
                //this validation checked that a check in was occurring before
                //the end of a shift.
                if (returnResult.EmployeePunchSummaryInfo.ExpirationDateTime < returnResult.EmployeePunchSummaryInfo.PunchDateTime)
                {
                    returnResult.PunchSuccess = false;
                }
            }

            return returnResult;
        }

        public EmployeePunchResult ValidateCheckOut(EmployeePunchSummary employeePunchSummary)
        {
            //check if there is already a check in without any previous checkouts.
            //if so then we do not need to validate the start.
            EmployeePunchResult returnResult = new EmployeePunchResult();
            returnResult.EmployeePunchSummaryInfo = employeePunchSummary;
            returnResult.PunchSuccess = true;
            bool roundedTime = employeePunchSummary.UseExactTimes;

            //2. check if the current time is past the ticket end time 
            //the times are stored in cst
            DateTime shiftEndDateTime = returnResult.EmployeePunchSummaryInfo.PunchDateTime.Date + TimeSpan.Parse(returnResult.EmployeePunchSummaryInfo.TicketInfo.ShiftEndTime);
            DateTime shiftStartTime = returnResult.EmployeePunchSummaryInfo.PunchDateTime.Date + TimeSpan.Parse(returnResult.EmployeePunchSummaryInfo.TicketInfo.ShiftStartTime);

            if (shiftEndDateTime < shiftStartTime)
                shiftEndDateTime = shiftEndDateTime.AddDays(1);

            if (employeePunchSummary.ClientID == 381)
            {
                bool wasRounded = false;
                employeePunchSummary.RoundedPunchDateTime = HelperFunctions.GetCheckOutRoundedTime(employeePunchSummary.PunchDateTime, ref wasRounded);
            }
            else if (returnResult.EmployeePunchSummaryInfo.PunchDateTime > shiftEndDateTime)
            {
                //if we are within punchOutGap minutes after shift end then round to shift end
                TimeSpan ts = returnResult.EmployeePunchSummaryInfo.PunchDateTime - shiftEndDateTime;
                int punchOutGap = 7;
                if (employeePunchSummary.ClientID == 332 || employeePunchSummary.ClientID == 334)
                {
                    punchOutGap = 15;
                }
                if (ts.Hours <= 0)
                {
                    if (ts.Hours == 0)
                    {
                        if ((ts.Minutes >= 0 && ts.Minutes <= punchOutGap))
                        {
                            //set the punch date/time to the shift end
                            //returnResult.EmployeePunchSummaryInfo.PunchDateTime = shiftEndDateTime;
                            if (returnResult.EmployeePunchSummaryInfo.ClientID < 325 || returnResult.EmployeePunchSummaryInfo.ClientID > 327)
                                returnResult.EmployeePunchSummaryInfo.RoundedPunchDateTime = helperFunctions.GetRoundedPunchTime(shiftEndDateTime);
                            else
                                returnResult.EmployeePunchSummaryInfo.RoundedPunchDateTime = helperFunctions.GetExact15PunchTime(returnResult.EmployeePunchSummaryInfo.RoundedPunchDateTime);
                            roundedTime = true;
                        }
                    }
                }
            }
            else if (employeePunchSummary.UseExactTimes)
            {
                returnResult.EmployeePunchSummaryInfo.RoundedPunchDateTime = returnResult.EmployeePunchSummaryInfo.PunchDateTime;
                roundedTime = true;
            }

            if (!roundedTime && (returnResult.EmployeePunchSummaryInfo.ClientID != 113
                                && returnResult.EmployeePunchSummaryInfo.ClientID != 206
                                && returnResult.EmployeePunchSummaryInfo.ClientID != 381
                                   && returnResult.EmployeePunchSummaryInfo.ClientID != 178)
                                    && returnResult.EmployeePunchSummaryInfo.ClientID != 256
                && (returnResult.EmployeePunchSummaryInfo.ClientID < 325 || returnResult.EmployeePunchSummaryInfo.ClientID > 327))
            {
                //round for check out
                //JHM 03/16/2012 returnResult.EmployeePunchSummaryInfo.PunchDateTime = helperFunctions.GetCheckOutRoundedTime(returnResult.EmployeePunchSummaryInfo.PunchDateTime, ref roundedTime);
                if (roundedTime)
                {
                    //if the check out was rounded then set the rounded time to the check out
                    returnResult.EmployeePunchSummaryInfo.RoundedPunchDateTime = returnResult.EmployeePunchSummaryInfo.PunchDateTime;
                }
                else
                {
                    DateTime punchDT = returnResult.EmployeePunchSummaryInfo.PunchDateTime;
                    if (returnResult.EmployeePunchSummaryInfo.ClientID == 205 && (punchDT.Minute >= 48 && punchDT.Minute <= 55))
                    {
                        returnResult.EmployeePunchSummaryInfo.RoundedPunchDateTime = new DateTime(punchDT.Year, punchDT.Month, punchDT.Day, punchDT.Hour + 1, 0, 0);
                    }
                    else
                    {
                        //the check out was not rounded so get the rounded time.
                        returnResult.EmployeePunchSummaryInfo.RoundedPunchDateTime = helperFunctions.GetRoundedPunchTime(returnResult.EmployeePunchSummaryInfo.PunchDateTime);
                    }
                }
            }
            else if (!roundedTime &&
                (returnResult.EmployeePunchSummaryInfo.ClientID == 113 ||
                    returnResult.EmployeePunchSummaryInfo.ClientID == 206 ||
                        returnResult.EmployeePunchSummaryInfo.ClientID == 178 ||
                            returnResult.EmployeePunchSummaryInfo.ClientID == 256))
            {
                if (returnResult.EmployeePunchSummaryInfo.ClientID == 206)
                {
                    returnResult.EmployeePunchSummaryInfo.RoundedPunchDateTime = helperFunctions.Get10MinRoundedPunchTime(returnResult.EmployeePunchSummaryInfo.PunchDateTime);
                }
                else
                {
                    returnResult.EmployeePunchSummaryInfo.RoundedPunchDateTime = helperFunctions.GetRoundedPunchTime(returnResult.EmployeePunchSummaryInfo.PunchDateTime);
                }
            }
            return returnResult;
        }

        public EmployeePunchResult RecordEmployeePunchNew(EmployeePunchSummary employeePunchSummary, IPrincipal userPrincipal)
        {
            EmployeePunchResult returnResult = null;

            ArrayList employeePunchList = null;
            //employeePunchList = GetEmployeeTicketForPunch(employeePunchSummary, userPrincipal);
            employeePunchList = GetEmployeeRosterScheduleForPunch(employeePunchSummary, userPrincipal);

            //1. check if the employee is on a ticket or is on the roster
            if (employeePunchList.Count > 0)
            {
                foreach (EmployeePunchResult result in employeePunchList)
                {
                    returnResult = new EmployeePunchResult();
                    returnResult.EmployeePunchSummaryInfo = result.EmployeePunchSummaryInfo;

                    if (result.EmployeePunchSummaryInfo.PreviousPunches.Count % 2 == 0)
                    {
                        //this should be a check in
                        //validate the shift start
                        returnResult = ValidateShiftStart(result.EmployeePunchSummaryInfo);
                        if (returnResult.PunchSuccess)
                        {
                            //validate against the shift end time **AND** against the expiration date of the roster
                            returnResult = ValidateShiftEnd(result.EmployeePunchSummaryInfo);
                            if (returnResult.PunchSuccess)
                            {
                                returnResult.PunchType = Enums.PunchTypes.CheckIn;
                                break;
                            }
                            else
                            {
                                //error check in attempt after shift end.
                                returnResult.PunchException = PunchExceptions.CheckInAttemptAfterShiftEnd;
                            }
                        }
                        else
                        {
                            //error it is 15 minutes or more before the shift start.
                            returnResult.PunchException = PunchExceptions.BeforeShiftStart;
                        }
                    }
                    else
                    {
                        //this is a check out so ignore
                        ValidateCheckOut(result.EmployeePunchSummaryInfo);

                        //make the check out department the same as the check in
                        EmployeePunchSummary latestCheckIn = (EmployeePunchSummary)result.EmployeePunchSummaryInfo.PreviousPunches[result.EmployeePunchSummaryInfo.PreviousPunches.Count - 1];
                        result.EmployeePunchSummaryInfo.TicketInfo.DepartmentInfo.DepartmentID = latestCheckIn.TicketInfo.DepartmentInfo.DepartmentID;
                        returnResult.PunchType = Enums.PunchTypes.CheckOut;
                        returnResult.PunchSuccess = true;
                        break;
                    }
                }
            }
            else
            {
                //error employee not on a ticket
                returnResult = new EmployeePunchResult();
                returnResult.EmployeePunchSummaryInfo = employeePunchSummary;
                returnResult.PunchException = PunchExceptions.EmployeeNotAuthorized;
            }

            //record the swipe
            //check if we need to return the summary hours
            EmployeePunchResult finalResult = null;
            if (returnResult.EmployeePunchSummaryInfo.CalculateWeeklyHours)
            {
                EmployeePunchDB epdb = new EmployeePunchDB();
                finalResult = epdb.RecordEmployeePunchSummary(returnResult, userPrincipal);
            }
            else
            {
                EmployeePunchDB epdb = new EmployeePunchDB();
                finalResult = epdb.RecordEmployeePunch(returnResult, userPrincipal);
            }

            //if its a dupe then return the previous type
            if (finalResult.DuplicatePunch)
            {
                if (returnResult.PunchType == Enums.PunchTypes.CheckIn)
                {
                    finalResult.PunchType = Enums.PunchTypes.CheckOut;
                }
                else if (returnResult.PunchType == Enums.PunchTypes.CheckOut)
                {
                    finalResult.PunchType = Enums.PunchTypes.CheckIn;
                }
            }

            return finalResult;
        }


        public EmployeePunchResult RecordEmployeePunch(EmployeePunchSummary employeePunchSummary, IPrincipal userPrincipal)
        {
            EmployeePunchResult returnResult = null;

            ArrayList employeePunchList = this.GetEmployeeTicketForPunch(employeePunchSummary, userPrincipal);
            //employeePunchList = this.GetEmployeeRosterScheduleForPunch(employeePunchSummary, userPrincipal);
            //1. check if the employee is on a ticket or is on the roster
            if (employeePunchList.Count > 0)
            {
                foreach (EmployeePunchResult result in employeePunchList)
                {
                    returnResult = new EmployeePunchResult();
                    returnResult.EmployeePunchSummaryInfo = result.EmployeePunchSummaryInfo;

                    if (result.EmployeePunchSummaryInfo.PreviousPunches.Count % 2 == 0)
                    {
                        //this is a check in
                        //validate the shift start
                        returnResult = ValidateShiftStart(result.EmployeePunchSummaryInfo);
                        if (returnResult.PunchSuccess)
                        {
                            //validate against the shift end time **AND** against the expiration date of the roster
                            returnResult = ValidateShiftEnd(result.EmployeePunchSummaryInfo);
                            if (returnResult.PunchSuccess)
                            {
                                returnResult.PunchType = Enums.PunchTypes.CheckIn;
                                break;
                            }
                            else
                            {
                                //error check in attempt after shift end.
                                returnResult.PunchException = PunchExceptions.CheckInAttemptAfterShiftEnd;
                            }
                        }
                        else
                        {
                            //error it is 15 minutes or more before the shift start.
                            returnResult.PunchException = PunchExceptions.BeforeShiftStart;
                        }
                    }
                    else
                    {
                        //this is a check out so ignore
                        ValidateCheckOut(result.EmployeePunchSummaryInfo);

                        //make the check out department the same as the check in
                        EmployeePunchSummary latestCheckIn = (EmployeePunchSummary)result.EmployeePunchSummaryInfo.PreviousPunches[result.EmployeePunchSummaryInfo.PreviousPunches.Count - 1];
                        result.EmployeePunchSummaryInfo.TicketInfo.DepartmentInfo.DepartmentID = latestCheckIn.TicketInfo.DepartmentInfo.DepartmentID;
                        returnResult.PunchType = Enums.PunchTypes.CheckOut;
                        returnResult.PunchSuccess = true;
                        break;
                    }
                }
            }
            else
            {
                //error employee not on a ticket
                returnResult = new EmployeePunchResult();
                returnResult.EmployeePunchSummaryInfo = employeePunchSummary;
                returnResult.PunchException = PunchExceptions.EmployeeNotAuthorized;
            }

            //record the swipe
            //check if we need to return the summary hours
            EmployeePunchDB epdb = new EmployeePunchDB();
            EmployeePunchResult finalResult = null;
            if (returnResult.EmployeePunchSummaryInfo.CalculateWeeklyHours)
            {
                finalResult = epdb.RecordEmployeePunchSummary(returnResult, userPrincipal);
            }
            else
            {
                finalResult = epdb.RecordEmployeePunch(returnResult, userPrincipal);
            }

            //if its a dupe then return the previous type
            if (finalResult.DuplicatePunch)
            {
                if (returnResult.PunchType == Enums.PunchTypes.CheckIn)
                {
                    finalResult.PunchType = Enums.PunchTypes.CheckOut;
                }
                else if (returnResult.PunchType == Enums.PunchTypes.CheckOut)
                {
                    finalResult.PunchType = Enums.PunchTypes.CheckIn;
                }
            }

            return finalResult;
        }

        public EmployeePunchResult RecordEmployeePunchDepartmentOverride(EmployeePunchSummary employeePunchSummary, IPrincipal userPrincipal)
        {
            EmployeePunchResult returnResult = null;

            ArrayList employeePunchList = this.GetEmployeeTicketForPunch(employeePunchSummary, userPrincipal);

            //1. check if the employee is on a ticket or is on the roster
            if (employeePunchList.Count > 0)
            {
                foreach (EmployeePunchResult result in employeePunchList)
                {
                    returnResult = new EmployeePunchResult();
                    returnResult.EmployeePunchSummaryInfo = result.EmployeePunchSummaryInfo;

                    if (result.EmployeePunchSummaryInfo.PreviousPunches.Count % 2 == 0)
                    {
                        //this is a check in
                        //validate the shift start
                        returnResult = ValidateShiftStart(result.EmployeePunchSummaryInfo);
                        if (returnResult.PunchSuccess)
                        {
                            //validate against the shift end time **AND** against the expiration date of the roster
                            returnResult = ValidateShiftEnd(result.EmployeePunchSummaryInfo);
                            if (returnResult.PunchSuccess)
                            {
                                returnResult.PunchType = Enums.PunchTypes.CheckIn;
                                break;
                            }
                            else
                            {
                                //error check in attempt after shift end.
                                returnResult.PunchException = PunchExceptions.CheckInAttemptAfterShiftEnd;
                            }
                        }
                        else
                        {
                            //error it is 15 minutes or more before the shift start.
                            returnResult.PunchException = PunchExceptions.BeforeShiftStart;
                        }
                    }
                    else
                    {
                        //this is a check out so ignore
                        ValidateCheckOut(result.EmployeePunchSummaryInfo);

                        //make the check out department the same as the check in
                        EmployeePunchSummary latestCheckIn = (EmployeePunchSummary)result.EmployeePunchSummaryInfo.PreviousPunches[result.EmployeePunchSummaryInfo.PreviousPunches.Count - 1];
                        result.EmployeePunchSummaryInfo.TicketInfo.DepartmentInfo.DepartmentID = latestCheckIn.TicketInfo.DepartmentInfo.DepartmentID;
                        returnResult.PunchType = Enums.PunchTypes.CheckOut;
                        returnResult.PunchSuccess = true;
                        break;
                    }
                }
            }
            else
            {
                //error employee not on a ticket
                returnResult = new EmployeePunchResult();
                returnResult.EmployeePunchSummaryInfo = employeePunchSummary;
                returnResult.PunchException = PunchExceptions.EmployeeNotAuthorized;
            }

            //check if we need to return the summary hours
            EmployeePunchResult finalResult = null;
            EmployeePunchDB epdb = new EmployeePunchDB();
            if (returnResult.EmployeePunchSummaryInfo.CalculateWeeklyHours)
            {
                finalResult = epdb.RecordEmployeePunchSummary(returnResult, userPrincipal);
            }
            else
            {
                finalResult = epdb.RecordEmployeePunch(returnResult, userPrincipal);
            }

            //if its a dupe then return the previous type
            if (finalResult.DuplicatePunch)
            {
                if (returnResult.PunchType == Enums.PunchTypes.CheckIn)
                {
                    finalResult.PunchType = Enums.PunchTypes.CheckOut;
                }
                else if (returnResult.PunchType == Enums.PunchTypes.CheckOut)
                {
                    finalResult.PunchType = Enums.PunchTypes.CheckIn;
                }
            }

            return finalResult;
        }

        public EmployeeDepartmentPunchResult RecordEmployeeDepartmentPunch(EmployeePunchSummary employeePunchSummary, IPrincipal userPrincipal)
        {
            EmployeeDepartmentPunchResult returnResult = null;

            ArrayList employeePunchList = this.GetEmployeeTicketForPunch(employeePunchSummary, userPrincipal);

            //1. check if the employee is on a ticket or is on the roster
            if (employeePunchList.Count > 0)
            {
                if (employeePunchList.Count > 1)
                {

                }
                foreach (EmployeePunchResult result in employeePunchList)
                {
                    returnResult = new EmployeeDepartmentPunchResult();
                    returnResult.EmployeePunchSummaryInfo = result.EmployeePunchSummaryInfo;

                    //Check into General
                    //Check into department
                    // -check out of general
                    // -check into new dept
                    //Check into another dept
                    // -check out of prev dept
                    // -check into new dept
                    //Check into same department
                    // -ignore swipe
                    //Checked out
                    // -just check into new department

                    if (result.EmployeePunchSummaryInfo.PreviousPunches.Count % 2 == 0)
                    {
                        if (result.EmployeePunchSummaryInfo.PreviousPunches.Count == 0)
                        {
                            //employee never swiped into General
                            returnResult.PunchType = Enums.PunchTypes.NoGeneralPunch;
                        }
                        else
                        {
                            //employee is checked out so all that needs to happen is 
                            //check into new department
                            returnResult.PunchType = Enums.PunchTypes.CheckIn;
                            returnResult.PunchSuccess = true;
                        }
                        returnResult.CurrentDepartmentId = 0;
                    }
                    else
                    {
                        //employee is checked in
                        //check if the department is the same
                        EmployeePunchSummary latestCheckIn = (EmployeePunchSummary)result.EmployeePunchSummaryInfo.PreviousPunches[result.EmployeePunchSummaryInfo.PreviousPunches.Count - 1];
                        if (employeePunchSummary.TicketInfo.DepartmentInfo.DepartmentID == latestCheckIn.TicketInfo.DepartmentInfo.DepartmentID)
                        {
                            //checking into the same department so we want to ignore
                            returnResult.PunchType = Enums.PunchTypes.SameDepartment;
                            returnResult.CurrentDepartmentId = latestCheckIn.TicketInfo.DepartmentInfo.DepartmentID;
                        }
                        else
                        {
                            //we need to check them out of the current dept
                            //and check them into the new department
                            returnResult.CurrentDepartmentId = latestCheckIn.TicketInfo.DepartmentInfo.DepartmentID;
                            //if the punch in time is less than the most recent, then
                            //make this punch the same time
                            if (returnResult.EmployeePunchSummaryInfo.PunchDateTime < latestCheckIn.PunchDateTime)
                            {
                                returnResult.EmployeePunchSummaryInfo.PunchDateTime = latestCheckIn.PunchDateTime;
                                returnResult.EmployeePunchSummaryInfo.RoundedPunchDateTime = latestCheckIn.RoundedPunchDateTime;
                            }
                            returnResult.PunchType = Enums.PunchTypes.CheckIn;
                            returnResult.PunchSuccess = true;
                        }
                    }
                }
            }
            else
            {
                //error employee not on a ticket
                returnResult = new EmployeeDepartmentPunchResult();
                returnResult.EmployeePunchSummaryInfo = employeePunchSummary;
                returnResult.PunchException = PunchExceptions.EmployeeNotAuthorized;
            }

            //record the swipe
            //check if we need to return the summary hours
            EmployeeDepartmentPunchResult finalResult = null;
            EmployeePunchDB epdb = new EmployeePunchDB();
            finalResult = epdb.RecordEmployeeDepartmentPunch(employeePunchSummary.TicketInfo.DepartmentInfo, returnResult, userPrincipal);

            //if its a dupe then return the previous type
            if (finalResult.DuplicatePunch)
            {
                finalResult.PunchType = Enums.PunchTypes.SameDepartment;
            }

            return finalResult;
        }
        public String UpdatePunch(string userID, string punchId, string month, string day, string year, string hour, string min)
        {
            DateTime dt = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day),
                                    Convert.ToInt32(hour), Convert.ToInt32(min), 0);
            EmployeePunchDB epdb = new EmployeePunchDB();
            return epdb.UpdatePunch(userID, Convert.ToInt32(punchId), dt);
        }
        public bool DeletePunch(string id, string punchID)
        {
            EmployeePunchDB epdb = new EmployeePunchDB();
            return epdb.DeletePunch(id, punchID);
        }
        public String SetGMPInfo(String csvStr)
        {
            RosterDB rdb = new RosterDB();
            return rdb.SetGMPInfo(csvStr);
        }

        public List<GMPInfo> GetGMPInfo(String s, String e, String c)
        {
            RosterDB rdb = new RosterDB();
            DateTime start = Convert.ToDateTime(s);
            DateTime end = Convert.ToDateTime(e);
            end = end.AddHours(23);
            end = end.AddMinutes(59);
            end = end.AddSeconds(59);
            int clientId = Convert.ToInt32(c);

            return rdb.GetGMPInfo(start, end, clientId);
        }
    }
}