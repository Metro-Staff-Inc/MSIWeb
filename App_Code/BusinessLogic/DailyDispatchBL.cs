using MSI.Web.MSINet.BusinessEntities;
using MSI.Web.MSINet.DataAccess;
////using MSIToolkit.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DailyDispatchBL
/// </summary>
/// 
namespace MSI.Web.MSINet.BusinessLogic
{
    public class DailyDispatchBL
    {
        public DailyDispatchBL()
        {
        }
        ////PerformanceLogger log = new PerformanceLogger("ADONetAppender");

        public string updateDailyDispatchData(List<DailyDispatchInfo> data)
        {
            /*
             * 		exec('insert into ##dailyDispatch(client_id, dispatch_dt, office_id, office_cd, shift_type, tot_sent, regs, temps_ordered, temps_sent, unfilled, extras, notes, created_by, created_dt) values' + @values);

		--SELECT 'records already in table'
		UPDATE daily_dispatch 
		SET tot_sent = dIN.tot_sent, regs = dIN.regs, temps_ordered = dIN.temps_ordered, office_id = dIN.office_id, office_cd = dIN.office_cd, 
				temps_sent = dIN.temps_sent, extras = dIN.extras, unfilled = dIN.unfilled, notes = dIN.notes, created_by = dIN.created_by, created_dt = GetDate(), void = null
		FROM ##dailyDispatch dIN JOIN daily_dispatch dTbl ON dIn.client_id = dTbl.client_id AND dIn.shift_type = dTbl.shift_type AND dIn.dispatch_dt = dTbl.dispatch_dt AND dIN.office_id = dTbl.office_id

		--SELECT 'records not yet in table'
		INSERT INTO daily_dispatch(client_id, dispatch_dt, office_id, office_cd, shift_type, tot_sent, regs, temps_ordered, temps_sent, extras, unfilled, notes, created_by, created_dt)
		SELECT d.client_id, d.dispatch_dt, d.office_id, d.office_cd, d.shift_type, d.tot_sent, d.regs, d.temps_ordered, d.temps_sent, d.extras, d.unfilled, d.notes, d.created_by, GetDate()
		FROM ##dailyDispatch d LEFT JOIN daily_dispatch dTbl ON d.client_id = dTbl.client_id AND d.shift_type = dTbl.shift_type AND d.dispatch_dt = dTbl.dispatch_dt AND d.office_id = dTbl.office_id
		WHERE dTbl.client_id is null

        */
            /* client_id, dispatch_dt, office_id, office_cd, shift_type, tot_sent, regs, 
             * temps_ordered, temps_sent, unfilled, extras, notes, created_by, created_dt */
            string value = "";
            for( int i=0; i<data.Count; i++ )
            {
                for( int j=i+1; j<data.Count; j++ )
                {
                    if( data[i].clientId == data[j].clientId && data[i].shiftType == data[i].shiftType &&
                        data[i].officeCd == data[j].officeCd && data[i].dispatchDt == data[j].dispatchDt )
                    {
                        data[j].clientId = -1;  //ignore this client
                        data[i].notes += " " + data[j].notes;
                        data[i].totSent += data[j].totSent;
                        data[i].regs += data[j].regs;
                        data[i].tempsOrdered += data[j].tempsOrdered;
                        data[i].tempsSent += data[j].tempsSent;
                        data[i].unfilled += data[j].unfilled;
                        data[i].extras += data[j].extras;
                    }
                }
            }
            for( int i=0; i<data.Count; i++ )
            {
                DailyDispatchInfo d = data[i];
                if ( d.clientId <= 0 ) continue;    //this client was combined with it's duplicate.
                value += "(" + d.clientId + ",'" + d.dispatchDt + "'," + d.officeId + ",'" +
                    d.officeCd.Substring(1) + "'," + d.shiftType + "," + d.totSent + "," + d.regs + "," +
                    d.tempsOrdered + "," + d.tempsSent + "," + d.unfilled + "," + d.extras + ",'" +
                    d.notes + "','" + d.createdBy + "','" + d.createdDt + "','" + d.transported + "')";
                if( i<data.Count-1 )
                {
                    value += ", ";
                }
            }
            DailyDispatchDB ddb = new DailyDispatchDB();
            return ddb.UpdateDailyDispatchData(value);
        }

        public List<DailyDispatchInfo> getDailyDispatchData(string dispatchDt, string officePrefix, int shiftType, bool weeklyReport)
        {
            if ( officePrefix.Length == 2 )
            {
                officePrefix = officePrefix.Substring(1);
            }
            int year = Convert.ToInt32(dispatchDt.Substring(0, 4));
            int month = Convert.ToInt32(dispatchDt.Substring(5, 2));
            int day = Convert.ToInt32(dispatchDt.Substring(8));
            DateTime date = new DateTime(year, month, day);
            DailyDispatchDB ddb = new DailyDispatchDB();
            return ddb.getDailyDispatchData(date, officePrefix, shiftType, weeklyReport);
        }
    }
}