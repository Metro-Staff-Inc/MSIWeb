namespace MSI.Web.MSINet.DataAccess
{
    public struct MSINetStoredProcs
    {
        public const string GetDepartmentSupervisors = "msinet_GetDepartmentSupervisors";
        public const string GetDaysPunches = "msinet_GetDaysPunches";
        public const string RetrievePunches = "msinet_RetrieveRostersAndPunches";
        public const string RetrievePunchExceptions = "msinet_RetrievePunchExceptions";
        public const string SetAidentICCardNum = "msinet_SetAidentICCardNum";
        public const string EmployeeList = "msinet_ClientEmployeeList";
        public const string ClientRosterLastUpdate = "msinet_ClientRosterLastUpdate";
        public const string CreateSuncastId = "msinet_GetClientTempNumberForAident";
        public const string GetSuncastId = "msinet_GetSuncastId";
        public const string ClearClientRoster = "msinet_ClearClientRoster";
        public const string LineApprove = "msinet_LineApprove";
        public const string GetDailyDispatch = "msinet_GetDailyDispatch";
        public const string DeleteDailyDispatch = "msinet_DeleteDailyDispatch";
        public const string UpdateDailyDispatch = "msinet_UpdateDailyDispatch";
        public const string GetEmployeeStatusReport = "msinet_GetEmployeeStatus";
        public const string GetTransportationInfo = "msinet_GetTransportationInfo";
        public const string GetVehicleUseInfo = "msinet_GetVehicleUseInfo";
        public const string GetDailyDriverData = "msinet_GetDailyDriverData";
        public const string GetClientTempNumberForAident = "msinet_GetClientTempNumberForAident";
        public const string InsertTransportation = "msinet_InsertTransportation";
        public const string GetDaysWorkedFirstPunch = "msinet_GetDaysWorkedFirstPunch";
        public const string GetDaysWorkedAndDNRStatus = "msinet_GetDaysWorkedAndDNRStatus";
        public const string GetDepartmentInfo = "msinet_GetDepartmentInfo";
        public const string GetClientDepartments = "msinet_GetClientDepartments";
        public const string SetDefaultClient = "msinet_SetDefaultClient";
        public const string GetDnrFirstPunch = "msinet_GetDnrFirstPunch";
        public const string UpdateEmployeeNotes = "msinet_UpdateEmployeeNotes";
        public const string GetEmployeeContact = "msinet_GetEmployeeContact";
        public const string UpdateEmployeeContact = "msinet_UpdateEmployeeContact";
        public const string GetWorkSchedule = "msinet_GetWorkSchedule";
        public const string UpdateEmployeeRole = "msinet_UpdateEmployeeRole";
        public const string GetALithoInfo = "msinet_GetALithoInfo";
        public const string UpdateResourceGroup = "msinet_UpdateResourceGroup";
        public const string SetGMPInfo = "msinet_UpdateJBGMP";
        public const string GetGMPInfo = "msinet_JBSSGMPDate";
        public const string GetResourceGroupHours = "msinet_GetResourceGroupHours";
        public const string GetResourceGroupInfo = "msinet_GetResourceGroupNames";
        public const string MovePunchDeptShift = "msinet_MovePunchDeptShift";
        public const string GetPunches = "msinet_GetPunches";
        public const string GetHoursReportPeekABoo = "msinet_GetHoursReportPeekABoo";
        public const string GetEmployeeNotes = "msinet_GetEmployeeNotes";
        public const string GetDispatchOffices = "msinet_GetDispatchOffices";
        public const string UpdateEmployeeHours = "msinet_UpdateEmployeeHours";
        public const string GetEmail = "msinet_GetEmail";
        public const string SetEmail = "msinet_SetEmail";
        public const string UpdatePhoneBlastList = "msinet_UpdatePhoneList";
        public const string GetSkillList = "msinet_GetSkillList";
        public const string SetSkillList = "msinet_SetSkillList";
        public const string GetSkillDescriptions = "msinet_GetSkillDescriptions";
        public const string CreateEmployeeHoursHeader = "msinet_CreateEmployeeHoursHeader";
        public const string GetETicketRosters = "msinet_GetETicketRosters";
        public const string GetEmployeeHours = "msinet_GetEmployeeHours";
        public const string SetPhoneBlast = "msinet_SetPhoneBlast";
        public const string GetPhoneBlast = "msinet_GetPhoneBlast";
        public const string GetPhoneBlastList = "msinet_GetPhoneBlastList";
        public const string GetPhoneBlastListNames = "msinet_GetPhoneBlastLists";
        public const string PunchesOutside = "msinet_PunchesOutside";
        public const string GetRecruitPool = "msinet_GetRecruitPool";
        public const string GetRecruitPoolFromRosters = "msinet_GetRecruitPoolFromRosters";
        public const string GetDepartments = "msinet_GetDepts";
        public const string ClientLocations = "msinet_ClientLocations";
        public const string SetClientMultiplier = "msinet_SetClientMultiplier";
        public const string UpdateShiftMappings = "msinet_UpdateShiftMappings";
        public const string GetShiftMapping = "msinet_GetShiftMapping";
        public const string GetGroupHoursReport = "msinet_GetGroupHoursReport";
        public const string AddPunch = "msinet_AddPunch";
        public const string ClientEmails = "msinet_GetClientEmail";
        public const string UpdateBonuses = "msinet_UpdateBonuses";
        public const string ValidateUser = "msinet_ValidateUser";
        public const string GetEmployeeRostersByShift = "msinet_GetEmployeeRostersByShift";
        public const string GetEmployeeRosters = "msinet_GetEmployeeRosters";
        public const string UpdateEmployeePunch = "msinet_UpdateEmployeePunch";
        public const string GetIDFromSuncastNum = "msinet_GetIDFromSuncastNum";
        public const string CheckIPClock = "msinet_CheckIPClock";
        public const string GetClientByUserName = "msinet_GetClientByUserName";
        public const string GetClientsByUserName = "msinet_GetClientsByUserName";
        public const string GetClientShiftTypes = "msinet_GetClientShiftTypes";
        public const string GetClientDepartmentsByShiftType = "msinet_GetClientDepartmentsByShiftType";
        public const string GetClientDepartmentsByShiftTypeByUser = "msinet_GetClientDepartmentsByShiftTypeByUser";
        public const string GetEmployeeTicketForPunch = "msinet_GetEmployeeTicketForPunch";
        public const string GetPunchExceptions = "msinet_GetPunchExceptions";
        public const string GetPunchMaintenanceReasons = "msinet_GetPunchMaintenanceReasons";
        public const string GetMinimumWageHistory = "msinet_GetMinimumWageHistory";
        public const string RecordEmployeePunch = "msinet_RecordEmployeePunch";
        public const string RecordEmployeeDepartmentPunch = "msinet_RecordEmployeeDepartmentPunch";
        public const string RecordEmployeePunchUpdateRoster = "msinet_RecordEmployeePunchUpdateRoster";
        public const string RecordEmployeePunchBiometricUpdateRoster = "msinet_RecordEmployeePunchBiometricUpdateRoster";
        public const string RecordEmployeePunchReturnSummary = "msinet_RecordEmployeePunchReturnSummary";
        public const string GetTicketTracking = "msinet_GetTicketTracking";
        public const string GetTicketTrackingAcrossDepartments = "msinet_GetTicketTrackingXDepts";
        public const string GetTicketTrackingByUser = "msinet_GetTicketTrackingByUser";
        public const string GetEmployeeHistory = "msinet_GetEmployeeHistory";
        public const string GetHoursReport = "msinet_GetHoursReport";
        public const string GetHoursReportSummary = "msinet_GetHoursReportSummary";
        public const string GetHoursReportSummaryByUser = "msinet_GetHoursReportSummaryByUser";
        public const string GetIndividualsHoursReportSummaryByUser = "msinet_GetIndividualsHoursReportSummaryByUser";
        public const string GetInvoiceHeader = "msinet_GetInvoiceHeader";
        public const string GetInvoiceDetail = "msinet_GetInvoiceDetail";
        public const string CreateInvoiceHeader = "msinet_CreateInvoiceHeader";
        public const string CreateInvoiceDetail = "msinet_CreateInvoiceDetail";
        public const string ApproveClientHours = "msinet_ApproveClientHours";
        public const string UnSubmitClientHours = "msinet_UnSubmitClientHours";
        public const string GetClientShifts = "msinet_GetClientShifts";
        public const string SaveEmployeePunch = "msinet_SaveEmployeePunch";
        public const string MoveEmployeePunch = "msinet_MoveEmployeePunch";
        public const string DeleteEmployeePunch = "msinet_DeleteEmployeePunch";
        public const string GetTicketTrackingEmployeeSummary = "msinet_GetTicketTrackingEmployeeSummary";
        public const string GetTicketTrackingExceptions = "msinet_GetTicketTrackingExceptions";
        public const string GetTicketTrackingExceptionsByEmployee = "msinet_GetTicketTrackingExceptionsByEmployee";
        public const string GetEmployeePunchMaintenance = "msinet_GetEmployeePunchMaintenance";
        public const string ApproveDailyHours = "msinet_ApproveDailyHours";
        public const string UnlockDailyHours = "msinet_UnlockDailyHours";
        public const string GetClientSwipeDepartments = "msinet_GetClientSwipeDepartments";
        public const string GetDepartmentPayRates = "msinet_GetDepartmentPayRates";
        public const string GetDepartmentJobCodes = "msinet_GetDepartmentJobCodes";
        public const string GetEmployeeByAident = "msinet_GetEmployeeByAident";
        public const string SetEmployee = "msinet_SetEmployee";
        public const string UpdateClientPayOverride = "msinet_UpdateClientPayOverride";
        public const string AddClientPayOverride = "msinet_AddClientPayOverride";
        public const string UpdateDepartmentPay = "msinet_UpdateDepartmentPay";
        public const string UpdateDepartmentJobCode = "msinet_UpdateDeparmentJobCode";
        public const string GetDaysWorkedReport = "msinet_GetDaysWorkedReport";
        public const string GetDaysWorkedReportSingleUser = "msinet_GetDaysWorkedReportSingleUser";
        public const string GetHeadCountReport = "msinet_GetHeadCountReport";
        public const string GetRosterAndHeadCountReport = "msinet_GetPunchesAndRoster";
        public const string GetHeadCountReportSummary = "msinet_GetHeadCountReportSummary";
        public const string GetHeadCountReportSummaryByUser = "msinet_GetHeadCountReportSummaryByUser";
        public const string ClientDNR_CheckForExistingRecords = "msinet_ClientDNR_CheckForExistingRecords";
        public const string ClientDNR_DeactivateEmployee = "msinet_ClientDNR_SetInactive";
        public const string ClientDNR_ActivateEmployee = "msinet_ClientDNR_ClearInactive";
        public const string SetClientPreferences = "msinet_SetClientPreferences";
        public const string ApprovePunchRange = "msinet_ApprovePunchRange";
        public const string GetClientPreferencesByID = "msinet_GetClientPreferencesByID";
        public const string GetPunchRecords = "msinet_GetPunchRecords";
        public const string GetPunchRecordCreators = "msinet_GetPunchRecordCreators";
        public const string UpdateDNR = "msinet_UpdateDNR";
        public const string RosterProgram = "msinet_Roster_Program";
        public const string GetActiveClients = "msinet_GetActiveClients";
        public const string GetShiftDepartment = "msinet_GetShiftDepartment";
        public const string UpdateClockTick = "msinet_UpdateClockTick";
        public const string InsertClockData = "msinet_InsertClockData";
    }
}
